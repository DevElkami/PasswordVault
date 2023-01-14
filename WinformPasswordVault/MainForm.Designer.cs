namespace WinformPasswordVault
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageSearch = new System.Windows.Forms.TabPage();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.dataGridViewVault = new System.Windows.Forms.DataGridView();
            this.contextMenuStripRow = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.supprimerLélémentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageFirefox = new System.Windows.Forms.TabPage();
            this.checkedListBoxMdpFireFox = new System.Windows.Forms.CheckedListBox();
            this.buttonViewFirefox = new System.Windows.Forms.Button();
            this.labelViewFirefox = new System.Windows.Forms.Label();
            this.buttonImportFromFirefox = new System.Windows.Forms.Button();
            this.labelImportFromFirefox = new System.Windows.Forms.Label();
            this.tabPageOption = new System.Windows.Forms.TabPage();
            this.groupBoxMyPassword = new System.Windows.Forms.GroupBox();
            this.groupBoxTranslateMsg = new System.Windows.Forms.GroupBox();
            this.labelMsgAddDoneTitle = new System.Windows.Forms.Label();
            this.labelMsgAddDone = new System.Windows.Forms.Label();
            this.labelMsgImportTitle = new System.Windows.Forms.Label();
            this.labelMsgImport = new System.Windows.Forms.Label();
            this.labelGridKeyword = new System.Windows.Forms.Label();
            this.labelGridData = new System.Windows.Forms.Label();
            this.labelGridPassword = new System.Windows.Forms.Label();
            this.labelGridLogin = new System.Windows.Forms.Label();
            this.labelMsgPasswordTitle = new System.Windows.Forms.Label();
            this.labelMsgPassword = new System.Windows.Forms.Label();
            this.textBoxMdpKeyword = new System.Windows.Forms.TextBox();
            this.labelMdpKeyword = new System.Windows.Forms.Label();
            this.textBoxMdpData = new System.Windows.Forms.TextBox();
            this.labelMdpData = new System.Windows.Forms.Label();
            this.textBoxMdpPassword = new System.Windows.Forms.TextBox();
            this.labelMdpPassword = new System.Windows.Forms.Label();
            this.textBoxMdpLogin = new System.Windows.Forms.TextBox();
            this.labelMdpLogin = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.myVaultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControlMain.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVault)).BeginInit();
            this.contextMenuStripRow.SuspendLayout();
            this.tabPageFirefox.SuspendLayout();
            this.tabPageOption.SuspendLayout();
            this.groupBoxMyPassword.SuspendLayout();
            this.groupBoxTranslateMsg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myVaultBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            resources.ApplyResources(this.tabControlMain, "tabControlMain");
            this.tabControlMain.Controls.Add(this.tabPageSearch);
            this.tabControlMain.Controls.Add(this.tabPageFirefox);
            this.tabControlMain.Controls.Add(this.tabPageOption);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            // 
            // tabPageSearch
            // 
            resources.ApplyResources(this.tabPageSearch, "tabPageSearch");
            this.tabPageSearch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabPageSearch.Controls.Add(this.buttonSave);
            this.tabPageSearch.Controls.Add(this.textBoxFilter);
            this.tabPageSearch.Controls.Add(this.dataGridViewVault);
            this.tabPageSearch.Name = "tabPageSearch";
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonSave.BackgroundImage = global::WinformPasswordVault.Properties.Resources.save;
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxFilter
            // 
            resources.ApplyResources(this.textBoxFilter, "textBoxFilter");
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // dataGridViewVault
            // 
            resources.ApplyResources(this.dataGridViewVault, "dataGridViewVault");
            this.dataGridViewVault.AllowUserToAddRows = false;
            this.dataGridViewVault.AllowUserToDeleteRows = false;
            this.dataGridViewVault.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVault.ContextMenuStrip = this.contextMenuStripRow;
            this.dataGridViewVault.Name = "dataGridViewVault";
            this.dataGridViewVault.ShowCellErrors = false;
            this.dataGridViewVault.ShowCellToolTips = false;
            this.dataGridViewVault.ShowEditingIcon = false;
            this.dataGridViewVault.ShowRowErrors = false;
            // 
            // contextMenuStripRow
            // 
            resources.ApplyResources(this.contextMenuStripRow, "contextMenuStripRow");
            this.contextMenuStripRow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.supprimerLélémentToolStripMenuItem});
            this.contextMenuStripRow.Name = "contextMenuStrip1";
            // 
            // supprimerLélémentToolStripMenuItem
            // 
            resources.ApplyResources(this.supprimerLélémentToolStripMenuItem, "supprimerLélémentToolStripMenuItem");
            this.supprimerLélémentToolStripMenuItem.Name = "supprimerLélémentToolStripMenuItem";
            this.supprimerLélémentToolStripMenuItem.Click += new System.EventHandler(this.deleteRowToolStripMenuItem_Click);
            // 
            // tabPageFirefox
            // 
            resources.ApplyResources(this.tabPageFirefox, "tabPageFirefox");
            this.tabPageFirefox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabPageFirefox.Controls.Add(this.checkedListBoxMdpFireFox);
            this.tabPageFirefox.Controls.Add(this.buttonViewFirefox);
            this.tabPageFirefox.Controls.Add(this.labelViewFirefox);
            this.tabPageFirefox.Controls.Add(this.buttonImportFromFirefox);
            this.tabPageFirefox.Controls.Add(this.labelImportFromFirefox);
            this.tabPageFirefox.Name = "tabPageFirefox";
            // 
            // checkedListBoxMdpFireFox
            // 
            resources.ApplyResources(this.checkedListBoxMdpFireFox, "checkedListBoxMdpFireFox");
            this.checkedListBoxMdpFireFox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.checkedListBoxMdpFireFox.FormattingEnabled = true;
            this.checkedListBoxMdpFireFox.Name = "checkedListBoxMdpFireFox";
            // 
            // buttonViewFirefox
            // 
            resources.ApplyResources(this.buttonViewFirefox, "buttonViewFirefox");
            this.buttonViewFirefox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonViewFirefox.Name = "buttonViewFirefox";
            this.buttonViewFirefox.UseVisualStyleBackColor = false;
            this.buttonViewFirefox.Click += new System.EventHandler(this.buttonShowImportData_Click);
            // 
            // labelViewFirefox
            // 
            resources.ApplyResources(this.labelViewFirefox, "labelViewFirefox");
            this.labelViewFirefox.Name = "labelViewFirefox";
            // 
            // buttonImportFromFirefox
            // 
            resources.ApplyResources(this.buttonImportFromFirefox, "buttonImportFromFirefox");
            this.buttonImportFromFirefox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonImportFromFirefox.Name = "buttonImportFromFirefox";
            this.buttonImportFromFirefox.UseVisualStyleBackColor = false;
            this.buttonImportFromFirefox.Click += new System.EventHandler(this.buttonImportData_Click);
            // 
            // labelImportFromFirefox
            // 
            resources.ApplyResources(this.labelImportFromFirefox, "labelImportFromFirefox");
            this.labelImportFromFirefox.Name = "labelImportFromFirefox";
            // 
            // tabPageOption
            // 
            resources.ApplyResources(this.tabPageOption, "tabPageOption");
            this.tabPageOption.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabPageOption.Controls.Add(this.groupBoxMyPassword);
            this.tabPageOption.Controls.Add(this.buttonAdd);
            this.tabPageOption.Name = "tabPageOption";
            // 
            // groupBoxMyPassword
            // 
            resources.ApplyResources(this.groupBoxMyPassword, "groupBoxMyPassword");
            this.groupBoxMyPassword.Controls.Add(this.groupBoxTranslateMsg);
            this.groupBoxMyPassword.Controls.Add(this.textBoxMdpKeyword);
            this.groupBoxMyPassword.Controls.Add(this.labelMdpKeyword);
            this.groupBoxMyPassword.Controls.Add(this.textBoxMdpData);
            this.groupBoxMyPassword.Controls.Add(this.labelMdpData);
            this.groupBoxMyPassword.Controls.Add(this.textBoxMdpPassword);
            this.groupBoxMyPassword.Controls.Add(this.labelMdpPassword);
            this.groupBoxMyPassword.Controls.Add(this.textBoxMdpLogin);
            this.groupBoxMyPassword.Controls.Add(this.labelMdpLogin);
            this.groupBoxMyPassword.Name = "groupBoxMyPassword";
            this.groupBoxMyPassword.TabStop = false;
            // 
            // groupBoxTranslateMsg
            // 
            resources.ApplyResources(this.groupBoxTranslateMsg, "groupBoxTranslateMsg");
            this.groupBoxTranslateMsg.Controls.Add(this.labelMsgAddDoneTitle);
            this.groupBoxTranslateMsg.Controls.Add(this.labelMsgAddDone);
            this.groupBoxTranslateMsg.Controls.Add(this.labelMsgImportTitle);
            this.groupBoxTranslateMsg.Controls.Add(this.labelMsgImport);
            this.groupBoxTranslateMsg.Controls.Add(this.labelGridKeyword);
            this.groupBoxTranslateMsg.Controls.Add(this.labelGridData);
            this.groupBoxTranslateMsg.Controls.Add(this.labelGridPassword);
            this.groupBoxTranslateMsg.Controls.Add(this.labelGridLogin);
            this.groupBoxTranslateMsg.Controls.Add(this.labelMsgPasswordTitle);
            this.groupBoxTranslateMsg.Controls.Add(this.labelMsgPassword);
            this.groupBoxTranslateMsg.Name = "groupBoxTranslateMsg";
            this.groupBoxTranslateMsg.TabStop = false;
            // 
            // labelMsgAddDoneTitle
            // 
            resources.ApplyResources(this.labelMsgAddDoneTitle, "labelMsgAddDoneTitle");
            this.labelMsgAddDoneTitle.Name = "labelMsgAddDoneTitle";
            // 
            // labelMsgAddDone
            // 
            resources.ApplyResources(this.labelMsgAddDone, "labelMsgAddDone");
            this.labelMsgAddDone.Name = "labelMsgAddDone";
            // 
            // labelMsgImportTitle
            // 
            resources.ApplyResources(this.labelMsgImportTitle, "labelMsgImportTitle");
            this.labelMsgImportTitle.Name = "labelMsgImportTitle";
            // 
            // labelMsgImport
            // 
            resources.ApplyResources(this.labelMsgImport, "labelMsgImport");
            this.labelMsgImport.Name = "labelMsgImport";
            // 
            // labelGridKeyword
            // 
            resources.ApplyResources(this.labelGridKeyword, "labelGridKeyword");
            this.labelGridKeyword.Name = "labelGridKeyword";
            // 
            // labelGridData
            // 
            resources.ApplyResources(this.labelGridData, "labelGridData");
            this.labelGridData.Name = "labelGridData";
            // 
            // labelGridPassword
            // 
            resources.ApplyResources(this.labelGridPassword, "labelGridPassword");
            this.labelGridPassword.Name = "labelGridPassword";
            // 
            // labelGridLogin
            // 
            resources.ApplyResources(this.labelGridLogin, "labelGridLogin");
            this.labelGridLogin.Name = "labelGridLogin";
            // 
            // labelMsgPasswordTitle
            // 
            resources.ApplyResources(this.labelMsgPasswordTitle, "labelMsgPasswordTitle");
            this.labelMsgPasswordTitle.Name = "labelMsgPasswordTitle";
            // 
            // labelMsgPassword
            // 
            resources.ApplyResources(this.labelMsgPassword, "labelMsgPassword");
            this.labelMsgPassword.Name = "labelMsgPassword";
            // 
            // textBoxMdpKeyword
            // 
            resources.ApplyResources(this.textBoxMdpKeyword, "textBoxMdpKeyword");
            this.textBoxMdpKeyword.Name = "textBoxMdpKeyword";
            // 
            // labelMdpKeyword
            // 
            resources.ApplyResources(this.labelMdpKeyword, "labelMdpKeyword");
            this.labelMdpKeyword.Name = "labelMdpKeyword";
            // 
            // textBoxMdpData
            // 
            resources.ApplyResources(this.textBoxMdpData, "textBoxMdpData");
            this.textBoxMdpData.Name = "textBoxMdpData";
            // 
            // labelMdpData
            // 
            resources.ApplyResources(this.labelMdpData, "labelMdpData");
            this.labelMdpData.Name = "labelMdpData";
            // 
            // textBoxMdpPassword
            // 
            resources.ApplyResources(this.textBoxMdpPassword, "textBoxMdpPassword");
            this.textBoxMdpPassword.Name = "textBoxMdpPassword";
            // 
            // labelMdpPassword
            // 
            resources.ApplyResources(this.labelMdpPassword, "labelMdpPassword");
            this.labelMdpPassword.Name = "labelMdpPassword";
            // 
            // textBoxMdpLogin
            // 
            resources.ApplyResources(this.textBoxMdpLogin, "textBoxMdpLogin");
            this.textBoxMdpLogin.Name = "textBoxMdpLogin";
            // 
            // labelMdpLogin
            // 
            resources.ApplyResources(this.labelMdpLogin, "labelMdpLogin");
            this.labelMdpLogin.Name = "labelMdpLogin";
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.tabControlMain);
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageSearch.ResumeLayout(false);
            this.tabPageSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVault)).EndInit();
            this.contextMenuStripRow.ResumeLayout(false);
            this.tabPageFirefox.ResumeLayout(false);
            this.tabPageFirefox.PerformLayout();
            this.tabPageOption.ResumeLayout(false);
            this.groupBoxMyPassword.ResumeLayout(false);
            this.groupBoxMyPassword.PerformLayout();
            this.groupBoxTranslateMsg.ResumeLayout(false);
            this.groupBoxTranslateMsg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myVaultBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageSearch;
        private System.Windows.Forms.TabPage tabPageOption;
        private System.Windows.Forms.TabPage tabPageFirefox;
        private System.Windows.Forms.Button buttonViewFirefox;
        private System.Windows.Forms.Label labelViewFirefox;
        private System.Windows.Forms.Button buttonImportFromFirefox;
        private System.Windows.Forms.Label labelImportFromFirefox;
        private System.Windows.Forms.CheckedListBox checkedListBoxMdpFireFox;
        private System.Windows.Forms.DataGridView dataGridViewVault;
        private System.Windows.Forms.BindingSource myVaultBindingSource;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRow;
        private System.Windows.Forms.ToolStripMenuItem supprimerLélémentToolStripMenuItem;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.GroupBox groupBoxMyPassword;
        private System.Windows.Forms.TextBox textBoxMdpKeyword;
        private System.Windows.Forms.Label labelMdpKeyword;
        private System.Windows.Forms.TextBox textBoxMdpData;
        private System.Windows.Forms.Label labelMdpData;
        private System.Windows.Forms.TextBox textBoxMdpPassword;
        private System.Windows.Forms.Label labelMdpPassword;
        private System.Windows.Forms.TextBox textBoxMdpLogin;
        private System.Windows.Forms.Label labelMdpLogin;
        private System.Windows.Forms.GroupBox groupBoxTranslateMsg;
        private System.Windows.Forms.Label labelMsgPasswordTitle;
        private System.Windows.Forms.Label labelMsgPassword;
        private System.Windows.Forms.Label labelGridKeyword;
        private System.Windows.Forms.Label labelGridData;
        private System.Windows.Forms.Label labelGridPassword;
        private System.Windows.Forms.Label labelGridLogin;
        private System.Windows.Forms.Label labelMsgImportTitle;
        private System.Windows.Forms.Label labelMsgImport;
        private System.Windows.Forms.Label labelMsgAddDoneTitle;
        private System.Windows.Forms.Label labelMsgAddDone;
    }
}

