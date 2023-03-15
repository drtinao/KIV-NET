
namespace DrSearch
{
    partial class SignInForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignInForm));
            this.loginTB = new System.Windows.Forms.TextBox();
            this.passwordTB = new System.Windows.Forms.TextBox();
            this.signInBTN = new System.Windows.Forms.Button();
            this.logTB = new System.Windows.Forms.TextBox();
            this.passTB = new System.Windows.Forms.TextBox();
            this.deleteAccBTN = new System.Windows.Forms.Button();
            this.signUpBTN = new System.Windows.Forms.Button();
            this.changePassBtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // loginTB
            // 
            this.loginTB.BackColor = System.Drawing.SystemColors.Control;
            this.loginTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.loginTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.loginTB.Location = new System.Drawing.Point(12, 12);
            this.loginTB.Name = "loginTB";
            this.loginTB.ReadOnly = true;
            this.loginTB.Size = new System.Drawing.Size(107, 16);
            this.loginTB.TabIndex = 1;
            this.loginTB.Text = "Login: ";
            // 
            // passwordTB
            // 
            this.passwordTB.BackColor = System.Drawing.SystemColors.Control;
            this.passwordTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.passwordTB.Location = new System.Drawing.Point(12, 34);
            this.passwordTB.Name = "passwordTB";
            this.passwordTB.ReadOnly = true;
            this.passwordTB.Size = new System.Drawing.Size(107, 16);
            this.passwordTB.TabIndex = 2;
            this.passwordTB.Text = "Password: ";
            // 
            // signInBTN
            // 
            this.signInBTN.Location = new System.Drawing.Point(12, 61);
            this.signInBTN.Name = "signInBTN";
            this.signInBTN.Size = new System.Drawing.Size(361, 23);
            this.signInBTN.TabIndex = 3;
            this.signInBTN.Text = "sign in";
            this.signInBTN.UseVisualStyleBackColor = true;
            this.signInBTN.Click += new System.EventHandler(this.signInBTN_Click);
            // 
            // logTB
            // 
            this.logTB.Location = new System.Drawing.Point(80, 8);
            this.logTB.Name = "logTB";
            this.logTB.Size = new System.Drawing.Size(293, 20);
            this.logTB.TabIndex = 0;
            // 
            // passTB
            // 
            this.passTB.Location = new System.Drawing.Point(80, 33);
            this.passTB.Name = "passTB";
            this.passTB.PasswordChar = '*';
            this.passTB.Size = new System.Drawing.Size(293, 20);
            this.passTB.TabIndex = 1;
            // 
            // deleteAccBTN
            // 
            this.deleteAccBTN.Location = new System.Drawing.Point(12, 111);
            this.deleteAccBTN.Name = "deleteAccBTN";
            this.deleteAccBTN.Size = new System.Drawing.Size(88, 23);
            this.deleteAccBTN.TabIndex = 11;
            this.deleteAccBTN.Text = "delete account";
            this.deleteAccBTN.UseVisualStyleBackColor = true;
            this.deleteAccBTN.Click += new System.EventHandler(this.deleteAccBTN_Click);
            // 
            // signUpBTN
            // 
            this.signUpBTN.Location = new System.Drawing.Point(285, 111);
            this.signUpBTN.Name = "signUpBTN";
            this.signUpBTN.Size = new System.Drawing.Size(88, 23);
            this.signUpBTN.TabIndex = 12;
            this.signUpBTN.Text = "sign up";
            this.signUpBTN.UseVisualStyleBackColor = true;
            this.signUpBTN.Click += new System.EventHandler(this.signUpBTN_Click);
            // 
            // changePassBtn
            // 
            this.changePassBtn.Location = new System.Drawing.Point(106, 111);
            this.changePassBtn.Name = "changePassBtn";
            this.changePassBtn.Size = new System.Drawing.Size(119, 23);
            this.changePassBtn.TabIndex = 13;
            this.changePassBtn.Text = "change password";
            this.changePassBtn.UseVisualStyleBackColor = true;
            this.changePassBtn.Click += new System.EventHandler(this.changePassBtn_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox1.Location = new System.Drawing.Point(12, 90);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(361, 16);
            this.textBox1.TabIndex = 14;
            this.textBox1.Text = "Other options";
            // 
            // SignInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 146);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.changePassBtn);
            this.Controls.Add(this.signUpBTN);
            this.Controls.Add(this.deleteAccBTN);
            this.Controls.Add(this.passTB);
            this.Controls.Add(this.logTB);
            this.Controls.Add(this.signInBTN);
            this.Controls.Add(this.passwordTB);
            this.Controls.Add(this.loginTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SignInForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign in";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SignInForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox loginTB;
        private System.Windows.Forms.TextBox passwordTB;
        private System.Windows.Forms.Button signInBTN;
        private System.Windows.Forms.TextBox logTB;
        private System.Windows.Forms.TextBox passTB;
        private System.Windows.Forms.Button deleteAccBTN;
        private System.Windows.Forms.Button signUpBTN;
        private System.Windows.Forms.Button changePassBtn;
        private System.Windows.Forms.TextBox textBox1;
    }
}