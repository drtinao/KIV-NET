using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace DrSearch
{
    /// <summary>
    /// Class contains method needed for crawling the web content.
    /// </summary>
    class CrawlerCore
    {
        private readonly string DATA_STORAGE = ".\\downloaded_data\\"; //where to store retrieved webpage data
        private readonly string SITE_ROOT = "https://diit.cz/"; //root page of desired webpage
        private readonly string SITE_SUFFIX = "/?utm_source=&page="; //suffix where to search articles - add number for specific page, starting from 0

        private TfIdfCore tfIdfCalc; //component responsible for tf-idf calculations
        private IndexManager indexManager; //used for managing indexed documents (loading + saving)
       
        public Thread crawlThread { get; set; } //thread used for crawling
        public Thread benchThread { get; set; } //thread used for running benchmark
        private DashboardForm dashboardForm; //form with the dashboard

        /// <summary>
        /// Constructor takes reference to dashboard form, which cointains GUI components for interaction with the application core.
        /// </summary>
        /// <param name="indexManager">used for managing indexed documents (loading + saving)</param>
        /// <param name="tfIdfCalc">component used for tf-idf calculations</param>
        /// <param name="dashboardFormRef">form with the dashboard</param>
        public CrawlerCore(IndexManager indexManager, TfIdfCore tfIdfCalc, DashboardForm dashboardFormRef)
        {
            this.indexManager = indexManager;
            this.tfIdfCalc = tfIdfCalc;
            this.dashboardForm = dashboardFormRef;

            this.crawlThread = new Thread(new ThreadStart(parseWholeWebsite)); //init crawling thread, dnt start yet!
            this.benchThread = new Thread(new ThreadStart(runBench)); //init bench thread, do not start yet
        }

        /// <summary>
        /// Used for posting progress information to information console located in dashboard.
        /// </summary>
        /// <param name="textToPost">text, which should be appended to information console</param>
        private void postProgressInfo(String textToPost)
        {
            dashboardForm.appendToProgInfoTBBuffer(textToPost);
        }

        /// <summary>
        /// Starts parsing of DIIT website. Calls respective methods which lead to creation of text files with perex and article details.
        /// </summary>
        public void parseWholeWebsite()
        {
            HtmlWeb htmlWeb = new HtmlWeb(); //connects to server, retrieves target website ; from HtmlAgilityPack
            htmlWeb.OverrideEncoding = Encoding.UTF8; //change encoding ; avoid diacritics problems

            int pageCounter = 0; //keeps track of number of visited pages

            deletePreviousCrawl(); //delete previously crawled data (if any - articles + perex)

            while (true) //go through webpages until empty one is found ; then break
            {
                Page onePage = new Page();
                HtmlNodeCollection pageUnparsedCont = getPageUnparsedCont(pageCounter, htmlWeb, onePage); //unparsed articles list which are present on the webpage
                if (pageUnparsedCont == null) //reached last page
                {
                    break;
                }

                parsePageCont(pageUnparsedCont, onePage); //Page object which represents one webpage, including articles (represented by Article object)

                List<Article> pageArticles = onePage.GetArticles(); //get list of articles present on the page
                if (pageArticles.Count != 0) //articles on webpage found, not empty => save page, parse articles
                {
                    postProgressInfo("Parsing page num: " + pageCounter + " - START");
                    savePage(onePage); //save perex from webpage to respective textfile
                    //go through articles on page and parse them - START

                    for (int i = 0; i < pageArticles.Count; i++) //go through retrieved articles and download data for each article
                    {
                        Article article = pageArticles[i]; //get one article from Page object
                        postProgressInfo("Parsing article url: " + article.Url);
                        HtmlNodeCollection articleContUnparsed = getArticleUnparsedCont(article.Url, htmlWeb); //get dom of article
                        if (articleContUnparsed == null) //article is not in standard format ; cannot parse
                        {
                            continue;
                        }
                        parseArticleCont(articleContUnparsed, article); //parse article content and assign to Article object
                        saveArticle(article); //save details from article to respective textfiles
                    }
                    //go through articles on page and parse them - END

                    postProgressInfo("Parsing page num: " + pageCounter + " - END");
                    pageCounter++;
                }
                else //current webpage is empty, exit
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Takes care of deleting files containing previously crawled data.
        /// </summary>
        private void deletePreviousCrawl()
        {
            string saveLocHtmlArticles = DATA_STORAGE + "articlesdetail_html.txt";
            string saveLocTidyArticles = DATA_STORAGE + "articlesdetail_tidyText.txt";
            string saveLocAllArticles = DATA_STORAGE + "articlesdetail_allText.txt";

            string saveLocHtmlPerex = DATA_STORAGE + "perex_html.txt";
            string saveLocTidyPerex = DATA_STORAGE + "perex_tidyText.txt";
            string saveLocAllPerex = DATA_STORAGE + "perex_allText.txt";

            if(File.Exists(saveLocHtmlArticles))
                File.Delete(saveLocHtmlArticles);
            if (File.Exists(saveLocTidyArticles))
                File.Delete(saveLocTidyArticles);
            if(File.Exists(saveLocAllArticles))
                File.Delete(saveLocAllArticles);
            if(File.Exists(saveLocHtmlPerex))
                File.Delete(saveLocHtmlPerex);
            if(File.Exists(saveLocTidyPerex))
                File.Delete(saveLocTidyPerex);
            if(File.Exists(saveLocAllPerex))
                File.Delete(saveLocAllPerex);
        }

        /// <summary>
        /// Downloads webpage with articles from target website and returns article section in unparsed form.
        /// </summary>
        /// <param name="pageNum">number of page which should be retrieved (lowest possible is 0)</param>
        /// <param name="htmlWeb">instance of HtmlWeb provided by HtmlAgilityPack ; connects to web server and retrieves target website</param>
        /// <param name="pageToAssign">Page object which whould be assigned url</param>
        /// <returns>HtmlNodeCollection - contains article section</returns>
        private HtmlNodeCollection getPageUnparsedCont(int pageNum, HtmlWeb htmlWeb, Page pageToAssign)
        {
            HtmlNodeCollection[] htmlNodes = new HtmlNodeCollection[2];
            HtmlAgilityPack.HtmlDocument domPage = htmlWeb.Load(SITE_ROOT + SITE_SUFFIX + pageNum);
            pageToAssign.Url = SITE_ROOT + SITE_SUFFIX + pageNum;

            HtmlNodeCollection articlesOnPage = domPage.DocumentNode.SelectNodes("//div[@class='page_center']//div[@class='view-content']//div[contains(@class, 'node') and contains(@class, 'node-article') and contains(@class, 'node-teaser') and contains(@class, 'clearfix') and contains(@class, 'hoverable') and contains(@class, 'teaser')]"); //get articles content

            return articlesOnPage;
        }

        /// <summary>
        /// Parses content of one page with articles.
        /// </summary>
        /// <param name="articlesUnparsedCont">HtmlNodeCollection - contains section with articles in unparsed form</param>
        /// <param name="pageToModify">Page object to which data should be assigned</param>
        private void parsePageCont(HtmlNodeCollection articlesUnparsedCont, Page pageToModify)
        {
            string html = ""; //content of the page in html
            string tidyText = ""; //plaintext of the page with respect to linebreaks
            string allText = ""; //plaintext of the page with removed linebreaks

            //go through odd and even articles code - START
            int articleCounter = 0; //number of parsed articles

            for (articleCounter = 0; articleCounter < articlesUnparsedCont.Count; articleCounter++)
            {
                html += articlesUnparsedCont[articleCounter].OuterHtml; //append html code of one particular article to the html of whole page

                string tidyTextEmptyRemoved = Regex.Replace(articlesUnparsedCont[articleCounter].InnerText, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline); //"tidy text" without empty lines
                string tidyTextSpacesRemoved = string.Join(Environment.NewLine, tidyTextEmptyRemoved.Split(new char[] { '\n', '\r' }, StringSplitOptions.None).Select(s => s.Trim())); //"tidy text" without spaces 

                tidyText += tidyTextSpacesRemoved; //append "tidy text" of one particular ODD article to the tidy text of whole page
                allText += tidyText.Replace("\n", "").Replace("\r", ""); //remove linebreaks from tidytext and append to the text with removed linebreaks of whole page

                Article article = new Article();
                article.Url = articlesUnparsedCont[articleCounter].SelectNodes("//h2//a//@href")[articleCounter].Attributes["href"].Value;
                pageToModify.addArticle(article);
            }
            //go through odd and even articles code - END

            pageToModify.Html = html; //assign retrieved code to Page object (represents one page on website)
            pageToModify.TidyText = tidyText;
            pageToModify.AllText = allText;
        }

        /// <summary>
        /// Downloads html of the article and returns article content (unparsed form).
        /// </summary>
        /// <param name="urlToDownload">address of article</param>
        /// <param name="htmlWeb">instance of HtmlWeb provided by HtmlAgilityPack ; connects to web server and retrieves target website</param>
        /// <returns></returns>
        private HtmlNodeCollection getArticleUnparsedCont(string urlToDownload, HtmlWeb htmlWeb)
        {
            HtmlAgilityPack.HtmlDocument domPageArticle;

            try
            {
                if (urlToDownload.StartsWith("https://cdr.cz/"))
                {
                    domPageArticle = htmlWeb.Load(urlToDownload);
                }
                else
                {
                    domPageArticle = htmlWeb.Load(SITE_ROOT + urlToDownload);
                }
            }catch(Exception e) //thread can be aborted anytime by user.. 
            {
                return null;
            }
            HtmlNodeCollection articleContent = domPageArticle.DocumentNode.SelectNodes("//div[@class='content']//div[@class='content-container mb bold'] | //div[@class='content']//div[contains(@class, 'field') and contains(@class, 'field-name-body') and contains(@class, 'field-type-text-with-summary') and contains(@class, ' field-label-hidden')]");

            return articleContent;
        }

        /// <summary>
        /// Parses content of one particular article.
        /// </summary>
        /// <param name="articleToParse">HtmlNodeCollection which represents one article (in detail ; opened)</param>
        /// <param name="articleToModify">Article object to which data should be assigned</param>
        private void parseArticleCont(HtmlNodeCollection articleToParse, Article articleToModify)
        {
            string html = ""; //content of the article in html
            string tidyText = ""; //plaintext of the article with respect to linebreaks
            string allText = ""; //plaintext of the article with removed linebreaks

            for (int i = 0; i < articleToParse.Count; i++)
            {
                html += articleToParse[i].OuterHtml; //append html code to the html of whole page

                string tidyTextEmptyRemoved = Regex.Replace(articleToParse[i].InnerText, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline); //"tidy text" without empty lines
                string tidyTextSpacesRemoved = string.Join(Environment.NewLine, tidyTextEmptyRemoved.Split(new char[] { '\n', '\r' }, StringSplitOptions.None).Select(s => s.Trim())); //"tidy text" without spaces 

                tidyText += tidyTextSpacesRemoved; //append "tidy text" of one particular ODD article to the tidy text of whole page
                allText += tidyText.Replace("\n", "").Replace("\r", ""); //remove linebreaks from tidytext and append to the text with removed linebreaks of whole page
            }

            articleToModify.Html = html; //assign retrieved code to Article object (represents one article on website)
            articleToModify.TidyText = tidyText;
            articleToModify.AllText = allText;
        }

        /// <summary>
        /// Appends data given by Article object to respective text files. Creates data folder if not present yet
        /// </summary>
        /// <param name="articleToSave">Article object which represents one article (in detail ; opened)</param>
        private void saveArticle(Article articleToSave)
        {
            string saveLocHtml = DATA_STORAGE + "articlesdetail_html.txt";
            string saveLocTidy = DATA_STORAGE + "articlesdetail_tidyText.txt";
            string saveLocAll = DATA_STORAGE + "articlesdetail_allText.txt";

            Directory.CreateDirectory(DATA_STORAGE); //create data directory, if not present yet

            using (StreamWriter sw = File.AppendText(saveLocHtml))
            {
                sw.WriteLine("START " + articleToSave.Url + Environment.NewLine + articleToSave.Html + Environment.NewLine + "END " + articleToSave.Url);
            }

            using (StreamWriter sw = File.AppendText(saveLocTidy))
            {
                sw.WriteLine("START " + articleToSave.Url + Environment.NewLine + articleToSave.TidyText + Environment.NewLine + "END " + articleToSave.Url);
            }

            using (StreamWriter sw = File.AppendText(saveLocAll))
            {
                sw.WriteLine("START " + articleToSave.Url + Environment.NewLine + articleToSave.AllText + Environment.NewLine + "END " + articleToSave.Url);
            }
        }

        /// <summary>
        /// Appends data given by Page object to respective text files. Creates data folder if not present yet
        /// </summary>
        /// <param name="pageToSave">Page object which represents one particular page of website</param>
        private void savePage(Page pageToSave)
        {
            string saveLocHtml = DATA_STORAGE + "perex_html.txt";
            string saveLocTidy = DATA_STORAGE + "perex_tidyText.txt";
            string saveLocAll = DATA_STORAGE + "perex_allText.txt";

            Directory.CreateDirectory(DATA_STORAGE); //create data directory, if not present yet

            using (StreamWriter sw = File.AppendText(saveLocHtml))
            {
                sw.WriteLine("START " + pageToSave.Url + Environment.NewLine + pageToSave.Html + Environment.NewLine + "END " + pageToSave.Url);
            }

            using (StreamWriter sw = File.AppendText(saveLocTidy))
            {
                sw.WriteLine("START " + pageToSave.Url + Environment.NewLine + pageToSave.TidyText + Environment.NewLine + "END " + pageToSave.Url);
            }

            using (StreamWriter sw = File.AppendText(saveLocAll))
            {
                sw.WriteLine("START " + pageToSave.Url + Environment.NewLine + pageToSave.AllText + Environment.NewLine + "END " + pageToSave.Url);
            }
        }

        /// <summary>
        /// Checks whether data were already crawled or not. That means: file with crawled data exists - if data found, return DateTime object with information regarding to file creation; else DateTime.MinValue.
        /// </summary>
        /// <returns>DateTime - crawled data creation time</returns>
        public static DateTime retrieveDataCrawlTime()
        {
            String pathToCrawledFileAll = ".\\downloaded_data\\articlesdetail_allText.txt"; //path to file with crawled data
            String pathToCrawledFileHtml = ".\\downloaded_data\\articlesdetail_html.txt";
            String pathToCrawledFileTidyText = ".\\downloaded_data\\articlesdetail_tidyText.txt";
            String pathToIndex = ".\\index.drsearch";

            DateTime creationCrawl = DateTime.MinValue;

            if (File.Exists(pathToCrawledFileAll) && File.Exists(pathToCrawledFileHtml) && File.Exists(pathToCrawledFileTidyText) && File.Exists(pathToIndex)) //if files with crawled data exist + index exists
            {
                creationCrawl = new FileInfo(pathToCrawledFileAll).LastWriteTime;
            }

            return creationCrawl;
        }

        /// <summary>
        /// Executed when button for searching is clicked. If data crawled + indexed, search is performed. Else alert is shown.
        /// </summary>
        /// <param name="phraseToSearch">phrase enetered by user into search field</param>
        /// <param name="vectorModeSearch">true if vector search should be performed (false for boolean mode search)</param>
        public void handleSearchBtnClick(String phraseToSearch, bool vectorModeSearch)
        {
            //check if crawling / indexing not in progress -> if so, disp alert
            if (crawlThread.IsAlive || indexManager.indexThread.IsAlive) //crawling already in progress, err
            {
                MessageBox.Show("Crawling / indexing in progress!" + Environment.NewLine + "Please wait.", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //no operation in progress -> ok, check what type of index is loaded
            if(indexManager.loadedIndexType == -1) //no index loaded, error
            {
                MessageBox.Show("No index loaded." + Environment.NewLine + "Please load crawl / test index before you perform search!", "No index", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //crawling not yet in progress -> go on, check if crawled data exist etc
            if(phraseToSearch.Length == 0) //crawl + index ok, check if user input is ok
            {
                MessageBox.Show("Please enter phrase for which you want to find relevant documents!", "No search phrase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //everything looks ok, perform search on loaded index
            if (vectorModeSearch)
            {//vector search
                performVectorSearch(phraseToSearch, true);
            }
            else
            {
                if(performBoolSearch(phraseToSearch) == null)
                {
                    MessageBox.Show("Error in query! Try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Runs benchmark on test data given by teacher (queries are in file topicData.json).
        /// </summary>
        public void runBench()
        {
            dashboardForm.appendToProgInfoTBBuffer("***BENCHMARK - START***");
            String benchFilename = "res_DRTINA.txt";

            if (File.Exists(benchFilename))
            {
                File.Delete(benchFilename);
            }
            
            List<TestDocQuery> testQueries = SupportTools.transQueryToTestDocQuery();
            foreach(TestDocQuery query in testQueries)
            {
                List<CosSimMatch> queryResults = performVectorSearch(query.title + " " + query.narrative + " " + query.description, false);
                if(queryResults.Count == 0)
                { //no match found
                    using (StreamWriter sw = File.AppendText(benchFilename))
                    {
                        sw.Write(query.id + " Q0 abc 99 0.0 runindex1" + Environment.NewLine);
                    }
                }
                else
                { //print matches, ordered
                     int i = 1;
                     foreach(CosSimMatch queryMatch in queryResults)
                     {
                        using (StreamWriter sw = File.AppendText(benchFilename))
                        {
                            sw.Write(query.id + " Q0 " + queryMatch.document.Html + " " + i + " " + queryMatch.cosSimScore.ToString(CultureInfo.InvariantCulture) + " runindex1" + Environment.NewLine);
                        }
                        i++;
                     }
                }
            }
            dashboardForm.appendToProgInfoTBBuffer("Results written to file: " + benchFilename);
            dashboardForm.appendToProgInfoTBBuffer("***BENCHMARK - END***");
        }

        /// <summary>
        /// Performs search based on given boolean query.
        /// </summary>
        /// <param name="phraseToSearch">query entered by user</param>
        /// <returns>list with matches</returns>
        private List<CosSimMatch> performBoolSearch(String phraseToSearch)
        {
            //firstly check if count of ( and ) is equal - START
            int numOccLeftBrack = phraseToSearch.Where(x => (x == '(')).Count();
            int numOccRighBrack = phraseToSearch.Where(x => (x == ')')).Count();

            if(numOccLeftBrack != numOccRighBrack) //number of brackets not matching
            {
                MessageBox.Show("Check your query." + Environment.NewLine + "Count of left ( and right brackets ) does not match!", "Wrong query", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            //firstly check if count of ( and ) is equal - END

            //brackets ok - go on, create tree with expressions - TreeNode represents one node in the tree
            SupportTools.traversedIndexTree = 0; //init index (recursion)
            TreeNode rootNode = SupportTools.createExprTree(phraseToSearch, indexManager);
            CrawlerCore.num = 0;
            assignRelDocsToNodes(rootNode);

            //load data from indexer - START
            Dictionary<String, List<Int32>> validDocumentsNumsLoaded = indexManager.validDocumentsNumsLoaded;
            List<Dictionary<String, Int32>> individualDocsCountsLoaded = indexManager.individualDocsCountsLoaded;
            List<Double> tfIdfDocsLoaded = indexManager.tfIdfDocsLoaded;

            List<Article> indexedDocs = indexManager.articlesLoaded;
            //load data from indexer - END

            List<CosSimMatch> matchingDocs = new List<CosSimMatch>();
            List<Int32> matchingArticlesIndex;
            if (rootNode.matchingArticles == null)
            {
                return null;
            }
            else
            {
                matchingArticlesIndex = rootNode.matchingArticles;
            }
            if (matchingArticlesIndex != null)
            {
                foreach (Int32 docIndex in matchingArticlesIndex)
                {
                    CosSimMatch docMatch = new CosSimMatch();
                    docMatch.cosSimScore = 0;
                    docMatch.document = indexedDocs.ElementAt(docIndex);
                    docMatch.query = String.Copy(phraseToSearch);
                    matchingDocs.Add(docMatch);
                }
            }

            int limit = 300; //limit to top 300
            foreach (CosSimMatch match in matchingDocs)
            {
                if (limit > 0)
                {
                    dashboardForm.addToRelDocsLBBuffer(match);
                    limit--;
                }
                else
                {
                    break;
                }
            }

            if (matchingDocs.Count == 0)
            { //no matches found :/!
                CosSimMatch emptyMatch = new CosSimMatch();
                emptyMatch.isEmpty = true;
                dashboardForm.addToRelDocsLBBuffer(emptyMatch);
            }

            return matchingDocs;
        }

        public static int num;
        /// <summary>
        /// Find relevant documents for each node in tree.
        /// </summary>
        /// <param name="node">represents one node in binary tree</param>
        public void assignRelDocsToNodes(TreeNode node)
        {
            if (node.leftNode != null)
            {
                assignRelDocsToNodes(node.leftNode);
            }

            //load data from indexer - START
            Dictionary<String, List<Int32>> validDocumentsNumsLoaded = indexManager.validDocumentsNumsLoaded;
            List<Dictionary<String, Int32>> individualDocsCountsLoaded = indexManager.individualDocsCountsLoaded;

            List<Article> indexedDocs = indexManager.articlesLoaded;
            //load data from indexer - END

            Dictionary<String, List<Int32>> tempDocs = new Dictionary<String, List<Int32>>(); //used for storing temp docs in the query - when suitable docs is found -> DRSEARCHX key

            string nodeText = String.Copy(node.nodeValue); //text belonging to the node
            string[] splittedCont = Regex.Split(node.nodeValue, "(AND)|(OR)|(NOT)"); //split by keywords

            if(splittedCont.Length == 1)
            {
                String wordWhichShouldBePresent = splittedCont[0];
                List<Int32> validDocs = new List<Int32>();

                List<String> matchingPrefixes = indexManager.validDocumentsNumsLoaded.Keys.Where(x => x.StartsWith(wordWhichShouldBePresent)).ToList();
                if (matchingPrefixes.Count != 0)
                { //retrieve valid docs which contain given word
                    foreach(String matchingPrefWord in matchingPrefixes)
                    {
                        for (int j = 1; j < validDocumentsNumsLoaded[matchingPrefWord].Count; j++)
                        { //documents WHICH should be included
                            if(!validDocs.Contains(validDocumentsNumsLoaded[matchingPrefWord][j]))
                            validDocs.Add(validDocumentsNumsLoaded[matchingPrefWord][j]);
                        }
                    }
                }
                else
                { //word is not present in any doc
                    validDocs = new List<Int32>();
                }
                
                nodeText = nodeText.Replace(wordWhichShouldBePresent, "DRSEACH" + tempDocs.Count); //add suitable docs to temp
                tempDocs.Add("DRSEACH" + tempDocs.Count, validDocs);
            }

            //firstly take words which have NOT in front of them and find suitable docs - START
            for(int i = 0; i < splittedCont.Length; i++)
            {
                String wordOnPos = splittedCont[i];
                if (wordOnPos.Equals("NOT")) //if not keyword found, find rel docs for next word
                {
                    String wordWhichShouldntBePresent = splittedCont[i + 1];
                    if (wordWhichShouldntBePresent.Length == 0) //no word after NOT, prolly something in (), check children
                    {
                        //children should be solved and contain valid docs num, INVERSE it
                        List<Int32> suitableNotDocs = Enumerable.Range(0, individualDocsCountsLoaded.Count).ToList(); //at first contains numbers of all available docs
                        List<Int32> documentsLeftNode = node.leftNode.matchingArticles; //docs which were suitable for left node - inverse
                        foreach(Int32 indexToRemove in documentsLeftNode)
                        {
                            suitableNotDocs.RemoveAll(r => r == indexToRemove);
                        }

                        nodeText = nodeText.Replace("NOT", "DRSEACH" + tempDocs.Count); //add suitable docs to temp
                        tempDocs.Add("DRSEACH" + tempDocs.Count, suitableNotDocs);
                    }
                    else
                    {
                        List<Int32> docsWithGivenWord = new List<Int32>();
                        List<String> matchingPrefixes = indexManager.validDocumentsNumsLoaded.Keys.Where(x => x.StartsWith(wordWhichShouldntBePresent)).ToList();
                        if(matchingPrefixes.Count != 0)
                        {
                            foreach (String matchingPrefWord in matchingPrefixes)
                            {
                                for (int j = 1; j < validDocumentsNumsLoaded[matchingPrefWord].Count; j++)
                                { //documents WHICH should NOT be included
                                    if (!docsWithGivenWord.Contains(validDocumentsNumsLoaded[matchingPrefWord][j]))
                                        docsWithGivenWord.Add(validDocumentsNumsLoaded[matchingPrefWord][j]);
                                }
                            }
                        }

                        List<Int32> docsWithoutTheWord = Enumerable.Range(0, individualDocsCountsLoaded.Count).ToList(); //at first contains numbers of all available docs

                        for (int j = 0; j < docsWithGivenWord.Count; j++) //go through docs which contain the unwanted word
                        {
                            docsWithoutTheWord.RemoveAll(r => r == docsWithGivenWord.ElementAt(j));
                        }

                        nodeText = nodeText.Replace("NOT" + wordWhichShouldntBePresent, "DRSEACH" + tempDocs.Count); //add suitable docs to temp
                        tempDocs.Add("DRSEACH" + tempDocs.Count, docsWithoutTheWord);
                    }
                }
            }
            //firstly take words which have NOT in front of them and find suitable docs - END

            //secondly, take words between which is AND - START
            splittedCont = Regex.Split(nodeText, "(AND)|(OR)|(NOT)"); //split by keywords again
            for (int i = 0; i < splittedCont.Length; i++)
            {
                String wordOnPos = splittedCont[i];
                if (wordOnPos.Equals("AND")) //if and keyword found find rel docs for word + 1 and word - 1
                {
                    String wordWhichShouldBePresent1 = splittedCont[i - 1];
                    String wordWhichShouldBePresent2 = splittedCont[i + 1];

                    //check left side - START
                    List<Int32> validDocsLeftSide = new List<Int32>();
                    if (tempDocs.ContainsKey(wordWhichShouldBePresent1)) //in temp (prolly NOT)
                    {
                        validDocsLeftSide = tempDocs[wordWhichShouldBePresent1];
                    }
                    else
                    { //not in temp
                        List<String> matchingPrefixes = indexManager.validDocumentsNumsLoaded.Keys.Where(x => x.StartsWith(wordWhichShouldBePresent1)).ToList();
                        if (matchingPrefixes.Count != 0)
                        { //retrieve valid docs which contain given word
                            foreach (String matchingPrefWord in matchingPrefixes)
                            {
                                for (int j = 1; j < validDocumentsNumsLoaded[matchingPrefWord].Count; j++)
                                {
                                    if (!validDocsLeftSide.Contains(validDocumentsNumsLoaded[matchingPrefWord][j]))
                                        validDocsLeftSide.Add(validDocumentsNumsLoaded[matchingPrefWord][j]);
                                }
                            }
                        }
                        else
                        { //word is not present in any doc
                            validDocsLeftSide = new List<Int32>();
                        }
                    }
                    //check left side - END

                    //check right side - START
                    List<Int32> validDocsRightSide = new List<Int32>();
                    if (tempDocs.ContainsKey(wordWhichShouldBePresent2)) //in temp (prolly NOT)
                    {
                        validDocsRightSide = tempDocs[wordWhichShouldBePresent2];
                    }
                    else
                    { //not in temp
                        List<String> matchingPrefixes = indexManager.validDocumentsNumsLoaded.Keys.Where(x => x.StartsWith(wordWhichShouldBePresent2)).ToList();
                        if (matchingPrefixes.Count != 0)
                        { //retrieve valid docs which contain given word
                            foreach (String matchingPrefWord in matchingPrefixes)
                            {
                                for (int j = 1; j < validDocumentsNumsLoaded[matchingPrefWord].Count; j++)
                                {
                                    if (!validDocsRightSide.Contains(validDocumentsNumsLoaded[matchingPrefWord][j]))
                                        validDocsRightSide.Add(validDocumentsNumsLoaded[matchingPrefWord][j]);
                                }
                            }
                        }
                        else
                        { //word is not present in any doc
                            validDocsRightSide = new List<Int32>();
                        }
                    }
                    //check right side - END

                    //got results for left and right side, continue by finding numbers which are same
                    //find same numbers left + right side - START
                    List<Int32> validDocs = new List<Int32>();

                    for (int j = 0; j < validDocsLeftSide.Count; j++)
                    {
                        int validDocNumLeft = validDocsLeftSide.ElementAt(j);

                        if (validDocsRightSide.Contains(validDocNumLeft))
                        {
                            validDocs.Add(validDocNumLeft);
                        }
                    }
                    //find same numbers left + right side - END
                    nodeText = nodeText.Replace(wordWhichShouldBePresent1 + "AND" + wordWhichShouldBePresent2, "DRSEACH" + tempDocs.Count); //add suitable docs to temp
                    tempDocs.Add("DRSEACH" + tempDocs.Count, validDocs);
                }
            }
            //secondly, take words between which is AND - END

            //finally, take words with OR - START
            splittedCont = Regex.Split(nodeText, "(AND)|(OR)|(NOT)"); //split by keywords again

            for (int i = 0; i < splittedCont.Length; i++)
            {
                String wordOnPos = splittedCont[i];
                if (wordOnPos.Equals("OR")) //if or keyword found find rel docs for word + 1 and word - 1
                {
                    String wordWhichShouldBePresent1 = splittedCont[i - 1];
                    String wordWhichShouldBePresent2 = splittedCont[i + 1];

                    //check left side - START
                    List<Int32> validDocsLeftSide = new List<Int32>();
                    if (tempDocs.ContainsKey(wordWhichShouldBePresent1)) //in temp (prolly NOT)
                    {
                        validDocsLeftSide = tempDocs[wordWhichShouldBePresent1];
                    }
                    else
                    { //not in temp
                        List<String> matchingPrefixes = indexManager.validDocumentsNumsLoaded.Keys.Where(x => x.StartsWith(wordWhichShouldBePresent1)).ToList();
                        if (matchingPrefixes.Count != 0)
                        { //retrieve valid docs which contain given word
                            foreach (String matchingPrefWord in matchingPrefixes)
                            {
                                for (int j = 1; j < validDocumentsNumsLoaded[matchingPrefWord].Count; j++)
                                {
                                    if (!validDocsLeftSide.Contains(validDocumentsNumsLoaded[matchingPrefWord][j]))
                                        validDocsLeftSide.Add(validDocumentsNumsLoaded[matchingPrefWord][j]);
                                }
                            }
                        }
                        else
                        { //word is not present in any doc
                            validDocsLeftSide = new List<Int32>();
                        }
                    }
                    //check left side - END

                    //check right side - START
                    List<Int32> validDocsRightSide = new List<Int32>();
                    if (tempDocs.ContainsKey(wordWhichShouldBePresent2)) //in temp (prolly NOT)
                    {
                        validDocsRightSide = tempDocs[wordWhichShouldBePresent2];
                    }
                    else
                    { //not in temp
                        List<String> matchingPrefixes = indexManager.validDocumentsNumsLoaded.Keys.Where(x => x.StartsWith(wordWhichShouldBePresent2)).ToList();
                        if (matchingPrefixes.Count != 0)
                        { //retrieve valid docs which contain given word
                            foreach (String matchingPrefWord in matchingPrefixes)
                            {
                                for (int j = 1; j < validDocumentsNumsLoaded[matchingPrefWord].Count; j++)
                                {
                                    if (!validDocsRightSide.Contains(validDocumentsNumsLoaded[matchingPrefWord][j]))
                                        validDocsRightSide.Add(validDocumentsNumsLoaded[matchingPrefWord][j]);
                                }
                            }
                        }
                        else
                        { //word is not present in any doc
                            validDocsRightSide = new List<Int32>();
                        }
                    }
                    //check right side - END

                    //get numbers left + right side - START
                    List<Int32> validDocs = new List<Int32>();

                    for (int j = 0; j < validDocsLeftSide.Count; j++)
                    {
                        int validDocNumLeft = validDocsLeftSide.ElementAt(j);

                        if (!validDocs.Contains(validDocNumLeft))
                        {
                            validDocs.Add(validDocNumLeft);
                        }
                    }

                    for (int j = 0; j < validDocsRightSide.Count; j++)
                    {
                        int validDocNumRight = validDocsRightSide.ElementAt(j);

                        if (!validDocs.Contains(validDocNumRight))
                        {
                            validDocs.Add(validDocNumRight);
                        }
                    }
                    //get numbers left + right side - END
                    nodeText = nodeText.Replace(wordWhichShouldBePresent1 + "OR" + wordWhichShouldBePresent2, "DRSEACH" + tempDocs.Count); //add suitable docs to temp
                    tempDocs.Add("DRSEACH" + tempDocs.Count, validDocs);
                }
                //finally, take words with OR - END
           }
                //node text should now contain only DRSEARCHX which will contain all valid doc nums
            //node text should now only contain DRSEARCHX, which is key to tempDict - contains all valid docs for given node
            
            if (tempDocs.ContainsKey(nodeText))
            {
                node.matchingArticles = tempDocs[nodeText];
            }
            else
            {
                node.matchingArticles = null;
            }
            if (node.rightNode != null)
            {
                assignRelDocsToNodes(node.rightNode);
            }
        }

        /// <summary>
        /// Performs vector search based on given query.
        /// </summary>
        /// <param name="phraseToSearch">query entered by user</param>
        /// <param name="printResToRelDocs">true if result should be printed to GUI, else false</param>
        /// <returns>list obj with result ordered by cos sim</returns>
        private List<CosSimMatch> performVectorSearch(String phraseToSearch, bool printResToRelDocs)
        {
            String preprocessedText = indexManager.applyPreprocessing(phraseToSearch);

            //load data from indexer - START
            Dictionary<String, List<Int32>> validDocumentsNumsLoaded = indexManager.validDocumentsNumsLoaded;
            List<Dictionary<String, Int32>> individualDocsCountsLoaded = indexManager.individualDocsCountsLoaded;
            List<Double> tfIdfDocsLoaded = indexManager.tfIdfDocsLoaded;

            List<Article> indexedDocs = indexManager.articlesLoaded;
            //load data from indexer - END

            //create dict with weights for query - START
            List<String> wordsInQuery = Regex.Split(phraseToSearch.ToLower(), "\\s+").ToList(); //represents words, terms in one document

            List<String> queryList = new List<String>();
            queryList.Add(phraseToSearch.ToLower());
            List<Dictionary<String, Int32>> numOfOcc = SupportTools.retrieveOccuranceWordInIndivDocs(queryList);

            Dictionary<String, Double> weightQueryDict = new Dictionary<String, Double>(); //contains weights for each word in query
            foreach (string smallWord in wordsInQuery) //count weight for every word in query
            {
                if (weightQueryDict.ContainsKey(smallWord)) //no need to count twice
                    continue;

                double idf = tfIdfCalc.retrieveIdf(validDocumentsNumsLoaded, smallWord, individualDocsCountsLoaded.Count);
                double tfweight = 1 + Math.Log10(numOfOcc.ElementAt(0)[smallWord]); //tfweight  1 + log tfraw (count of occ in the one doc)
                double weight = idf * tfweight;

                weightQueryDict.Add(smallWord, weight);
            }
            //create dict with weights for query - END

            //count weight^2 for the query - START
            double weightQuerySquare = 0;
            List<Double> savedDocInfoValue = weightQueryDict.Values.ToList();

            foreach (double weight in savedDocInfoValue)
            {
                weightQuerySquare += weight * weight;
            }

            weightQuerySquare = Math.Sqrt(weightQuerySquare);
            //count weight^2 for the query - END

            //create dict with weights for indexed documents
            List<String> wordPresentInQuery = weightQueryDict.Keys.ToList();
            Dictionary<Int32, Dictionary<String, Double>> weightDocDicts = new Dictionary<Int32, Dictionary<String, Double>>(); //outer - key: index of indexed doc, value: dict with info regarding to doc; inner - key: word, value: weight

            for (int i = 0; i < wordPresentInQuery.Count; i++)
            { //go through words present in query
                String word = wordPresentInQuery[i];
                List<Int32> docsInWhichWord;

                if (validDocumentsNumsLoaded.ContainsKey(word))
                {
                    docsInWhichWord = new List<Int32>();
                    foreach (Int32 wordToCopy in validDocumentsNumsLoaded[word]) //deep copy...
                    {
                        docsInWhichWord.Add(wordToCopy);
                    }
                    docsInWhichWord.RemoveAt(0); //first index contains total count
                }
                else
                {
                    docsInWhichWord = new List<Int32>();
                }

                if (docsInWhichWord.Count > 0)
                { //word present in atleast one indexed doc
                    for (int j = 0; j < docsInWhichWord.Count; j++) //go through documents and retrieve weight
                    {
                        int docIndex = docsInWhichWord[j]; //index of doc containing the word
                        Dictionary<String, Int32> oneDocAppearCount = individualDocsCountsLoaded.ElementAt(docIndex);
                        double idf = weightQueryDict[word]; //idf is same as with query
                        double tfweight = 1 + Math.Log10(oneDocAppearCount[word]); //tfweight  1 + log tfraw (count of occ in the one doc)
                        double weight = idf * tfweight;

                        if (!weightDocDicts.ContainsKey(docIndex))
                        { //outer dict for specific doc not yet present
                            Dictionary<String, Double> innerDocDict = new Dictionary<String, Double>(); //create dict for newly encountered doc
                            innerDocDict.Add(word, weight);
                            weightDocDicts.Add(docIndex, innerDocDict);
                        }
                        else
                        { //outer contains the specific doc, add to inner
                            Dictionary<String, Double> innerDocDict = weightDocDicts[docIndex];
                            innerDocDict.Add(word, weight);
                            weightDocDicts[docIndex] = innerDocDict;
                        }
                    }
                }
            }

            //now go through docs present in weightDocDicts and calculate cos sim for each of them - START
            List<CosSimMatch> matches = new List<CosSimMatch>();

            List<Int32> indexRelDocs = weightDocDicts.Keys.ToList(); //get indexes of relevant docs
            foreach (Int32 docIndex in indexRelDocs)
            {
                CosSimMatch docMatch = new CosSimMatch();
                Dictionary<String, Double> weightForDoc = weightDocDicts[docIndex]; //weights for one doc

                double docScore = 0;
                foreach (String word in wordPresentInQuery)
                { //go through words in query and retrieve score for query + for the given doc
                    double weightWordQuery = weightQueryDict[word];
                    double weightWordDoc;
                    if (weightForDoc.ContainsKey(word))
                    {
                        weightWordDoc = weightForDoc[word];
                    }
                    else
                    {
                        weightWordDoc = 0;
                    }

                    docScore += weightWordQuery * weightWordDoc;
                }

                docScore /= weightQuerySquare * tfIdfDocsLoaded.ElementAt(docIndex);

                docMatch.cosSimScore = docScore;
                docMatch.document = indexedDocs.ElementAt(docIndex);
                docMatch.query = String.Copy(phraseToSearch);
                matches.Add(docMatch);
            }

            matches = matches.OrderByDescending(o => o.cosSimScore).ToList(); //order by achieved cos sim score

            if (printResToRelDocs)
            {
                int limit = 300; //limit to top 300
                foreach (CosSimMatch match in matches)
                {
                    if (limit > 0)
                    {
                        dashboardForm.addToRelDocsLBBuffer(match);
                        limit--;
                    }
                    else
                    {
                        break;
                    }
                }

                if (matches.Count == 0)
                { //no matches found :/!
                    CosSimMatch emptyMatch = new CosSimMatch();
                    emptyMatch.isEmpty = true;
                    dashboardForm.addToRelDocsLBBuffer(emptyMatch);
                }
                return null;
            }
            else
            {
                return matches;
            }
            //now go through docs present in weightDocDicts and calculate cos sim for each of them - END
        }

        /// <summary>
        /// Is executed when button for crawling / indexing data is clicked. Checks whether crawling is in progress - no => start crawling, yes => cancel crawling.
        /// </summary>
        /// <returns>true if crawling process trigerred, else false (if already running)</returns>
        public bool handleCrawlDataBtnClick()
        {
            if (!crawlThread.IsAlive && crawlThread.ThreadState != ThreadState.AbortRequested && !indexManager.indexThread.IsAlive && indexManager.indexThread.ThreadState != ThreadState.AbortRequested) //crawling / indexing is currently not running, could start process
            {
                performDataCrawl();
                return true;
            }
            else //crawling / indexing in progress, abort
            {
                if(crawlThread.IsAlive)
                crawlThread.Abort(); //abort crawling thread (indexing cannot be cancelled, that would be fatal -> user wait..)
                MessageBox.Show("Crawling cancelled!" + Environment.NewLine + "Please wait for indexing to finish!", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        /// <summary>
        /// Starts process of data crawling. After crawling is finished, indexing is performed.
        /// </summary>
        public void performDataCrawl()
        {
            this.crawlThread = new Thread(new ThreadStart(parseWholeWebsite)); //init crawling thread, dnt start yet!
            crawlThread.Start();
            Task.Run(() =>
            {
                crawlThread.Join();
                indexManager.indexThread = new Thread(new ThreadStart(indexManager.performDataIndex));
                indexManager.indexThread.Start();
            });
        }
    }
}