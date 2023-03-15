using System;
using System.Windows.Forms;

namespace DrSearch
{
    public partial class SignUpForm : Form
    {
        private DatabaseManager dbMan; //instance of class which is used for working with DB

        /// <summary>
        /// Form is used for user registration.
        /// </summary>
        /// <param name="dbMan">for working with sqlite DB</param>
        public SignUpForm(DatabaseManager dbMan)
        {
            this.dbMan = dbMan;
            InitializeComponent();
            this.logTB.MaxLength = 20;
            this.passTB.MaxLength = 20;
            this.passAgainTB.MaxLength = 20;
        }

        /// <summary>
        /// Triggered when sign in btn is clicked -> show login form.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void signInBTN_Click(object sender, EventArgs e)
        {
            SignInForm signIn = new SignInForm(dbMan);
            signIn.Show();
            this.Visible = false;
        }

        /// <summary>
        /// Fields cannot be empty, check them.
        /// </summary>
        /// <returns>true if no empty field found, else false</returns>
        private bool checkFields()
        {
            if (logTB.Text.Length == 0)
            {
                MessageBox.Show("Login field cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (passTB.Text.Length == 0)
            {
                MessageBox.Show("Password field cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        /// Triggered when sign up btn is clicked -> perform register.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void signUpBTN_Click(object sender, EventArgs e)
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

            if (!dbMan.validUsername(logTB.Text)) //user already exists
            {
                MessageBox.Show("User already exist!" + Environment.NewLine + "Please choose another login.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else //user doesnt exist, create user
            {
                dbMan.createUser(logTB.Text, passTB.Text);
                MessageBox.Show("Your account was created!" + Environment.NewLine + "You can now proceed to sign in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SignInForm signIn = new SignInForm(dbMan);
                signIn.Show();
                this.Visible = false;
            }
        }

        private void SignUpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dbMan.closeDBCon();
            Application.Exit();
        }
    }
}
