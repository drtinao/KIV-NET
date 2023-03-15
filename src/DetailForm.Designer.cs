
namespace DrSearch
{
    partial class DetailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailForm));
            this.titleTB = new System.Windows.Forms.TextBox();
            this.idTB = new System.Windows.Forms.TextBox();
            this.textTB = new System.Windows.Forms.TextBox();
            this.dateTB = new System.Windows.Forms.TextBox();
            this.relToTB = new System.Windows.Forms.TextBox();
            this.achCosSimTB = new System.Windows.Forms.TextBox();
            this.textTBCont = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // titleTB
            // 
            this.titleTB.BackColor = System.Drawing.SystemColors.Control;
            this.titleTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.titleTB.Location = new System.Drawing.Point(1, 3);
            this.titleTB.Name = "titleTB";
            this.titleTB.Size = new System.Drawing.Size(679, 16);
            this.titleTB.TabIndex = 0;
            this.titleTB.Text = "Title: ";
            // 
            // idTB
            // 
            this.idTB.BackColor = System.Drawing.SystemColors.Control;
            this.idTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.idTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.idTB.Location = new System.Drawing.Point(1, 25);
            this.idTB.Name = "idTB";
            this.idTB.Size = new System.Drawing.Size(679, 16);
            this.idTB.TabIndex = 1;
            this.idTB.Text = "Id: ";
            // 
            // textTB
            // 
            this.textTB.BackColor = System.Drawing.SystemColors.Control;
            this.textTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textTB.Location = new System.Drawing.Point(1, 136);
            this.textTB.Name = "textTB";
            this.textTB.Size = new System.Drawing.Size(679, 19);
            this.textTB.TabIndex = 2;
            this.textTB.Text = "Text";
            // 
            // dateTB
            // 
            this.dateTB.BackColor = System.Drawing.SystemColors.Control;
            this.dateTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dateTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dateTB.Location = new System.Drawing.Point(1, 47);
            this.dateTB.Name = "dateTB";
            this.dateTB.Size = new System.Drawing.Size(679, 16);
            this.dateTB.TabIndex = 3;
            this.dateTB.Text = "Date: ";
            // 
            // relToTB
            // 
            this.relToTB.BackColor = System.Drawing.SystemColors.Control;
            this.relToTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.relToTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.relToTB.Location = new System.Drawing.Point(1, 92);
            this.relToTB.Name = "relToTB";
            this.relToTB.Size = new System.Drawing.Size(679, 16);
            this.relToTB.TabIndex = 8;
            this.relToTB.Text = "Rel. to query: ";
            // 
            // achCosSimTB
            // 
            this.achCosSimTB.BackColor = System.Drawing.SystemColors.Control;
            this.achCosSimTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.achCosSimTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.achCosSimTB.Location = new System.Drawing.Point(1, 114);
            this.achCosSimTB.Name = "achCosSimTB";
            this.achCosSimTB.Size = new System.Drawing.Size(679, 16);
            this.achCosSimTB.TabIndex = 9;
            this.achCosSimTB.Text = "Achieved cos. sim.: ";
            // 
            // textTBCont
            // 
            this.textTBCont.BackColor = System.Drawing.SystemColors.Control;
            this.textTBCont.Location = new System.Drawing.Point(1, 161);
            this.textTBCont.Name = "textTBCont";
            this.textTBCont.ReadOnly = true;
            this.textTBCont.Size = new System.Drawing.Size(679, 269);
            this.textTBCont.TabIndex = 10;
            this.textTBCont.Text = "";
            this.textTBCont.TextChanged += new System.EventHandler(this.textTBCont_TextChanged_1);
            // 
            // DetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 442);
            this.Controls.Add(this.textTBCont);
            this.Controls.Add(this.achCosSimTB);
            this.Controls.Add(this.relToTB);
            this.Controls.Add(this.dateTB);
            this.Controls.Add(this.textTB);
            this.Controls.Add(this.idTB);
            this.Controls.Add(this.titleTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Article detail";
            this.Load += new System.EventHandler(this.IndexingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox titleTB;
        private System.Windows.Forms.TextBox idTB;
        private System.Windows.Forms.TextBox textTB;
        private System.Windows.Forms.TextBox dateTB;
        private System.Windows.Forms.TextBox relToTB;
        private System.Windows.Forms.TextBox achCosSimTB;
        private System.Windows.Forms.RichTextBox textTBCont;
    }
}