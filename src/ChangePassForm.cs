using System;
using System.Windows.Forms;

namespace DrSearch
{
    public partial class ChangePassForm : Form
    {
        private DatabaseManager dbMan; //instance of class which is used for working with DB
        private String login; //user login

        /// <summary>
        /// Is used for changing user password.
        /// </summary>
        /// <param name="dbMan">for working with sqlite DB</param>
        /// <param name="login">users login</param>
        public ChangePassForm(DatabaseManager dbMan, String login)
        {
            this.dbMan = dbMan;
            this.login = login;
            InitializeComponent();
            this.passTB.MaxLength = 20;
            this.passAgainTB.MaxLength = 20;
        }

        /// <summary>
        /// Fields cannot be empty, check them.
        /// </summary>
        /// <returns>true if no empty field found, else false</returns>
        private bool checkFields()
        {
            if (passTB.Text.Length == 0)
            {
                MessageBox.Show("New password field cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (passAgainTB.Text.Length == 0)
            {
                MessageBox.Show("Repeat pass field cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Cancel btn returns user to form from which he/she can be logged in.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            SignInForm logIn = new SignInForm(dbMan);
            logIn.Show();
            this.Visible = false;
        }

        /// <summary>
        /// Triggered when request for pass change is triggered by user.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void changePassBtn_Click(object sender, EventArgs e)
        {
            if (!checkFields())
            {
                return;
            }

            if (!passTB.Text.Equals(passAgainTB.Text))
            {
                MessageBox.Show("Passwords do not match!" + Environment.NewLine + "Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult userResp = MessageBox.Show("Do you really want to change your password?", "Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (userResp == DialogResult.Yes) //change pass user
            {
                dbMan.updateUserPass(login, passTB.Text);
                MessageBox.Show("Your password was changed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SignInForm signIn = new SignInForm(dbMan);
                signIn.Show();
                this.Visible = false;
            }
            else
            {
                //do not delete acc if No clicked
            }
        }

        private void ChangePassForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dbMan.closeDBCon();
            Application.Exit();
        }
    }
}
