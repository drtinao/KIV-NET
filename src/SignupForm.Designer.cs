
namespace DrSearch
{
    partial class SignUpForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignUpForm));
            this.passTB = new System.Windows.Forms.TextBox();
            this.logTB = new System.Windows.Forms.TextBox();
            this.passwordTB = new System.Windows.Forms.TextBox();
            this.loginTB = new System.Windows.Forms.TextBox();
            this.passAgainTB = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.signInBTN = new System.Windows.Forms.Button();
            this.signUpBTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // passTB
            // 
            this.passTB.Location = new System.Drawing.Point(95, 35);
            this.passTB.Name = "passTB";
            this.passTB.PasswordChar = '*';
            this.passTB.Size = new System.Drawing.Size(278, 20);
            this.passTB.TabIndex = 1;
            // 
            // logTB
            // 
            this.logTB.Location = new System.Drawing.Point(95, 10);
            this.logTB.Name = "logTB";
            this.logTB.Size = new System.Drawing.Size(278, 20);
            this.logTB.TabIndex = 0;
            // 
            // passwordTB
            // 
            this.passwordTB.BackColor = System.Drawing.SystemColors.Control;
            this.passwordTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.passwordTB.Location = new System.Drawing.Point(12, 39);
            this.passwordTB.Name = "passwordTB";
            this.passwordTB.ReadOnly = true;
            this.passwordTB.Size = new System.Drawing.Size(107, 16);
            this.passwordTB.TabIndex = 12;
            this.passwordTB.Text = "Password: ";
            // 
            // loginTB
            // 
            this.loginTB.BackColor = System.Drawing.SystemColors.Control;
            this.loginTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.loginTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.loginTB.Location = new System.Drawing.Point(12, 14);
            this.loginTB.Name = "loginTB";
            this.loginTB.ReadOnly = true;
            this.loginTB.Size = new System.Drawing.Size(107, 16);
            this.loginTB.TabIndex = 11;
            this.loginTB.Text = "Login: ";
            // 
            // passAgainTB
            // 
            this.passAgainTB.Location = new System.Drawing.Point(95, 61);
            this.passAgainTB.Name = "passAgainTB";
            this.passAgainTB.PasswordChar = '*';
            this.passAgainTB.Size = new System.Drawing.Size(278, 20);
            this.passAgainTB.TabIndex = 2;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Control;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox3.Location = new System.Drawing.Point(12, 65);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(107, 16);
            this.textBox3.TabIndex = 16;
            this.textBox3.Text = "Repeat pass:";
            // 
            // signInBTN
            // 
            this.signInBTN.Location = new System.Drawing.Point(12, 86);
            this.signInBTN.Name = "signInBTN";
            this.signInBTN.Size = new System.Drawing.Size(88, 23);
            this.signInBTN.TabIndex = 19;
            this.signInBTN.Text = "cancel";
            this.signInBTN.UseVisualStyleBackColor = true;
            this.signInBTN.Click += new System.EventHandler(this.signInBTN_Click);
            // 
            // signUpBTN
            // 
            this.signUpBTN.Location = new System.Drawing.Point(203, 86);
            this.signUpBTN.Name = "signUpBTN";
            this.signUpBTN.Size = new System.Drawing.Size(170, 23);
            this.signUpBTN.TabIndex = 18;
            this.signUpBTN.Text = "sign up";
            this.signUpBTN.UseVisualStyleBackColor = true;
            this.signUpBTN.Click += new System.EventHandler(this.signUpBTN_Click);
            // 
            // SignUpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 121);
            this.Controls.Add(this.signInBTN);
            this.Controls.Add(this.signUpBTN);
            this.Controls.Add(this.passAgainTB);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.passTB);
            this.Controls.Add(this.logTB);
            this.Controls.Add(this.passwordTB);
            this.Controls.Add(this.loginTB);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SignUpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign up";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SignUpForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox passTB;
        private System.Windows.Forms.TextBox logTB;
        private System.Windows.Forms.TextBox passwordTB;
        private System.Windows.Forms.TextBox loginTB;
        private System.Windows.Forms.TextBox passAgainTB;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button signInBTN;
        private System.Windows.Forms.Button signUpBTN;
    }
}