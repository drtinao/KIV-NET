using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace DrSearch
{
    /// <summary>
    /// Contains just static methods which are used for various tasks in the program.
    /// </summary>
    class SupportTools
    {
        public static int traversedIndexTree { get; set; } //actual traversed index in tree - used in createExprTree method

        /// <summary>
        /// Read content of input text file and return its content in list with strings.
        /// </summary>
        /// <param name="path">path to input text file, which should be read and parsed.</param>
        /// <returns>list with strings, which contains all lines present in input text document</returns>
        public static List<String> retrieveInputFile(String path)
        {
            String[] fileToRead = null; //retrieve all lines which are present in the document which should be indexed in form of string arr
            List<String> docToIndex = null; //convert string arr with doc content to list for better future manipulation

            try
            {
                fileToRead = File.ReadAllLines(path);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Err while reading file. Err: " + e);
            }

            if (fileToRead != null) //file read ok
            {
                docToIndex = new List<String>(fileToRead);
            }
            else //file read err
            {
                Console.WriteLine("Err while reading file.");
            }

            return docToIndex;
        }

        /// <summary>
        /// Creates List with inner Dictionaries which represent content of individual docs. Each Dictionary contains words present in the one doc (key: word, value: number of occurance of the given word in the doc).
        /// </summary>
        /// <param name="docsToIndex">documents to be indexed</param>
        /// <returns></returns>
        public static List<Dictionary<String, Int32>> retrieveOccuranceWordInIndivDocs(List<String> docsToIndex)
        {
            List<Dictionary<String, Int32>> occInDocs = new List<Dictionary<String, Int32>>(); //outer list = docs ; inner dictionary = one doc content

            for (int i = 0; i < docsToIndex.Count; i++)
            {//go through all crawled docs
                String oneDoc = docsToIndex.ElementAt(i);

                List<String> innerList = Regex.Split(oneDoc, "\\s+").ToList(); //represents words, terms in one document
                Dictionary<String, Int32> oneDocCont = new Dictionary<String, Int32>(); //create Dict for new document
                foreach (String wordInDoc in innerList)
                { //go through words in one document and set them as present
                    String wordInDocLower = wordInDoc.ToLower();

                    if (oneDocCont.ContainsKey(wordInDocLower))
                    { //key already present, increment occ by 1
                        oneDocCont[wordInDocLower] = oneDocCont[wordInDocLower] + 1;
                    }
                    else
                    { //key NOT found, create with 1
                        oneDocCont.Add(wordInDocLower, 1);
                    }
                }
                occInDocs.Add(oneDocCont); //add content of one doc to global list
            }

            return occInDocs;
        }

        /// <summary>
        /// Builds global dictionary with word occ count across all docs. Using given dictionaries, which reflect state of individual documents.
        /// </summary>
        /// <param name="indivDocsOccurance">dictionaries in which: keys - word present in the doc, values - number of occ in the document</param>
        /// <returns>dictionary with global counts of words + docs, which contain the given word</returns>
        public static Dictionary<String, List<Int32>> retrieveOccuranceWordGlobal(List<Dictionary<String, Int32>> indivDocsOccurance)
        {
            Dictionary<String, List<Int32>> globalOccInDocs = new Dictionary<String, List<Int32>>();

            for (int i = 0; i < indivDocsOccurance.Count; i++) //go through individual docs content
            {
                Dictionary<String, Int32> oneDocCont = indivDocsOccurance.ElementAt(i);
                List<String> oneDocContKeys = oneDocCont.Keys.ToList(); //list of words present in one doc
                List<Int32> oneDocContValues = oneDocCont.Values.ToList(); //number of occ of word in the doc

                for (int j = 0; j < oneDocCont.Count; j++)
                { //go through word + occurances for ONE document
                    String wordInDoc = oneDocContKeys.ElementAt(j); //word
                    int numOfOcc = oneDocContValues.ElementAt(j); //total occ of the word in doc

                    if (globalOccInDocs.ContainsKey(wordInDoc)) //word already exists in global dict, upgrade occ num
                    {
                        List<Int32> totalCountDocs = globalOccInDocs[wordInDoc]; //index 0 = total num of occ across docs; >= index 1 numberś of docs which contain the given word
                        totalCountDocs[0] += numOfOcc; //update number of occ across docs

                        List<Int32> presentInDocs = new List<int>(totalCountDocs);
                        presentInDocs.RemoveAt(0); //remove first, collections now contains only nums of docs which contain the given word
                        if (!presentInDocs.Contains(i))
                        { //if list doesnt contain the document yet, add it
                            totalCountDocs.Add(i);
                        }

                        globalOccInDocs[wordInDoc] = totalCountDocs;
                    }
                    else //word not yet present in global dict, add new entry
                    {
                        List<Int32> totalCountDocs = new List<Int32>();
                        totalCountDocs.Add(numOfOcc); //first is total num of occ across docs
                        totalCountDocs.Add(i); //word is present in currently traversed doc
                        globalOccInDocs.Add(wordInDoc, totalCountDocs);
                    }
                }
            }

            return globalOccInDocs;
        }

        /// <summary>
        /// Transforms files with test data to form of List which contains Article objects. Each of them represents one test doc and its content.
        /// </summary>
        /// <returns>List<Article> which contains parsed test data, each item is represented by one Article object</returns>
        public static List<Article> transTestToArticles()
        {
            //load input test data
            List<String> inputFileTestDataCzechCont = SupportTools.retrieveInputFile(".\\downloaded_data\\czechData.json"); //file: czechData.json
            inputFileTestDataCzechCont[0] = inputFileTestDataCzechCont.ElementAt(0).Remove(0, 1); //first line contains extra [

            List<Article> testDataArticles = new List<Article>();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            for (int i = 0; i < inputFileTestDataCzechCont.Count; i++) //go through file with cont to index ; one line = one doc
            {
                string docLine = inputFileTestDataCzechCont.ElementAt(i); //one line in the doc to be indexed
                docLine = docLine.Remove(docLine.Length - 1); //on regular line remove just last char = ,
                /* retrieve relevant data from line converted to json obj - START */
                var oneDocContJson = JsonConvert.DeserializeObject<TestDocIndex>(docLine, settings);

                string articleUrl = oneDocContJson.title;
                string articleHtml = oneDocContJson.id;
                string articleTidyText = oneDocContJson.text;
                string articleAllText = "";
                DateTime articleDateIndex = convUnixTimeToDateTime(long.Parse(oneDocContJson.date));
                /* retrieve relevant data from line converted to json obj - END */

                /* convert json to Article obj suitable for searching - START */
                Article newArticle = new Article();
                newArticle.Url = articleUrl.Trim(); //title
                newArticle.Html = articleHtml; //id
                newArticle.TidyText = articleTidyText; //text
                newArticle.AllText = articleAllText; //nothing in test data!!!
                newArticle.DateIndex = articleDateIndex; //date
                newArticle.isTestData = true;
                /* convert json to Article obj suitable for searching - END */

                testDataArticles.Add(newArticle);
            }

            return testDataArticles;
        }

        public static List<TestDocQuery> transQueryToTestDocQuery()
        {
            List<TestDocQuery> queries = new List<TestDocQuery>();
            List<String> inputFileTestDataTopicCont = retrieveInputFile(".\\downloaded_data\\topicData.json"); //file: topicData.json, file with queries
            inputFileTestDataTopicCont[0] = inputFileTestDataTopicCont.ElementAt(0).Remove(0, 1); //first line contains extra [

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            for (int i = 0; i < inputFileTestDataTopicCont.Count; i++) //go through file with queries
            {
                string docLine = inputFileTestDataTopicCont.ElementAt(i); //one line with query
                docLine = docLine.Remove(docLine.Length - 1); //on regular line remove just last char = ,
                var oneDocContJson = JsonConvert.DeserializeObject<TestDocQuery>(docLine, settings); //retrieve relevant data from line converted to json obj

                queries.Add(oneDocContJson);
            }
            return queries;
        }

        /// <summary>
        /// Converts unix time present in demo docs to DateTime object.
        /// </summary>
        /// <param name="unixtime">unix time, from demo doc</param>
        /// <returns></returns>
        private static DateTime convUnixTimeToDateTime(long unixtime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dateTime;
        }

        /// <summary>
        /// Transforms files with crawled data to form of List which contains Article objects. Each of them represents one article and its content.
        /// </summary>
        /// <returns>List<Article> which contains parsed articles, each represented by one Article object</returns>
        public static List<Article> transCrawlToArticles()
        {
            //load input from crawler; just documents to index without any queries - START
            List<String> inputFilearticlesdetailAllText = SupportTools.retrieveInputFile(".\\downloaded_data\\articlesdetail_allText.txt"); //file: articlesdetail_allText.txt ; crawled article text without respect to linebreaks
            List<String> inputFilearticlesdetailTidyText = SupportTools.retrieveInputFile(".\\downloaded_data\\articlesdetail_tidyText.txt"); //file: articlesdetail_tidyText.txt ; crawled article text with respect to linebreaks
            List<String> inputFilearticlesdetailHtml = SupportTools.retrieveInputFile(".\\downloaded_data\\articlesdetail_html.txt"); //file: articlesdetail_html.txt ; all article data retrieved from crawler, including html
            //load input from crawler; just documents to index without any queries - END

            //transform crawled article data to form of Article objects - START
            List<Article> crawledArticlesAllText = new List<Article>(); //List with Article objects

            int contentCounter = 0;
            int beginIndexTidyText = -1; //contains index from which begins content of recently traversed article, tidyText form (in articlesdetail_tidyText.txt)
            int endIndexTidyText = -1; //index where currently traversed article ends, tidyText form (file articlesdetail_tidyText.txt)
            int beginIndexHtml = -1; //contains index from which begins content of recently traversed article, html form (in articlesdetail_html.txt)
            int endIndexHtml = -1; //index where currently traversed article ends, html form (file articlesdetail_html.txt)

            Article newArticle = new Article();
            for (int i = 0; i < inputFilearticlesdetailAllText.Count; i++) //go through all retrieved lines ; i = link to article, (i + 1) = article content, (i + 2) = link to article
            {
                contentCounter++;

                if (contentCounter == 1) //going through first line - link, start of article
                {
                    String startLink = inputFilearticlesdetailAllText.ElementAt(i);

                    beginIndexTidyText = inputFilearticlesdetailTidyText.IndexOf(startLink); //find beginning in articlesdetail_tidyText.txt
                    beginIndexHtml = inputFilearticlesdetailHtml.IndexOf(startLink); //find beginning in articlesdetail_html.txt
                }
                else if (contentCounter == 2) //going through second line - article content
                {
                    String articleContentAllText = inputFilearticlesdetailAllText.ElementAt(i); //content of the one particular article, without respect to linebreaks
                    newArticle.AllText = articleContentAllText; //assign content to the traversed article
                }
                else if (contentCounter == 3) //going through third line - link, end of article
                {
                    String endLink = inputFilearticlesdetailAllText.ElementAt(i);
                    endIndexTidyText = inputFilearticlesdetailTidyText.IndexOf(endLink); //find article end position in articlesdetail_tidyText.txt
                    endIndexHtml = inputFilearticlesdetailHtml.IndexOf(endLink); //find article end index in articlesdetail_html.txt

                    contentCounter = 0; //reset counter, new article ahead
                    String articleLink = endLink.Replace("END ", ""); //replace beginning of the url
                    newArticle.Url = articleLink; //assign url to the traversed article

                    //tidyText content relevant to article is located within beginIndexTidyText and endIndexTidyText (excluded these two indexes)
                    List<String> tidyTextCont = inputFilearticlesdetailTidyText.GetRange(beginIndexTidyText + 1, endIndexTidyText - beginIndexTidyText - 2); //+1 = dont include START tag ; -2 dont include empty line located on end of article
                    var tidyTextContArr = tidyTextCont.Cast<String>().ToArray();
                    var tidyTextContString = string.Join(Environment.NewLine, tidyTextContArr);
                    newArticle.TidyText = tidyTextContString;

                    //html content relevant to article is located within beginIndexHtml and endIndexHtml (excluded these two indexes)
                    List<String> htmlCont = inputFilearticlesdetailHtml.GetRange(beginIndexHtml + 1, endIndexHtml - beginIndexHtml - 1); //+1 = dont include START tag ; -2 dont include empty line located on end of article
                    var htmlContArr = htmlCont.Cast<String>().ToArray();
                    var htmlContString = string.Join(Environment.NewLine, htmlContArr);
                    newArticle.Html = htmlContString;

                    newArticle.DateIndex = DateTime.Now; //actual time - time when indexed
                    crawledArticlesAllText.Add(newArticle); //add article to the list of available articles
                    newArticle = new Article();
                }
            }
            //transform crawled article data to form of Article objects - END
            return crawledArticlesAllText;
        }

        /// <summary>
        /// Loads stopwords from file stop_words_czech.txt.
        /// </summary>
        /// <returns>stopwords to remove</returns>
        public static List<String> loadStopWordsFromFile()
        {
            return retrieveInputFile("stop_words_czech.txt");
        }

        /// <summary>
        /// Builds expression tree for given string. Used for boolean search.
        /// </summary>
        /// <param name="phrase">search phrase entered by the user</param>
        /// <param name="indexManager">instance of IndexManager - for preprocessing</param>
        /// <returns>root node of the tree</returns>
        public static TreeNode createExprTree(String phrase, IndexManager indexManager)
        {
            if (traversedIndexTree >= phrase.Length) //end of string
            {
                return null;
            }

            //read the word - START
            String word = "";
            while (traversedIndexTree < phrase.Length) //end of phrase not encountered
            {
                if (phrase[traversedIndexTree] == '(' || phrase[traversedIndexTree] == ')') //end of the word, brackets
                {
                    break;
                }
                word += phrase[traversedIndexTree];
                traversedIndexTree++;
            }
            //read the word - END

            String preprocessedQuery = "";

            TreeNode node = new TreeNode(); //root will be created as first
            string[] splittedCont = Regex.Split(word, "(AND)|(OR)|(NOT)"); //split by keywords
            foreach(String oneWord in splittedCont)
            {
                if (!oneWord.Equals("AND") && !oneWord.Equals("OR") && !oneWord.Equals("NOT"))
                {
                    preprocessedQuery += indexManager.applyPreprocessing(oneWord);
                }
                else
                { //do not preprocess keywords
                    preprocessedQuery += oneWord;
                }
            }

            node.nodeValue = preprocessedQuery;

            if (traversedIndexTree >= phrase.Length)
            {
                return node;
            }

            //encountered open brack -> some expr on other side, recursive call left side
            if (traversedIndexTree < phrase.Length && phrase[traversedIndexTree] == '(')
            {
                traversedIndexTree++;
                node.leftNode = createExprTree(phrase, indexManager);
            }

            //encountered closing brack -> some expr ended, left side
            if (traversedIndexTree < phrase.Length && phrase[traversedIndexTree] == ')')
            {
                traversedIndexTree++;
                return node;
            }

            //recursive calls for right side
            if (traversedIndexTree < phrase.Length && phrase[traversedIndexTree] == '(')
            {
                traversedIndexTree++;
                node.rightNode = createExprTree(phrase, indexManager);
            }

            //encountered closing brack -> some expr ended, right side
            if (traversedIndexTree < phrase.Length && phrase[traversedIndexTree] == ')')
            {
                traversedIndexTree++;
                return node;
            }

            return node;
        }

        public static int num;
        public static void showTreeContDebug(TreeNode node)
        {
            num++;
            if (node == null)
                return;

            showTreeContDebug(node.leftNode);
            showTreeContDebug(node.rightNode);
        }
    }
}
