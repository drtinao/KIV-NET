
namespace DrSearch
{
    partial class DashboardForm
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            this.crawlDataBtn = new System.Windows.Forms.Button();
            this.runBenchBtn = new System.Windows.Forms.Button();
            this.progressInformationLbl = new System.Windows.Forms.Label();
            this.progInfoTB = new System.Windows.Forms.TextBox();
            this.searchInputTB = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.relDocsLbl = new System.Windows.Forms.Label();
            this.relDocsLB = new System.Windows.Forms.ListBox();
            this.searchMethodLB = new System.Windows.Forms.ListBox();
            this.loadIndexBtn = new System.Windows.Forms.Button();
            this.buildIndexTestBtn = new System.Windows.Forms.Button();
            this.loadIndexTestBtn = new System.Windows.Forms.Button();
            this.curIndexLB = new System.Windows.Forms.Label();
            this.searchModeCB = new System.Windows.Forms.ComboBox();
            this.searchModeL = new System.Windows.Forms.Label();
            this.buildIndexL = new System.Windows.Forms.Label();
            this.didYouMeanTB = new System.Windows.Forms.Label();
            this.resCountLB = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // crawlDataBtn
            // 
            this.crawlDataBtn.Location = new System.Drawing.Point(722, 462);
            this.crawlDataBtn.Name = "crawlDataBtn";
            this.crawlDataBtn.Size = new System.Drawing.Size(193, 23);
            this.crawlDataBtn.TabIndex = 3;
            this.crawlDataBtn.Text = "1) crawl data and build index";
            this.crawlDataBtn.UseVisualStyleBackColor = true;
            this.crawlDataBtn.Click += new System.EventHandler(this.crawlDataBtn_Click);
            // 
            // runBenchBtn
            // 
            this.runBenchBtn.Location = new System.Drawing.Point(722, 576);
            this.runBenchBtn.Name = "runBenchBtn";
            this.runBenchBtn.Size = new System.Drawing.Size(193, 23);
            this.runBenchBtn.TabIndex = 4;
            this.runBenchBtn.Text = "RUN BENCHMARK";
            this.runBenchBtn.UseVisualStyleBackColor = true;
            this.runBenchBtn.Click += new System.EventHandler(this.runBenchBtn_Click);
            // 
            // progressInformationLbl
            // 
            this.progressInformationLbl.AutoSize = true;
            this.progressInformationLbl.Location = new System.Drawing.Point(12, 384);
            this.progressInformationLbl.Name = "progressInformationLbl";
            this.progressInformationLbl.Size = new System.Drawing.Size(102, 13);
            this.progressInformationLbl.TabIndex = 5;
            this.progressInformationLbl.Text = "Progress information";
            // 
            // progInfoTB
            // 
            this.progInfoTB.Location = new System.Drawing.Point(12, 400);
            this.progInfoTB.Multiline = true;
            this.progInfoTB.Name = "progInfoTB";
            this.progInfoTB.ReadOnly = true;
            this.progInfoTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.progInfoTB.Size = new System.Drawing.Size(704, 189);
            this.progInfoTB.TabIndex = 6;
            // 
            // searchInputTB
            // 
            this.searchInputTB.Location = new System.Drawing.Point(12, 1);
            this.searchInputTB.Name = "searchInputTB";
            this.searchInputTB.Size = new System.Drawing.Size(704, 20);
            this.searchInputTB.TabIndex = 8;
            // 
            // searchBtn
            // 
            this.searchBtn.Location = new System.Drawing.Point(722, 1);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(193, 23);
            this.searchBtn.TabIndex = 9;
            this.searchBtn.Text = "search!";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // relDocsLbl
            // 
            this.relDocsLbl.AutoSize = true;
            this.relDocsLbl.Location = new System.Drawing.Point(12, 35);
            this.relDocsLbl.Name = "relDocsLbl";
            this.relDocsLbl.Size = new System.Drawing.Size(105, 13);
            this.relDocsLbl.TabIndex = 10;
            this.relDocsLbl.Text = "Relevant documents";
            // 
            // relDocsLB
            // 
            this.relDocsLB.FormattingEnabled = true;
            this.relDocsLB.Location = new System.Drawing.Point(12, 51);
            this.relDocsLB.Name = "relDocsLB";
            this.relDocsLB.Size = new System.Drawing.Size(903, 329);
            this.relDocsLB.TabIndex = 11;
            this.relDocsLB.SelectedIndexChanged += new System.EventHandler(this.relDocsLB_SelectedIndexChanged);
            // 
            // searchMethodLB
            // 
            this.searchMethodLB.FormattingEnabled = true;
            this.searchMethodLB.Location = new System.Drawing.Point(722, 400);
            this.searchMethodLB.Name = "searchMethodLB";
            this.searchMethodLB.Size = new System.Drawing.Size(193, 56);
            this.searchMethodLB.TabIndex = 12;
            this.searchMethodLB.SelectedIndexChanged += new System.EventHandler(this.searchMethodLB_SelectedIndexChanged);
            // 
            // loadIndexBtn
            // 
            this.loadIndexBtn.Location = new System.Drawing.Point(722, 520);
            this.loadIndexBtn.Name = "loadIndexBtn";
            this.loadIndexBtn.Size = new System.Drawing.Size(193, 23);
            this.loadIndexBtn.TabIndex = 14;
            this.loadIndexBtn.Text = "2) load crawled data and index";
            this.loadIndexBtn.UseVisualStyleBackColor = true;
            this.loadIndexBtn.Click += new System.EventHandler(this.loadIndexBtn_Click);
            // 
            // buildIndexTestBtn
            // 
            this.buildIndexTestBtn.Location = new System.Drawing.Point(722, 491);
            this.buildIndexTestBtn.Name = "buildIndexTestBtn";
            this.buildIndexTestBtn.Size = new System.Drawing.Size(193, 23);
            this.buildIndexTestBtn.TabIndex = 15;
            this.buildIndexTestBtn.Text = "1) TEST DATA - build index";
            this.buildIndexTestBtn.UseVisualStyleBackColor = true;
            this.buildIndexTestBtn.Click += new System.EventHandler(this.buildIndexTestBtn_Click);
            // 
            // loadIndexTestBtn
            // 
            this.loadIndexTestBtn.Location = new System.Drawing.Point(722, 549);
            this.loadIndexTestBtn.Name = "loadIndexTestBtn";
            this.loadIndexTestBtn.Size = new System.Drawing.Size(193, 23);
            this.loadIndexTestBtn.TabIndex = 16;
            this.loadIndexTestBtn.Text = "2) TEST DATA - load index";
            this.loadIndexTestBtn.UseVisualStyleBackColor = true;
            this.loadIndexTestBtn.Click += new System.EventHandler(this.loadIndexTestBtn_Click);
            // 
            // curIndexLB
            // 
            this.curIndexLB.AutoSize = true;
            this.curIndexLB.Location = new System.Drawing.Point(331, 30);
            this.curIndexLB.Name = "curIndexLB";
            this.curIndexLB.Size = new System.Drawing.Size(114, 13);
            this.curIndexLB.TabIndex = 17;
            this.curIndexLB.Text = "Currently loaded index:";
            // 
            // searchModeCB
            // 
            this.searchModeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.searchModeCB.Enabled = false;
            this.searchModeCB.FormattingEnabled = true;
            this.searchModeCB.Location = new System.Drawing.Point(722, 27);
            this.searchModeCB.Name = "searchModeCB";
            this.searchModeCB.Size = new System.Drawing.Size(193, 21);
            this.searchModeCB.TabIndex = 18;
            // 
            // searchModeL
            // 
            this.searchModeL.AutoSize = true;
            this.searchModeL.Location = new System.Drawing.Point(643, 30);
            this.searchModeL.Name = "searchModeL";
            this.searchModeL.Size = new System.Drawing.Size(73, 13);
            this.searchModeL.TabIndex = 19;
            this.searchModeL.Text = "Search mode:";
            // 
            // buildIndexL
            // 
            this.buildIndexL.AutoSize = true;
            this.buildIndexL.Location = new System.Drawing.Point(719, 384);
            this.buildIndexL.Name = "buildIndexL";
            this.buildIndexL.Size = new System.Drawing.Size(171, 13);
            this.buildIndexL.TabIndex = 20;
            this.buildIndexL.Text = "Select index mode (default vector):";
            // 
            // didYouMeanTB
            // 
            this.didYouMeanTB.AutoSize = true;
            this.didYouMeanTB.Location = new System.Drawing.Point(12, 22);
            this.didYouMeanTB.Name = "didYouMeanTB";
            this.didYouMeanTB.Size = new System.Drawing.Size(78, 13);
            this.didYouMeanTB.TabIndex = 21;
            this.didYouMeanTB.Text = "Did you mean: ";
            // 
            // resCountLB
            // 
            this.resCountLB.AutoSize = true;
            this.resCountLB.Location = new System.Drawing.Point(274, 383);
            this.resCountLB.Name = "resCountLB";
            this.resCountLB.Size = new System.Drawing.Size(73, 13);
            this.resCountLB.TabIndex = 22;
            this.resCountLB.Text = "Result count: ";
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 601);
            this.Controls.Add(this.resCountLB);
            this.Controls.Add(this.didYouMeanTB);
            this.Controls.Add(this.buildIndexL);
            this.Controls.Add(this.searchModeL);
            this.Controls.Add(this.searchModeCB);
            this.Controls.Add(this.curIndexLB);
            this.Controls.Add(this.loadIndexTestBtn);
            this.Controls.Add(this.buildIndexTestBtn);
            this.Controls.Add(this.loadIndexBtn);
            this.Controls.Add(this.searchMethodLB);
            this.Controls.Add(this.relDocsLB);
            this.Controls.Add(this.relDocsLbl);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.searchInputTB);
            this.Controls.Add(this.progInfoTB);
            this.Controls.Add(this.progressInformationLbl);
            this.Controls.Add(this.runBenchBtn);
            this.Controls.Add(this.crawlDataBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DrSearch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DashboardForm_FormClosing);
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button crawlDataBtn;
        private System.Windows.Forms.Button runBenchBtn;
        private System.Windows.Forms.Label progressInformationLbl;
        private System.Windows.Forms.TextBox progInfoTB;
        private System.Windows.Forms.TextBox searchInputTB;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.Label relDocsLbl;
        private System.Windows.Forms.ListBox relDocsLB;
        private System.Windows.Forms.ListBox searchMethodLB;
        private System.Windows.Forms.Button loadIndexBtn;
        private System.Windows.Forms.Button buildIndexTestBtn;
        private System.Windows.Forms.Button loadIndexTestBtn;
        private System.Windows.Forms.Label curIndexLB;
        private System.Windows.Forms.ComboBox searchModeCB;
        private System.Windows.Forms.Label searchModeL;
        private System.Windows.Forms.Label buildIndexL;
        private System.Windows.Forms.Label didYouMeanTB;
        private System.Windows.Forms.Label resCountLB;
    }
}

