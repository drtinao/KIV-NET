using System;

namespace DrSearch
{
    /// <summary>
    /// Represents one match; query + document.
    /// </summary>
    public class CosSimMatch
    {
        public String query { get; set; } //text query
        public Article document { get; set; } //matching indexed doc
        public double cosSimScore { get; set; } //cos sim achieved score
        public bool isEmpty { get; set; } //if empty (needed for dashboard form logic, default true)

        public CosSimMatch()
        {
            this.isEmpty = false;
        }

        public override string ToString()
        {
            return document.TidyText;
        }
    }
}
