using System.Collections.Generic;

namespace DrSearch
{
    /// <summary>
    /// Represents one webpage with article list. Provides access to webpage representation in html and plaintext.
    /// </summary>
    class Page
    {
        public string Url { get; set; } //url of the article
        public string Html { get; set; } //content of the page in html
        public string TidyText { get; set; } //plaintext of the page with respect to linebreaks
        public string AllText { get; set; } //plaintext of the page with removed linebreaks

        private List<Article> articles; //List of all articles (Article object) which belong to the page

        public Page()
        {
            articles = new List<Article>();
        }

        /// <summary>
        /// Returns List with Article objects which represent all articles on the webpage
        /// </summary>
        /// <returns>List<Article> with articles on the webpage.</returns>
        public List<Article> GetArticles()
        {
            return articles;
        }

        /// <summary>
        /// Assigns article (Article object) to the webpage object.
        /// </summary>
        /// <param name="article">Article object which represents one article.</param>
        public void addArticle(Article article)
        {
            articles.Add(article);
        }
    }
}
