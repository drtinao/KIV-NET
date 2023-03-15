using System;
using System.Windows.Forms;

namespace DrSearch
{
    static class Program
    {
        /// <summary>
        /// Hlavní vstupní bod aplikace.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SignInForm(new DatabaseManager("DrSearchDB.sqlite")));
        }
    }
}
