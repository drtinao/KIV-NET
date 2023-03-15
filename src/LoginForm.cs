using System;
using System.Windows.Forms;

namespace DrSearch
{
    public partial class SignInForm : Form
    {
        private DatabaseManager dbMan; //instance of class which is used for working with DB

        /// <summary>
        /// Form is used for user login.
        /// </summary>
        /// <param name="dbMan">for working with sqlite DB</param>
        public SignInForm(DatabaseManager dbMan)
        {
            this.dbMan = dbMan;
            InitializeComponent();
            this.logTB.MaxLength = 20;
            this.passTB.MaxLength = 20;
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

            return true;
        }

        /// <summary>
        /// Triggered when sign in btn is clicked -> check if user creds are valid, show dashboard.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void signInBTN_Click(object sender, EventArgs e)
        {
            if (!checkFields())
            {
                return;
            }

            String enteredLogin = logTB.Text;
            String enteredPass = passTB.Text;

            if (!dbMan.validLogin(enteredLogin, enteredPass)) //invalid login, show err
            {
                MessageBox.Show("Invalid login / password!" + Environment.NewLine + "Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else //ok login, show dashboard
            {
                dbMan.closeDBCon();
                DashboardForm dash = new DashboardForm();
                dash.Show();
                this.Visible = false;
            }
        }

        /// <summary>
        /// Triggered when sign up btn is clicked -> show registration window.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void signUpBTN_Click(object sender, EventArgs e)
        {
            SignUpForm signUp = new SignUpForm(dbMan);
            signUp.Show();
            this.Visible = false;
        }

        /// <summary>
        /// Triggered when button for deleting acc is cliked -> show alert / delete.
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void deleteAccBTN_Click(object sender, EventArgs e)
        {
            if (!checkFields())
            {
                return;
            }

            String enteredLogin = logTB.Text;
            String enteredPass = passTB.Text;

            //firstly check if user is allowed to delete the acc
            if (!dbMan.validLogin(enteredLogin, enteredPass)) //invalid login, show err
            {
                MessageBox.Show("Invalid login / password!" + Environment.NewLine + "You are not allowed to delete the account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else //ok, valid creds, delete user
            {
                DialogResult userResp = MessageBox.Show("Do you really want to delete your account?", "Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (userResp == DialogResult.Yes) //delete user
                {
                    dbMan.deleteUser(enteredLogin);
                    MessageBox.Show("Your account was deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    //do not delete acc if No clicked
                }
            }
        }

        /// <summary>
        /// Triggered when password change is requested
        /// </summary>
        /// <param name="sender">object which raised the event</param>
        /// <param name="e">contains some data related to event</param>
        private void changePassBtn_Click(object sender, EventArgs e)
        {
            if (!checkFields())
            {
                return;
            }

            String enteredLogin = logTB.Text;
            String enteredPass = passTB.Text;

            //firstly check if user is allowed to change the pass of acc
            if (!dbMan.validLogin(enteredLogin, enteredPass)) //invalid login, show err
            {
                MessageBox.Show("Invalid login / password!" + Environment.NewLine + "You are not allowed to chenge password regarding to the account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else //valid creds, go on with pass change
            {
                ChangePassForm changePassForm = new ChangePassForm(dbMan, enteredLogin);
                changePassForm.Show();
                this.Visible = false;
            }
        }

        private void SignInForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dbMan.closeDBCon();
            Application.Exit();
        }
    }
}
