using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DrSearch
{
    /// <summary>
    /// Used for showing article details.
    /// </summary>
    public partial class DetailForm : Form
    {
        private String title;
        private String id;
        private DateTime date;
        private String text;

        private String relToQuery;
        private String achievedCosSim;

        public DetailForm(String title, String id, DateTime date, String text, String relToQuery, String achievedCosSim)
        {
            this.title = title;
            this.id = id;
            this.date = date;
            this.text = text;

            this.relToQuery = relToQuery;
            this.achievedCosSim = achievedCosSim;

            InitializeComponent();
        }

        private void IndexingForm_Load(object sender, EventArgs e)
        {
            textTB.TextAlign = HorizontalAlignment.Center;

            titleTB.Text += title;
            idTB.Text += id;
            dateTB.Text += date.ToString();
            textTBCont.Text += text;
            relToTB.Text += relToQuery;
            achCosSimTB.Text += achievedCosSim;
        }

        private void textTBCont_TextChanged(object sender, EventArgs e)
        {
            if (textTBCont.Text.ToLower().Contains(relToQuery.ToLower()))
            {
                var matchString = Regex.Escape(relToQuery);
                foreach (Match match in Regex.Matches(textTBCont.Text, matchString))
                {
                    textTBCont.Select(match.Index, relToQuery.Length);
                    textTBCont.SelectionColor = Color.Aqua;
                    textTBCont.Select(textTBCont.TextLength, 0);
                    textTBCont.SelectionColor = textTBCont.ForeColor;
                };
            }
        }

        private void titleTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void textTBCont_TextChanged_1(object sender, EventArgs e)
        {
            List<String> wordsInQuery = Regex.Split(relToQuery.ToLower(), "(and)|(or)|(not)|\\)|\\(| ").ToList(); //represents words in search query
            foreach (string word in wordsInQuery) //go through words in query
            {
                if (word.Equals("and") || word.Equals("or") || word.Equals("not"))
                {
                    continue;
                }
                if (textTBCont.Text.ToLower().Contains(word))
                {
                    var matches = Regex.Escape(word);
                    foreach (Match match in Regex.Matches(textTBCont.Text.ToLower(), matches))
                    {
                        textTBCont.Select(match.Index, word.Length);
                        textTBCont.SelectionColor = Color.Red;
                        textTBCont.Select(textTBCont.TextLength, 0);
                        textTBCont.SelectionColor = textTBCont.ForeColor;
                    };
                }
            }
        }
    }
}
