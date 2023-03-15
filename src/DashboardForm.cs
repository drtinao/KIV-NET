using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrSearch
{
    public partial class DashboardForm : Form
    {
        private readonly SynchronizationContext syncCont; //target of the synchronization, that is dashboard window
        private DateTime prevDT = DateTime.Now; //for keeping previous time of GUI synchronization
        private String infoConsoleBuffer; //information which user have not seen yet ; waiting to be pushed into console
        private String indexModeBuffer; //buffer of label which reflects currently loaded index
        private List<String> searchModeListBuffer; //buffer of items which should be displayed in search mode combobox
        private String crawlDataTextBuffer; //buffer of crawlDataBtn, text to display on it

        private List<CosSimMatch> latestArticlesShown; //articles which are currently displayed
        private List<CosSimMatch> articlesToAddBuffer; //articles not shown yet ; waiting to be pushed to relevant documents window
        public bool indexVectorMode { get; set; }
        private string notFound { get; set; } //word which was not found
        private string latestQuery { get; set; } //latest query which was searched

        private CrawlerCore cc; //component which is responsible for crawling data from site
        private TfIdfCore tfIdfCore; //instance responsible for tf-idf calculations 
        private IndexManager indexManager; //used for managing indexed documents (loading + saving)

        public DashboardForm()
        {
            InitializeComponent();
            syncCont = SynchronizationContext.Current; //context from UI thread
            crawlDataTextBuffer = "";
            infoConsoleBuffer = ""; //information window empty on start
            indexModeBuffer = ""; //do not change mode on start
            searchModeListBuffer = new List<String>(); //nothing to display in search mode cb on app start
            articlesToAddBuffer = new List<CosSimMatch>(); //no relevant articles visible on start
            latestArticlesShown = new List<CosSimMatch>(); //no relevant articles visible on start

            //create instances which will be later needed for calculations and relevant documents retrieving - START
            tfIdfCore = new TfIdfCore();
            indexManager = new IndexManager("index.drsearch", "indexTestData.drsearch", tfIdfCore, this);
            cc = new CrawlerCore(indexManager, tfIdfCore, this);
            indexManager.assignCrawlerCore(cc);
            //create instances which will be later needed for calculations and relevant documents retrieving - END
            notFound = "";
            latestQuery = "";
            initGUIUpdater();
        }

        /// <summary>
        /// Method inits GUI updater.
        /// </summary>
        private async void initGUIUpdater()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    UpdateGUI();
                }
            });
        }

        /// <summary>
        /// Method takes care of GUI updating, showing content of the infoConsoleBuffer.
        /// </summary>
        public async void UpdateGUI()
        {
            var actualDT = DateTime.Now;

            if ((DateTime.Now - prevDT).Milliseconds <= 50) return; //refresh gui every 50ms

            //wake up target
            syncCont.Post(new SendOrPostCallback(o =>
            {
                if (infoConsoleBuffer.Length != 0)
                {
                    progInfoTB.Text += infoConsoleBuffer; //append content of the buffer to information console
                    progInfoTB.SelectionStart = progInfoTB.Text.Length;
                    progInfoTB.ScrollToCaret();
                    infoConsoleBuffer = ""; //remove content of the information buffer, already shown
                }

                if (articlesToAddBuffer.Count != 0)
                { //buffer not empty
                    if (articlesToAddBuffer.Count == 1 && articlesToAddBuffer[0].isEmpty)
                    { //no res found, just delete prev content
                        relDocsLB.Items.Clear();
                        latestArticlesShown.Clear();
                        articlesToAddBuffer = new List<CosSimMatch>(); //remove content of the article buffer, already shown
                    }
                    else
                    { //articles found, display
                        relDocsLB.Items.Clear();
                        relDocsLB.Items.AddRange(articlesToAddBuffer.ToArray());
                        latestArticlesShown.Clear();
                        latestArticlesShown.AddRange(articlesToAddBuffer);
                        articlesToAddBuffer = new List<CosSimMatch>(); //remove content of the article buffer, already shown
                    }
                }

                if(crawlDataTextBuffer.Length != 0)
                {
                    crawlDataBtn.Text = crawlDataTextBuffer;
                    crawlDataTextBuffer = "";
                }

                if (indexModeBuffer.Length != 0)
                {
                    curIndexLB.Text = indexModeBuffer;
                    indexModeBuffer = "";
                }

                if (searchModeListBuffer.Count != 0)
                {
                    searchModeCB.Items.Clear();
                    searchModeCB.Items.AddRange(searchModeListBuffer.ToArray());
                    searchModeCB.SelectedIndex = 0;
                    searchModeCB.Enabled = true;
                    searchModeListBuffer = new List<String>();
                }

                updateDidYouMeanTB();
                resCountLB.Text = "Result count: " + latestArticlesShown.Count();
            }), 0);

            prevDT = actualDT;
        }

        /// <summary>
        /// Used for updating content of updateDidYouMeanTB based on the latest word which user wrote.
        /// </summary>
        private void updateDidYouMeanTB()
        {
            if(searchInputTB.Text.Length == 0 || searchInputTB.Text.Equals(" ")) //no search yet entered
            {
                return;
            }

            if (latestQuery.Equals(searchInputTB.Text))
            {
                return;
            }
            else
            {
                latestQuery = searchInputTB.Text;
            }

            string[] splittedInput = Regex.Split(searchInputTB.Text, "(AND)|(OR)|(NOT)|\\)|\\(| "); //split by keywords to get latest word which user started to write
            if (splittedInput[splittedInput.Length - 1].StartsWith(notFound) && notFound.Length != 0)
            {
                return;
            }

            if (indexManager.validDocumentsNumsLoaded == null || splittedInput.Length == 0) //no index loaded, do not check
            {
               return;
            }
            else //index loaded, check what word is suitable
            {
                String matchWord = indexManager.validDocumentsNumsLoaded.Keys.Where(x => x.StartsWith(splittedInput[splittedInput.Length - 1])).FirstOrDefault();
                if(matchWord != null && matchWord.Length != 0)
                {
                    didYouMeanTB.Text = "Did you mean: " + matchWord;
                }
                else
                {
                    notFound = splittedInput[splittedInput.Length - 1];
                    return;
                }
            }
        }

        /// <summary>
        /// Contains operations which should be performed when main form is loaded -> show search method list etc.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void DashboardForm_Load(object sender, EventArgs e)
        {
            curIndexLB.Text = "Currently loaded index: NO INDEX!";
            indexVectorMode = true; //by default index with tfidf counts
            searchMethodLB.Items.Add("Vector space model");
            searchMethodLB.Items.Add("Boolean model");

            DateTime crawlDataCreationTime = CrawlerCore.retrieveDataCrawlTime();
        }

        /// <summary>
        /// Used to update text display on crawlDataBtn.
        /// </summary>
        /// <param name="textToDis">texto to display on button crawlDataBtn</param>
        public void changeCrawlDataText(String textToDis)
        {
            crawlDataTextBuffer = textToDis;
        }

        /// <summary>
        /// Appends Article object to buffer of relDocsLB GUI component.
        /// </summary>
        /// <param name="articleToAdd">CosSimMatch object which represents article which should be shown</param>
        public void addToRelDocsLBBuffer(CosSimMatch articleToAdd)
        {
            articlesToAddBuffer.Add(articleToAdd);
        }

        /// <summary>
        /// Appends given text to buffer of progInfoTB GUI component.
        /// </summary>
        /// <param name="textToAppend">text which should be displayed in information window</param>
        public void appendToProgInfoTBBuffer(String textToAppend)
        {
            infoConsoleBuffer += textToAppend + Environment.NewLine;
        }

        /// <summary>
        /// Takes care of changing label with loaded index.
        /// </summary>
        /// <param name="loadedIndex">text desc of loaded index: crawled data + vector space, crawled data + boolean model, test data + vector space, test data + boolean model</param>
        public void changeLoadedIndexInfo(String loadedIndex)
        {
            indexModeBuffer = loadedIndex;

            List<String> searchModesToShow = new List<String>();
            switch (loadedIndex) {
                case "Currently loaded index: test data + vector space": //vector data can work with bool search
                    searchModesToShow.Add("Boolean model");
                    searchModesToShow.Add("Vector space model");
                    break;

                case "Currently loaded index: test data + boolean model": //VECTOR not available!
                    searchModesToShow.Add("Boolean model");
                    break;

                case "Currently loaded index: crawled data + vector space": //vector data can work with bool search
                    searchModesToShow.Add("Boolean model");
                    searchModesToShow.Add("Vector space model");
                    break;

                case "Currently loaded index: crawled data + boolean model": //VECTOR not available!
                    searchModesToShow.Add("Boolean model");
                    break;
            }
            searchModeListBuffer = searchModesToShow;
        }

        /// <summary>
        /// Triggered when button for data crawling / indexing is clicked. Calls other methods which result in starting/stopping of crawling process.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void crawlDataBtn_Click(object sender, EventArgs e)
        {
            if (cc.benchThread.IsAlive)
            { //bench running, err
                MessageBox.Show("Benchmark in progress!" + Environment.NewLine + "Please wait!", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!cc.handleCrawlDataBtnClick()) //crawling was in progress -> aborted
            {
                crawlDataBtn.Text = "1) crawl data and build index";
            }
            else //crawling started
            {
                crawlDataBtn.Text = "STOP CRAWLING / INDEXING";
            }
        }

        /// <summary>
        /// Called when search button is clicked. Search can be performed only when data already crawled + indexed, ie. these operations are not currently NOT running.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void searchBtn_Click(object sender, EventArgs e)
        {
            String phraseToSearch = searchInputTB.Text;

            if (cc.benchThread.IsAlive)
            { //bench running, err
                MessageBox.Show("Benchmark in progress!" + Environment.NewLine + "Please wait!", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (searchModeCB.SelectedIndex == 0) //bool search
            {
                var searchThread = new Thread(() => cc.handleSearchBtnClick(phraseToSearch, false));
                searchThread.Start();
            }
            else //vect mode search
            {
                var searchThread = new Thread(() => cc.handleSearchBtnClick(phraseToSearch, true));
                searchThread.Start();
            }
        }

        /// <summary>
        /// Trigerred when button for loading index WITH CRAWL DATA is pressed. Takes care of index load.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void loadIndexBtn_Click(object sender, EventArgs e)
        {
            if (cc.benchThread.IsAlive)
            { //bench running, err
                MessageBox.Show("Benchmark in progress!" + Environment.NewLine + "Please wait!", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //check if crawling / indexing not in progress -> if so, disp alert
            if (cc.crawlThread.IsAlive || indexManager.indexThread.IsAlive) //crawling already in progress, err
            {
                MessageBox.Show("Crawling / indexing in progress!" + Environment.NewLine + "Please wait.", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //perform data index reload
            indexManager.indexThread = new Thread(new ThreadStart(indexManager.performCrawlIndexReload));
            indexManager.indexThread.Start();
        }

        /// <summary>
        /// Button triggers indexing of TEST data.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void buildIndexTestBtn_Click(object sender, EventArgs e)
        {
            if (cc.benchThread.IsAlive)
            { //bench running, err
                MessageBox.Show("Benchmark in progress!" + Environment.NewLine + "Please wait!", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //check if crawling / indexing not in progress -> if so, disp alert
            if (cc.crawlThread.IsAlive || indexManager.indexThread.IsAlive) //crawling already in progress, err
            {
                MessageBox.Show("Crawling / indexing in progress!" + Environment.NewLine + "Please wait.", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            indexManager.indexThread = new Thread(new ThreadStart(indexManager.performTestDataIndex));
            indexManager.indexThread.Start();
        }

        /// <summary>
        /// Changes value of variable which tells whether indexing should be performed with vector space model or boolean model.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void searchMethodLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(searchMethodLB.SelectedIndex == 0)
            {
                indexVectorMode = true;
            }
            else
            {
                indexVectorMode = false;
            }
        }

        /// <summary>
        /// Trigerred when button for loading index WITH TEST DATA is pressed. Takes care of index load.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void loadIndexTestBtn_Click(object sender, EventArgs e)
        {
            if (cc.benchThread.IsAlive)
            { //bench running, err
                MessageBox.Show("Benchmark in progress!" + Environment.NewLine + "Please wait!", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //check if crawling / indexing not in progress -> if so, disp alert
            if (cc.crawlThread.IsAlive || indexManager.indexThread.IsAlive) //crawling already in progress, err
            {
                MessageBox.Show("Crawling / indexing in progress!" + Environment.NewLine + "Please wait.", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //perform data index reload
            indexManager.indexThread = new Thread(new ThreadStart(indexManager.performTestIndexReload));
            indexManager.indexThread.Start();
        }

        /// <summary>
        /// Called when article from relevant documents section is selected.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void relDocsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (relDocsLB.SelectedIndex == -1) //user clicked outside title
                return;
            CosSimMatch cosSimMatch = latestArticlesShown.ElementAt(relDocsLB.SelectedIndex);
            Article selectedArticle = cosSimMatch.document;

            DetailForm detForm;
            if (!selectedArticle.isTestData)
            {
                detForm = new DetailForm(selectedArticle.Url, selectedArticle.Url, selectedArticle.DateIndex, selectedArticle.TidyText, cosSimMatch.query, cosSimMatch.cosSimScore.ToString());
            }
            else
            {
                detForm = new DetailForm(selectedArticle.Url, selectedArticle.Html, selectedArticle.DateIndex, selectedArticle.TidyText, cosSimMatch.query, cosSimMatch.cosSimScore.ToString());
            }
            detForm.Show();
        }

        /// <summary>
        /// Trigerred when benchmark should be performed. Prereq: loaded test data with in VECTOR MODE!
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void runBenchBtn_Click(object sender, EventArgs e)
        {
            if (cc.benchThread.IsAlive)
            { //bench already running, err
                MessageBox.Show("Benchmark in progress!" + Environment.NewLine + "Please wait!", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //check if crawling / indexing not in progress -> if so, disp alert
            if (cc.crawlThread.IsAlive || indexManager.indexThread.IsAlive) //crawling already in progress, err
            {
                MessageBox.Show("Crawling / indexing in progress!" + Environment.NewLine + "Please wait.", "Working", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (indexManager.loadedIndexType != 0) //vector mode with test data not loaded...
            {
                MessageBox.Show("No index / wrong index loaded." + Environment.NewLine + "Please load test data index with vector mode to perform benchmark!", "Wrong index", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //suitable index loaded - go on, load file with search queries

            cc.benchThread = new Thread(new ThreadStart(cc.runBench));
            cc.benchThread.Start();
        }

        private void DashboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
