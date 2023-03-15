using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace DrSearch
{
    class IndexManager
    {
        private readonly String pathToIndex; //path to index file (CRAWLED, PRIMARY)
        private readonly String pathToIndexTest; //path to index with test data (MOSTLY FOR BENCHMARK)
        private readonly List<String> stopWords; //stopwords retrieved from https://countwordsfree.com/stopwords/czech

        //vars regarding to currently loaded index - START
        public short loadedIndexType { get; set; } //type of loaded index
        public Dictionary<String, List<Int32>> validDocumentsNumsLoaded { get; set; } //loaded index; key: word present in atleast one document; value: index 0 = total occ num across docs, index >= 1 = document number, which countains the given word
        public List<Dictionary<String, Int32>> individualDocsCountsLoaded { get; set; } //loaded index; represents content of indiv. docs - key: word present in the doc, value: number of occ of the word in the doc
        public List<Double> tfIdfDocsLoaded { get; set; } //loaded index; tfIdf counts for each document - only when Vector space model is active (useless in case of Boolean model)
        public List<Article> articlesLoaded { get; set; } //loaded docs in form of articles
        //vars regarding to currently loaded index - END
        public DateTime lastReturnedIndexCreationDate { get; set; } //creation date of index which was returned as last 
        private TfIdfCore tfIdfCalc; //component responsible for tf-idf calculations
        private CrawlerCore crawlerCore; //component responsible for crawling the selected web page

        public Dictionary<String, List<Int32>> lastSavedvalidDocumentsNums { get; set; } //last saved index
        public List<Dictionary<String, Int32>> lastSavedindividualDocsCounts { get; set; } //last saved index
        private List<Double> lastSavedTfIdfDocs { get; set; }

        public Thread indexThread { get; set; } //thread used for indexing
        private DashboardForm dashboardForm; //form with the dashboard

        /// <summary>
        /// Constructor takes just one param, which tells location and name of the file with index.
        /// </summary>
        /// <param name="pathToIndex">path to primary index file</param>
        /// <param name="pathToIndexTest">path to index file with test data (for bench)</param>
        /// <param name="tfIdfCalc">component used for tf-idf calculations</param>
        /// <param name="dashboardFormRef">form with the dashboard</param>
        public IndexManager(String pathToIndex, String pathToIndexTest, TfIdfCore tfIdfCalc, DashboardForm dashboardFormRef)
        {
            this.stopWords = SupportTools.loadStopWordsFromFile();
            this.pathToIndex = pathToIndex;
            this.pathToIndexTest = pathToIndexTest;
            this.tfIdfCalc = tfIdfCalc;
            this.dashboardForm = dashboardFormRef;

            this.loadedIndexType = -1;
            this.validDocumentsNumsLoaded = null;
            this.individualDocsCountsLoaded = null;
            this.tfIdfDocsLoaded = null;
            this.articlesLoaded = null;

            this.indexThread = new Thread(new ThreadStart(performDataIndex)); //init indexing thread, without starting
            this.lastReturnedIndexCreationDate = new DateTime();
        }

        /// <summary>
        /// Used for assigning crawlerCore object to instance.
        /// </summary>
        /// <param name="crawlerCore">component responsible for crawling the selected web page</param>
        public void assignCrawlerCore(CrawlerCore crawlerCore)
        {
            this.crawlerCore = crawlerCore;
        }

        /// <summary>
        /// Returns true if loaded index contains weights -> mode is: INDEX_DRSEARCH_VECTOR_SPACE_MODE_TEST or INDEX_DRSEARCH_VECTOR_SPACE_MODE_CRAWL.
        /// </summary>
        /// <returns>true if index contains weights, else false</returns>
        public bool isLoadedIndexTfIdf()
        {
            if(loadedIndexType.Equals("INDEX_DRSEARCH_VECTOR_SPACE_MODE_TEST") || loadedIndexType.Equals("INDEX_DRSEARCH_VECTOR_SPACE_MODE_CRAWL"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if index contains crawled data (NOT test ones...) -> mode is: INDEX_DRSEARCH_VECTOR_SPACE_MODE_CRAWL or INDEX_DRSEARCH_BOOLEAN_MODE_CRAWL
        /// </summary>
        /// <returns></returns>
        public bool isLoadedIndexCrawl()
        {
            if(loadedIndexType.Equals("INDEX_DRSEARCH_VECTOR_SPACE_MODE_CRAWL") || loadedIndexType.Equals("INDEX_DRSEARCH_BOOLEAN_MODE_CRAWL"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Loads indexed documents from file.
        /// </summary>
        /// <param name="testData">false if index should be loaded from index.drsearch ; true for indexTestData.drsearch</param>
        private void reloadIndexFromFile(bool testData){
            String pathToIndex;
            if (!testData)
            {
                pathToIndex = this.pathToIndex;
            }
            else
            {
                pathToIndex = this.pathToIndexTest;
            }

            Dictionary<String, List<Int32>> validDocumentsNums = new Dictionary<String, List<Int32>>(); //key: word present in atleast one document; value: index 0 = total occ num across docs, index >= 1 = document number, which countains the given word
            List<Dictionary<String, Int32>> individualDocsCounts = new List<Dictionary<String, Int32>>(); //represents content of indiv. docs - key: word present in the doc, value: number of occ of the word in the doc
            List<Double> tfIdfDocs = new List<Double>(); //tfIdf counts for each document - only when Vector space model is active (useless in case of Boolean model)
            bool tfidfActive = false; //true when START_TFIDF_NUMS_DRSEARCH flag set, start tfidf nums loading

            String[] indexCont = null; //all lines present in index document

            try
            {
                indexCont = File.ReadAllLines(pathToIndex);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Err while reading index file. Err: " + e);
            }

            String indexMode = indexCont.ElementAt(0); //contains mode of index
            if (indexMode.Equals("INDEX_DRSEARCH_VECTOR_SPACE_MODE_TEST"))
            {
                loadedIndexType = 0;
            }
            else if (indexMode.Equals("INDEX_DRSEARCH_BOOLEAN_MODE_TEST"))
            {
                loadedIndexType = 1;
            }
            else if (indexMode.Equals("INDEX_DRSEARCH_VECTOR_SPACE_MODE_CRAWL"))
            {
                loadedIndexType = 2;
            }
            else if (indexMode.Equals("INDEX_DRSEARCH_BOOLEAN_MODE_CRAWL"))
            {
                loadedIndexType = 3;
            }

            //parse first part of index, format: WORD TOTAL_OCC_COUNT, DOC_NUM_X, DOC_NUM_X - START
            int secondPartStart = -1; //index where starts second part of index (in indexCont)

            for (int i = 2; i < indexCont.Length; i++) //go through the index count till END_DOC_INDEX_GLOBAL_DRSEARCH is encountered - end of first part of index
            {
                String actualLine = indexCont.ElementAt(i);
                if (actualLine.Equals("END_DOC_INDEX_GLOBAL_DRSEARCH")) //end of first part encountered
                {
                    secondPartStart = i + 1; //start on next pos
                    break;
                }
                else //found regular cont of first part of index
                {
                    String[] lineCont = actualLine.Split(' '); //first is word ; second index contains num of occ + document nums
                    String word = lineCont[0];
                    String[] numOccDocNumsStr = lineCont[1].Split(','); //first = num of occ in all docs, >= second = nuber of doc which contains the given word

                    List<Int32> numOccDocNumsInt = Array.ConvertAll(numOccDocNumsStr, int.Parse).ToList(); //convert nums in string to int

                    validDocumentsNums.Add(word, numOccDocNumsInt);
                }
            }
            //parse first part of index, format: WORD TOTAL_OCC_COUNT, DOC_NUM_X, DOC_NUM_X - END

            //parse second part of index, format: WORD OCC_COUNT_IN_THE_DOC - START
            Dictionary<String, Int32> oneDocCont = new Dictionary<String, Int32>(); //content of the one, currently parsed doc
            for (int i = secondPartStart; i < indexCont.Length; i++)
            {
                String actualLine = indexCont.ElementAt(i);
                if (actualLine.Equals("START_DOC_INDEX_INDIVIDUAL_DRSEARCH")) //start of new indiv. doc
                {
                    oneDocCont = new Dictionary<String, Int32>();
                }
                else if(actualLine.Equals("END_DOC_INDEX_INDIVIDUAL_DRSEARCH")) //add cont of indiv. doc to list
                {
                    individualDocsCounts.Add(oneDocCont);
                }
                else if(actualLine.Equals("START_TFIDF_NUMS_DRSEARCH")) //start of tfidf nums (ONLY WHEN TFIDF INDEX)
                {
                    tfidfActive = true;
                }
                else if(actualLine.Equals("END_TFIDF_NUMS_DRSEARCH")) //end of tfidf nums (ONLY WHEN TFIDF INDEX)
                {
                    break; //must be EOF
                }
                else if(tfidfActive)
                {
                    tfIdfDocs.Add(double.Parse(actualLine));
                }
                else //regular line with doc cont found, add to inner Dictionary
                {
                    String[] lineCont = actualLine.Split(' '); //first is word ; second index contains number of occ
                    String word = lineCont[0]; //word present in the doc
                    int numberOcc = int.Parse(lineCont[1]); //number of occ in one doc
                    oneDocCont.Add(word, numberOcc);
                }
            }
            //parse second part of index, format: WORD OCC_COUNT_IN_THE_DOC - END

            //assign parsed info to global vars
            this.validDocumentsNumsLoaded = validDocumentsNums;
            this.individualDocsCountsLoaded = individualDocsCounts;
            this.tfIdfDocsLoaded = tfIdfDocs;

            if (!testData)
            {
                this.articlesLoaded = SupportTools.transCrawlToArticles(); //List with Article objects
            }
            else
            {
                this.articlesLoaded = SupportTools.transTestToArticles();
            }

            //set type of index
            switch (loadedIndexType)
            {
                case 0:
                    dashboardForm.changeLoadedIndexInfo("Currently loaded index: test data + vector space");
                    break;

                case 1:
                    dashboardForm.changeLoadedIndexInfo("Currently loaded index: test data + boolean model");
                    break;

                case 2:
                    dashboardForm.changeLoadedIndexInfo("Currently loaded index: crawled data + vector space");
                    break;

                case 3:
                    dashboardForm.changeLoadedIndexInfo("Currently loaded index: crawled data + boolean model");
                    break;
            }
        }

        /// <summary>
        /// Saves given index to file - index.
        /// </summary>
        /// <param name="validDocumentsNums">key: word present in atleast one document; value: index 0 = total occ num across docs, index >= 1 = document index, which countains the given word</param>
        /// <param name="individualDocsCounts">represents content of indiv. docs - key: word present in the doc, value: number of occ of the word in the doc</param>
        /// <param name="tfidf">true if tfidf should be calculated and included, else false</param>
        /// <param name="testData">true if test data are indexed, else false</param>
        public void saveIndexToFile(Dictionary<String, List<Int32>> validDocumentsNums, List<Dictionary<String, Int32>> individualDocsCounts, bool tfidf, bool testData)
        {
            lastSavedvalidDocumentsNums = validDocumentsNums;
            lastSavedindividualDocsCounts = individualDocsCounts;

            String pathToIndex;
            if (!testData)
            {
                pathToIndex = this.pathToIndex;
            }
            else
            {
                pathToIndex = this.pathToIndexTest;
            }

            //remove previously created index, if any
            File.Delete(pathToIndex);

            List<String> validDocumentsNumsKeys = validDocumentsNums.Keys.ToList(); //get list of all words present across docs
            List<List<Int32>> validDocumentsNumsValues = validDocumentsNums.Values.ToList(); //get nums of occ of all words across docs + docs in which word appears

            String modeCont = ""; //write selected index mode at the beginning of the file
            if(tfidf && testData)
            {
                modeCont = "INDEX_DRSEARCH_VECTOR_SPACE_MODE_TEST";
            }
            else if(!tfidf && testData)
            {
                modeCont = "INDEX_DRSEARCH_BOOLEAN_MODE_TEST";
            }
            else if(tfidf && !testData)
            {
                modeCont = "INDEX_DRSEARCH_VECTOR_SPACE_MODE_CRAWL";
            }
            else if(!tfidf && !testData)
            {
                modeCont = "INDEX_DRSEARCH_BOOLEAN_MODE_CRAWL";
            }
            using (StreamWriter sw = File.AppendText(pathToIndex))
            {
                sw.Write(modeCont + Environment.NewLine);
            }

            //start of global index
            using (StreamWriter sw = File.AppendText(pathToIndex))
            {
                sw.Write("START_DOC_INDEX_GLOBAL_DRSEARCH" + Environment.NewLine);
            }

            String globalIndexCont = ""; //create first part of index; format: WORD TOTAL_OCC_COUNT, DOC_NUM_X, DOC_NUM_X
            for (int i = 0; i < validDocumentsNums.Count; i++) //go through all entries in global Dictionary and prepare global index
            {
                globalIndexCont += validDocumentsNumsKeys.ElementAt(i); //word
                globalIndexCont += " ";
                globalIndexCont += string.Join(",", validDocumentsNumsValues.ElementAt(i)); //total occ + document nums
                globalIndexCont += Environment.NewLine;

                //end of global index
                using (StreamWriter sw = File.AppendText(pathToIndex))
                {
                    sw.Write(globalIndexCont);
                }
                globalIndexCont = "";
            }

            //end of global index
            using (StreamWriter sw = File.AppendText(pathToIndex))
            {
                sw.Write("END_DOC_INDEX_GLOBAL_DRSEARCH" + Environment.NewLine);
            }

            String indivDocsIndexCont = ""; //create sec part of index, cont of individual docs; format: WORD OCC_COUNT_IN_THE_DOC
            for(int i = 0; i < individualDocsCounts.Count(); i++) //go through individual docs content
            {
                Dictionary<String, Int32> oneDocCont = individualDocsCounts.ElementAt(i); //get content relevant to one doc
                List<String> oneDocContKeys = oneDocCont.Keys.ToList(); //list of words present in one doc
                List<Int32> oneDocContValues = oneDocCont.Values.ToList(); //number of occ of word in the doc

                indivDocsIndexCont += "START_DOC_INDEX_INDIVIDUAL_DRSEARCH" + Environment.NewLine;
                for (int j = 0; j < oneDocCont.Count; j++)
                { //go through word + occurances for ONE document
                    indivDocsIndexCont += oneDocContKeys.ElementAt(j); //word
                    indivDocsIndexCont += " ";
                    indivDocsIndexCont += oneDocContValues.ElementAt(j); //total occ of the word in doc
                    indivDocsIndexCont += Environment.NewLine;
                }
                indivDocsIndexCont += "END_DOC_INDEX_INDIVIDUAL_DRSEARCH" + Environment.NewLine;

                //individual index prepared -> write to file
                using (StreamWriter sw = File.AppendText(pathToIndex))
                {
                    sw.Write(indivDocsIndexCont);
                }
                indivDocsIndexCont = "";
            }

            if (!tfidf) //do not write calcs
            {
                return;
            }

            //at the end of the index file will be cos sim counts for individual documents = one line = one num
            String tfidfIndexCont = ""; //last part of index (saved only when tfidf indexing is used)
            lastSavedTfIdfDocs = new List<Double>();

            int counter = 0;
            foreach (Dictionary<String, Int32> oneDoc in individualDocsCounts) //go through inner dictionaries which represent individual indexed docs
            {
                double docCosSize = 0;

                List<String> wordsInDoc = oneDoc.Keys.ToList(); //words present in the document
                for (int j = 0; j < wordsInDoc.Count; j++)
                {
                    String wordInDoc = wordsInDoc.ElementAt(j);
                    double tfidfRes = tfIdfCalc.retrieveTfIdf(counter, validDocumentsNums, individualDocsCounts, wordInDoc);
                    docCosSize += (tfidfRes * tfidfRes);
                }

                tfidfIndexCont += Math.Sqrt(docCosSize) + Environment.NewLine;
                lastSavedTfIdfDocs.Add(Math.Sqrt(docCosSize));
                counter++;
            }

            //tfidf nums prepared -> write to file
            using (StreamWriter sw = File.AppendText(pathToIndex))
            {
                sw.Write("START_TFIDF_NUMS_DRSEARCH" + Environment.NewLine + tfidfIndexCont + "END_TFIDF_NUMS_DRSEARCH");
            }
        }

        /// <summary>
        /// Checks whether CRAWL DATA were already indexed or not. That means: file with index exists.
        /// </summary>
        /// <returns>true if data indexed, else false</returns>
        public bool isDataIndexed()
        {
            String pathToIndexFile = "index.drsearch"; //path to file with indexed data
            if (File.Exists(pathToIndexFile)) //if file with index data exists
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether TEST DATA were already indexed or not. That means: file with index exists.
        /// </summary>
        /// <returns>true if data indexed, else false</returns>
        public bool isTestDataIndexed()
        {
            String pathToIndexFile = "indexTestData.drsearch"; //path to file with indexed data
            if (File.Exists(pathToIndexFile)) //if file with index data exists
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieve timestamp of index file; when file was lastly edited.
        /// </summary>
        /// <returns>timestamp of index file</returns>
        public DateTime retrieveIndexCreationDate()
        {
            return new FileInfo(pathToIndex).LastWriteTime;
        }

        /// <summary>
        /// Triggered when button for index test data is clicked. Reloads index with test data for bench.
        /// </summary>
        public void performTestIndexReload()
        {
            if (!isTestDataIndexed()) //crawled data OR file with index does not exist, perform crawling + index
            {
                DialogResult noDataIndexedRes = MessageBox.Show("Test data not yet indexed." + Environment.NewLine + "Do you want to index test data now?", "No index", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (noDataIndexedRes == DialogResult.Yes) //perform data crawling + indexing
                {
                    performTestDataIndex();
                }
                return; //nothing to be returned
            }

            //file with index exists = index can be loaded, ok
            dashboardForm.appendToProgInfoTBBuffer("***LOAD TEST INDEX FROM FILE - START***");
            reloadIndexFromFile(true);

            dashboardForm.appendToProgInfoTBBuffer("Loaded index with " + individualDocsCountsLoaded.Count + " documents");
            dashboardForm.appendToProgInfoTBBuffer("***LOAD TEST INDEX FROM FILE - END***");
        }

        /// <summary>
        /// Is executed when button for loading crawled data index is clicked. Takes care of loading crawled index content.
        /// </summary>
        /// <returns></returns>
        public void performCrawlIndexReload()
        {
            DateTime crawlDataCreationTime = CrawlerCore.retrieveDataCrawlTime();

            if (!isDataIndexed() || crawlDataCreationTime == DateTime.MinValue) //crawled data OR file with index does not exist, perform crawling + index
            {
                DialogResult noDataIndexedRes = MessageBox.Show("Data not yet crawled." + Environment.NewLine + "Please perform data crawl first!", "No data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; //nothing to be returned
            }

            //file with index exists = index can be loaded, ok
            dashboardForm.appendToProgInfoTBBuffer("***LOAD CRAWL INDEX FROM FILE - START***");
            reloadIndexFromFile(false);

            dashboardForm.appendToProgInfoTBBuffer("Loaded index with " + individualDocsCountsLoaded.Count + " documents");
            dashboardForm.appendToProgInfoTBBuffer("***LOAD CRAWL INDEX FROM FILE - END***");
        }

        /// <summary>
        /// Checks whether test data index is present. File: test_index.drsearch 
        /// </summary>
        /// <returns></returns>
        private bool isTestDataIndexPresent()
        {
            String pathToTestIndex = ".\\indexTestData.drsearch";
            if (File.Exists(pathToTestIndex)) //if files with crawled data exist + index exists
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether files with test data are present. File: downloaded_data/czechData.json + downloaded_data/topicData.json
        /// </summary>
        /// <returns></returns>
        private bool isTestDataPresent()
        {
            String pathToTestIndex1 = ".\\downloaded_data\\czechData.json";
            String pathToTestIndex2 = ".\\downloaded_data\\topicData.json";

            if (File.Exists(pathToTestIndex1) && File.Exists(pathToTestIndex2)) //index with test data: czech data + topic data
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Performs indexing of test data.
        /// </summary>
        public void performTestDataIndex()
        {
            if (isTestDataIndexPresent()) { //test data already indexed
                DialogResult noDataCrawledRes = MessageBox.Show("Test data already indexed." + Environment.NewLine + "Do you want really want to reindex them?", "Indexed", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (noDataCrawledRes == DialogResult.No) //do not reindex
                {
                    return;
                }
            }

            //index with test data not present or user wants to reindex -> go on - check if source files are present
            if (!isTestDataPresent())
            {
                DialogResult noDataCrawledRes = MessageBox.Show("Test data (or queries) are not present at all!" + Environment.NewLine + "Input file czechData.json OR topicData.json not found in downloaded_data!", "No input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //file present but not indexed -> index
            List<String> inputFileTestDataCzech = SupportTools.retrieveInputFile(".\\downloaded_data\\czechData.json"); //file: czechData.json
            dashboardForm.appendToProgInfoTBBuffer("***INDEX TEST DATA TO FILE - START***");
            List<Article> inputFileTestDataCzechArticles = SupportTools.transTestToArticles(); //List with Article objects - suitable for indexing
            List<String> inputFileTextToIndex = new List<String>(); //text which should be indexed from input file - text
            for(int i = 0; i < inputFileTestDataCzechArticles.Count; i++)
            {
                String textToIndex = inputFileTestDataCzechArticles.ElementAt(i).TidyText.ToLower();
                //preprocess - START
                foreach (string stopword in stopWords)
                {
                    if (textToIndex.Contains(" " + stopword + " "))
                    {
                        textToIndex = textToIndex.Replace(" " + stopword + " ", "");
                    }
                }
                textToIndex = applyPreprocessing(textToIndex);
                //preprocess - END
                inputFileTextToIndex.Add(String.Copy(textToIndex)); //get text attr
            }

            List<Dictionary<String, Int32>> wordOccuranceInDocs = SupportTools.retrieveOccuranceWordInIndivDocs(inputFileTextToIndex);
            Dictionary<String, List<Int32>> wordOccuranceGlobal = SupportTools.retrieveOccuranceWordGlobal(wordOccuranceInDocs);
            saveIndexToFile(wordOccuranceGlobal, wordOccuranceInDocs, dashboardForm.indexVectorMode, true); //if vector mode selected -> perform tfidf calculations
            dashboardForm.appendToProgInfoTBBuffer("Created index with " + lastSavedindividualDocsCounts.Count + " documents");
            dashboardForm.appendToProgInfoTBBuffer("***INDEX TEST DATA TO FILE - END***");
        }

        /// <summary>
        /// Starts process of data indexing. Indexing can be performed only if data crawling process has already been finished.
        /// </summary>
        public void performDataIndex()
        {
            //continue with data indexing, data crawled - ok
            //load input from crawler; just documents to index without any queries
            List<String> inputFilearticlesdetailAllText = SupportTools.retrieveInputFile(".\\downloaded_data\\articlesdetail_allText.txt"); //file: articlesdetail_allText.txt ; crawled article text without respect to linebreaks
            dashboardForm.appendToProgInfoTBBuffer("***INDEX DATA RETRIEVED VIA CRAWLER TO FILE - START***");
            //transform crawled article data to form of Article objects - START
            List<Article> crawledArticlesAllText = SupportTools.transCrawlToArticles(); //List with Article objects
            List<String> crawledArticlesAllTextIndex = new List<String>(); //List with documents to index in plaintext
            for (int i = 0; i < crawledArticlesAllText.Count; i++) //go through retrieved articles
            {
                Article traversedArticle = crawledArticlesAllText.ElementAt(i);
                String textToIndex = traversedArticle.AllText.ToLower();
                //preprocess - START
                foreach(string stopword in stopWords)
                {
                    if (textToIndex.Contains(" " + stopword + " "))
                    {
                        textToIndex = textToIndex.Replace(" " + stopword + " ", "");
                    }
                }
                textToIndex = applyPreprocessing(textToIndex);
                //preprocess - END

                crawledArticlesAllTextIndex.Add(String.Copy(textToIndex)); //add text of document to index to list of all docs
            }

            //index article content´s - allText, without respect to linebreaks
            List<Dictionary<String, Int32>> wordOccuranceInDocs = SupportTools.retrieveOccuranceWordInIndivDocs(crawledArticlesAllTextIndex);
            Dictionary<String, List<Int32>> wordOccuranceGlobal = SupportTools.retrieveOccuranceWordGlobal(wordOccuranceInDocs);

            saveIndexToFile(wordOccuranceGlobal, wordOccuranceInDocs, dashboardForm.indexVectorMode, false); //if vector mode selected -> perform tfidf calculations
            dashboardForm.appendToProgInfoTBBuffer("Created index with " + lastSavedindividualDocsCounts.Count + " documents");
            dashboardForm.appendToProgInfoTBBuffer("***INDEX DATA RETRIEVED VIA CRAWLER TO FILE - END***");
        }

        /// <summary>
        /// Applies preprocessing to given document.
        /// </summary>
        /// <param name="textToPreprocess">text to which preprocessing should be applied</param>
        /// <returns></returns>
        public String applyPreprocessing(String textToPreprocess)
        {
            textToPreprocess = textToPreprocess.ToLower();
            //remove diacritics - START
            String normalText = textToPreprocess.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char characterInText in normalText)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(characterInText);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(characterInText);
                }
            }
            String textWODia = sb.ToString().Normalize(NormalizationForm.FormC); //text without diacritics
            //remove diacritics - END
            String[] splittedWODiaText = Regex.Split(textWODia, "\\s+|(?=[,.])");
            String stemmedText = "";

            //split orig text to indiv. word and apply stemming
            foreach (String word in splittedWODiaText)
            {
                if(word.Length <= 1)
                {
                    continue;
                }

                String preprocessedWord = PreprocessingTools.removeCase(word);
                preprocessedWord = PreprocessingTools.removePossessives(preprocessedWord);
                preprocessedWord = PreprocessingTools.removeComparative(preprocessedWord);
                preprocessedWord = PreprocessingTools.removeDiminutive(preprocessedWord);
                preprocessedWord = PreprocessingTools.removeAugmentative(preprocessedWord);
                preprocessedWord = PreprocessingTools.removeDerivational(preprocessedWord);
                if(preprocessedWord[0] == ',' || preprocessedWord[0] == '.' || preprocessedWord[0] == '(' || preprocessedWord[0] == ')' || preprocessedWord[0] == '[' || preprocessedWord[0] == ']')
                {
                    preprocessedWord = preprocessedWord.Remove(0, 1);
                }

                if (preprocessedWord[preprocessedWord.Length - 1] == ',' || preprocessedWord[preprocessedWord.Length - 1] == '.' || preprocessedWord[preprocessedWord.Length - 1] == '(' || preprocessedWord[preprocessedWord.Length - 1] == ')' || preprocessedWord[preprocessedWord.Length - 1] == '[' || preprocessedWord[preprocessedWord.Length - 1] == ']')
                {
                    preprocessedWord = preprocessedWord.Remove(preprocessedWord.Length - 1, 1);
                }

                stemmedText += " " + preprocessedWord + " ";
            }
            stemmedText = stemmedText.Trim();

            return stemmedText;
        }
    }
}