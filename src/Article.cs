using System;

namespace DrSearch
{
    /// <summary>
    /// Represents one article found on the webpage. Provides access to article representation in html and plaintext.
    /// </summary>
    public class Article
    {
        public Article()
        {
            this.isTestData = false;
        }

        public string Url { get; set; } //url of the article - in test data defined as: title, getTitle()
        public string Html { get; set; } //content of the article in html - in test data: id, getId()
        public string TidyText { get; set; } //plaintext of the article with respect to linebreaks - in test data defined as: text, getText()
        public string AllText { get; set; } //plaintext of the article with removed linebreaks - in test data: NOT PRESENT!!!
        public DateTime DateIndex { get; set; } //date of doc index - in test data defined as: date, getDate()
        public bool isTestData { get; set; } //true if article is created from test data (not crawled ones)
        public override string ToString()
        {
            return TidyText;
        }
    }
}
