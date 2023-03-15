namespace DrSearch
{
    /// <summary>
    /// Object which represents one test query from file - topicData.json.
    /// </summary>
    class TestDocQuery
    {
        public string narrative { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string lang { get; set; }

        public TestDocQuery()
        {
            this.narrative = "";
            this.description = "";
            this.id = "";
            this.title = "";
            this.lang = "";
        }
    }
}
