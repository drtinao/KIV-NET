
namespace DrSearch
{
    partial class ChangePassForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePassForm));
            this.passAgainTB = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.passTB = new System.Windows.Forms.TextBox();
            this.passwordTB = new System.Windows.Forms.TextBox();
            this.changePassBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // passAgainTB
            // 
            this.passAgainTB.Location = new System.Drawing.Point(112, 38);
            this.passAgainTB.Name = "passAgainTB";
            this.passAgainTB.PasswordChar = '*';
            this.passAgainTB.Size = new System.Drawing.Size(261, 20);
            this.passAgainTB.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Control;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox3.Location = new System.Drawing.Point(12, 42);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(107, 16);
            this.textBox3.TabIndex = 20;
            this.textBox3.Text = "Repeat pass:";
            // 
            // passTB
            // 
            this.passTB.Location = new System.Drawing.Point(112, 12);
            this.passTB.Name = "passTB";
            this.passTB.PasswordChar = '*';
            this.passTB.Size = new System.Drawing.Size(261, 20);
            this.passTB.TabIndex = 0;
            // 
            // passwordTB
            // 
            this.passwordTB.BackColor = System.Drawing.SystemColors.Control;
            this.passwordTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.passwordTB.Location = new System.Drawing.Point(12, 16);
            this.passwordTB.Name = "passwordTB";
            this.passwordTB.ReadOnly = true;
            this.passwordTB.Size = new System.Drawing.Size(107, 16);
            this.passwordTB.TabIndex = 18;
            this.passwordTB.Text = "New password: ";
            // 
            // changePassBtn
            // 
            this.changePassBtn.Location = new System.Drawing.Point(203, 64);
            this.changePassBtn.Name = "changePassBtn";
            this.changePassBtn.Size = new System.Drawing.Size(170, 23);
            this.changePassBtn.TabIndex = 22;
            this.changePassBtn.Text = "change password";
            this.changePassBtn.UseVisualStyleBackColor = true;
            this.changePassBtn.Click += new System.EventHandler(this.changePassBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(12, 64);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(88, 23);
            this.cancelBtn.TabIndex = 23;
            this.cancelBtn.Text = "cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // ChangePassForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 95);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.changePassBtn);
            this.Controls.Add(this.passAgainTB);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.passTB);
            this.Controls.Add(this.passwordTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ChangePassForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change password";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChangePassForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox passAgainTB;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox passTB;
        private System.Windows.Forms.TextBox passwordTB;
        private System.Windows.Forms.Button changePassBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}