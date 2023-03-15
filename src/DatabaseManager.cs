using System;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace DrSearch
{
   public class DatabaseManager
    {
        private readonly String pathToDB; //path to file which contains sqlite DB
        private SQLiteConnection fileDBCon; //represents connection to SQLite db

        /// <summary>
        /// Takes reference to path to sqlite DB file and inits connection to DB.
        /// </summary>
        /// <param name="pathToDB">path to sqlite DB file</param>
        public DatabaseManager(String pathToDB)
        {
            this.pathToDB = pathToDB;
            this.fileDBCon = new SQLiteConnection("Data Source=" + this.pathToDB + ";Version=3;"); //init connection to DB
            this.fileDBCon.Open(); //open connection to DB
            createDBTables();
        }

        /// <summary>
        /// Used when new DB file is created -> create required tables.
        /// </summary>
        private void createDBTables()
        {
            String sqlComContCreate = "CREATE TABLE IF NOT EXISTS logins (login VARCHAR(20), password VARCHAR(32))"; //table with users - pass will be saved as MD5 hash
            SQLiteCommand sqlComCreate = new SQLiteCommand(sqlComContCreate, fileDBCon);
            sqlComCreate.ExecuteNonQuery();
        }

        /// <summary>
        /// Creates DB entry for new user with given login a password.
        /// </summary>
        /// <param name="login">login of new user</param>
        /// <param name="password">password of new user</param>
        public void createUser(String login, String password)
        {
            login = escapeString(login);
            password = escapeString(password);
            var md5 = new MD5CryptoServiceProvider();
            var md5Pass = md5.ComputeHash(new MemoryStream(Encoding.UTF8.GetBytes(password ?? "")));
            String md5HashedPass = escapeString(new ASCIIEncoding().GetString(md5Pass));
            String sqlComCont = "INSERT INTO logins (login, password) VALUES (" + login + ", " + md5HashedPass + ")";
            SQLiteCommand sqlCom = new SQLiteCommand(sqlComCont, fileDBCon);
            sqlCom.ExecuteNonQuery();
        }

        /// <summary>
        /// Finds DB entry which contains given login and updates password regarding to the entry.
        /// </summary>
        /// <param name="login">login of user who wants to change password</param>
        /// <param name="password">new user password</param>
        public void updateUserPass(String login, String password)
        {
            login = escapeString(login);
            password = escapeString(password);
            var md5 = new MD5CryptoServiceProvider();
            var md5Pass = md5.ComputeHash(new MemoryStream(Encoding.UTF8.GetBytes(password ?? "")));
            String md5HashedPass = escapeString(new ASCIIEncoding().GetString(md5Pass));
            String sqlComCont = "UPDATE logins SET password=" + md5HashedPass + " WHERE login=" + login;
            SQLiteCommand sqlCom = new SQLiteCommand(sqlComCont, fileDBCon);
            sqlCom.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes user with given login from sqlite database file.
        /// </summary>
        /// <param name="login">login of user who wants to be deleted</param>
        public void deleteUser(String login)
        {
            login = escapeString(login);
            String sqlComCont = "DELETE FROM logins" + " WHERE login=" + login;
            SQLiteCommand sqlCom = new SQLiteCommand(sqlComCont, fileDBCon);
            sqlCom.ExecuteNonQuery();
        }

        /// <summary>
        /// Tells whether login credentials enetered by user are valid or not.
        /// </summary>
        /// <param name="login">login entered by user</param>
        /// <param name="password">password entered by user</param>
        /// <returns>true if login is valid, else false</returns>
        public bool validLogin(String login, String password)
        {
            login = escapeString(login);
            password = escapeString(password);
            var md5 = new MD5CryptoServiceProvider();
            var md5Pass = md5.ComputeHash(new MemoryStream(Encoding.UTF8.GetBytes(password ?? "")));
            String md5HashedPass = escapeString(new ASCIIEncoding().GetString(md5Pass));
            String sqlComCont = "SELECT * FROM logins WHERE login="+ login + " AND password=" + md5HashedPass;
            SQLiteCommand sqlCom = new SQLiteCommand(sqlComCont, fileDBCon);
            SQLiteDataReader reader = sqlCom.ExecuteReader();

            int resCount = 0;
            while (reader.Read())
            {
                resCount += 1;
            }

            if(resCount == 1) //exactly one user, ok
            {
                return true;
            }
            else //zero or > 1 user found, err
            {
                return false;
            }
        }

        /// <summary>
        /// Tells whether login already exists or not.
        /// </summary>
        /// <param name="login">login entered by user</param>
        /// <returns>true if login is valid, else false</returns>
        public bool validUsername(String login)
        {
            login = escapeString(login);
            String sqlComCont = "SELECT * FROM logins WHERE login=" + login;
            SQLiteCommand sqlCom = new SQLiteCommand(sqlComCont, fileDBCon);
            SQLiteDataReader reader = sqlCom.ExecuteReader();

            int resCount = 0;
            while (reader.Read())
            {
                resCount += 1;
            }

            if (resCount == 0) //no user found
            {
                return true;
            }
            else //user already exists
            {
                return false;
            }
        }

        /// <summary>
        /// Creates String which has escaped criticals chars, used for parsing login and password.
        /// </summary>
        /// <param name="toEscape">string which should be escaped</param>
        /// <returns>escaped string</returns>
        private String escapeString(String toEscape)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(toEscape), writer, null);
                    return writer.ToString();
                }
            }
        }

        /// <summary>
        /// Just close connection to DB.
        /// </summary>
        public void closeDBCon(){
            if(this.fileDBCon != null) //if not already destroyed by garbage col
            this.fileDBCon.Close(); //close DB con
        }
    }
}
