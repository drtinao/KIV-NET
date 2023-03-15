namespace DrSearch
{
    /// <summary>
    /// Object which represents one document taken from test file - czechData.json.
    /// </summary>
    class TestDocIndex
    {
        public string date { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public string title { get; set; }

        public TestDocIndex()
        {
            this.date = "";
            this.id = "";
            this.title = "";
            this.text = "";
        }
    }
}
