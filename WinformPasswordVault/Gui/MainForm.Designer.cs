using ReaLTaiizor.Controls;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControlMain = new ReaLTaiizor.Controls.MaterialTabControl();
            this.tabPageSearch = new System.Windows.Forms.TabPage();
            this.materialTextBoxFilter = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            this.poisonGridVault = new ReaLTaiizor.Controls.PoisonDataGridView();
            this.poisonContextMenuStripGrid = new ReaLTaiizor.Controls.PoisonContextMenuStrip(this.components);
            this.supprimerLélémentToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageBrowserInport = new System.Windows.Forms.TabPage();
            this.materialCheckListBoxBrowserData = new ReaLTaiizor.Controls.MaterialCheckListBox();
            this.materialButtonImportBrowserData = new ReaLTaiizor.Controls.MaterialButton();
            this.materialButtonShowBrowserDatas = new ReaLTaiizor.Controls.MaterialButton();
            this.tabPageManualImport = new System.Windows.Forms.TabPage();
            this.groupBoxMyPassword = new System.Windows.Forms.GroupBox();
            this.textBoxMdpKeyword = new System.Windows.Forms.TextBox();
            this.labelMdpKeyword = new System.Windows.Forms.Label();
            this.textBoxMdpData = new System.Windows.Forms.TextBox();
            this.labelMdpData = new System.Windows.Forms.Label();
            this.textBoxMdpPassword = new System.Windows.Forms.TextBox();
            this.labelMdpPassword = new System.Windows.Forms.Label();
            this.textBoxMdpLogin = new System.Windows.Forms.TextBox();
            this.labelMdpLogin = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.tabPageSaveManagement = new System.Windows.Forms.TabPage();
            this.materialButtonSave = new ReaLTaiizor.Controls.MaterialButton();
            this.tabPageAbout = new System.Windows.Forms.TabPage();
            this.webViewGitHubProfile = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.imageListTabCtrl = new System.Windows.Forms.ImageList(this.components);
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.myVaultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControlMain.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.poisonGridVault)).BeginInit();
            this.poisonContextMenuStripGrid.SuspendLayout();
            this.tabPageBrowserInport.SuspendLayout();
            this.tabPageManualImport.SuspendLayout();
            this.groupBoxMyPassword.SuspendLayout();
            this.tabPageSaveManagement.SuspendLayout();
            this.tabPageAbout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewGitHubProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myVaultBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            resources.ApplyResources(this.tabControlMain, "tabControlMain");
            this.tabControlMain.Controls.Add(this.tabPageSearch);
            this.tabControlMain.Controls.Add(this.tabPageBrowserInport);
            this.tabControlMain.Controls.Add(this.tabPageManualImport);
            this.tabControlMain.Controls.Add(this.tabPageSaveManagement);
            this.tabControlMain.Controls.Add(this.tabPageAbout);
            this.tabControlMain.Depth = 0;
            this.tabControlMain.ImageList = this.imageListTabCtrl;
            this.tabControlMain.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.tabControlMain.Multiline = true;
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabPageSearch.Controls.Add(this.materialTextBoxFilter);
            this.tabPageSearch.Controls.Add(this.poisonGridVault);
            resources.ApplyResources(this.tabPageSearch, "tabPageSearch");
            this.tabPageSearch.Name = "tabPageSearch";
            // 
            // materialTextBoxFilter
            // 
            this.materialTextBoxFilter.AnimateReadOnly = false;
            this.materialTextBoxFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.materialTextBoxFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            resources.ApplyResources(this.materialTextBoxFilter, "materialTextBoxFilter");
            this.materialTextBoxFilter.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxFilter.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialTextBoxFilter.Depth = 0;
            this.materialTextBoxFilter.HideSelection = true;
            this.materialTextBoxFilter.LeadingIcon = null;
            this.materialTextBoxFilter.MaxLength = 50;
            this.materialTextBoxFilter.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.materialTextBoxFilter.Name = "materialTextBoxFilter";
            this.materialTextBoxFilter.PasswordChar = '\0';
            this.materialTextBoxFilter.ReadOnly = false;
            this.materialTextBoxFilter.SelectedText = "";
            this.materialTextBoxFilter.SelectionLength = 0;
            this.materialTextBoxFilter.SelectionStart = 0;
            this.materialTextBoxFilter.ShortcutsEnabled = true;
            this.materialTextBoxFilter.TabStop = false;
            this.materialTextBoxFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxFilter.TrailingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxFilter.TrailingIcon")));
            this.materialTextBoxFilter.UseSystemPasswordChar = false;
            this.materialTextBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // poisonGridVault
            // 
            this.poisonGridVault.AllowUserToAddRows = false;
            this.poisonGridVault.AllowUserToDeleteRows = false;
            this.poisonGridVault.AllowUserToResizeRows = false;
            resources.ApplyResources(this.poisonGridVault, "poisonGridVault");
            this.poisonGridVault.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.poisonGridVault.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.poisonGridVault.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.poisonGridVault.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(101)))), ((int)(((byte)(190)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.poisonGridVault.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.poisonGridVault.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.poisonGridVault.ContextMenuStrip = this.poisonContextMenuStripGrid;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(101)))), ((int)(((byte)(190)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.poisonGridVault.DefaultCellStyle = dataGridViewCellStyle2;
            this.poisonGridVault.EnableHeadersVisualStyles = false;
            this.poisonGridVault.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.poisonGridVault.MultiSelect = false;
            this.poisonGridVault.Name = "poisonGridVault";
            this.poisonGridVault.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(65)))), ((int)(((byte)(153)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(101)))), ((int)(((byte)(190)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.poisonGridVault.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.poisonGridVault.RowHeadersVisible = false;
            this.poisonGridVault.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.poisonGridVault.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.poisonGridVault.ShowCellErrors = false;
            this.poisonGridVault.ShowCellToolTips = false;
            this.poisonGridVault.ShowEditingIcon = false;
            this.poisonGridVault.ShowRowErrors = false;
            // 
            // poisonContextMenuStripGrid
            // 
            this.poisonContextMenuStripGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.supprimerLélémentToolStripMenuItem1});
            this.poisonContextMenuStripGrid.Name = "poisonContextMenuStripGrid";
            resources.ApplyResources(this.poisonContextMenuStripGrid, "poisonContextMenuStripGrid");
            // 
            // supprimerLélémentToolStripMenuItem1
            // 
            this.supprimerLélémentToolStripMenuItem1.Name = "supprimerLélémentToolStripMenuItem1";
            resources.ApplyResources(this.supprimerLélémentToolStripMenuItem1, "supprimerLélémentToolStripMenuItem1");
            this.supprimerLélémentToolStripMenuItem1.Click += new System.EventHandler(this.deleteRowToolStripMenuItem_Click);
            // 
            // tabPageBrowserInport
            // 
            this.tabPageBrowserInport.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageBrowserInport.Controls.Add(this.materialCheckListBoxBrowserData);
            this.tabPageBrowserInport.Controls.Add(this.materialButtonImportBrowserData);
            this.tabPageBrowserInport.Controls.Add(this.materialButtonShowBrowserDatas);
            resources.ApplyResources(this.tabPageBrowserInport, "tabPageBrowserInport");
            this.tabPageBrowserInport.Name = "tabPageBrowserInport";
            // 
            // materialCheckListBoxBrowserData
            // 
            resources.ApplyResources(this.materialCheckListBoxBrowserData, "materialCheckListBoxBrowserData");
            this.materialCheckListBoxBrowserData.BackColor = System.Drawing.SystemColors.Control;
            this.materialCheckListBoxBrowserData.Depth = 0;
            this.materialCheckListBoxBrowserData.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialCheckListBoxBrowserData.Name = "materialCheckListBoxBrowserData";
            this.materialCheckListBoxBrowserData.Striped = false;
            this.materialCheckListBoxBrowserData.StripeDarkColor = System.Drawing.Color.Empty;
            // 
            // materialButtonImportBrowserData
            // 
            resources.ApplyResources(this.materialButtonImportBrowserData, "materialButtonImportBrowserData");
            this.materialButtonImportBrowserData.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButtonImportBrowserData.Depth = 0;
            this.materialButtonImportBrowserData.HighEmphasis = true;
            this.materialButtonImportBrowserData.Icon = ((System.Drawing.Image)(resources.GetObject("materialButtonImportBrowserData.Icon")));
            this.materialButtonImportBrowserData.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.materialButtonImportBrowserData.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialButtonImportBrowserData.Name = "materialButtonImportBrowserData";
            this.materialButtonImportBrowserData.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButtonImportBrowserData.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButtonImportBrowserData.UseAccentColor = true;
            this.materialButtonImportBrowserData.UseVisualStyleBackColor = true;
            this.materialButtonImportBrowserData.Click += new System.EventHandler(this.buttonImportData_Click);
            // 
            // materialButtonShowBrowserDatas
            // 
            resources.ApplyResources(this.materialButtonShowBrowserDatas, "materialButtonShowBrowserDatas");
            this.materialButtonShowBrowserDatas.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButtonShowBrowserDatas.Depth = 0;
            this.materialButtonShowBrowserDatas.HighEmphasis = true;
            this.materialButtonShowBrowserDatas.Icon = ((System.Drawing.Image)(resources.GetObject("materialButtonShowBrowserDatas.Icon")));
            this.materialButtonShowBrowserDatas.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.materialButtonShowBrowserDatas.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialButtonShowBrowserDatas.Name = "materialButtonShowBrowserDatas";
            this.materialButtonShowBrowserDatas.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButtonShowBrowserDatas.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButtonShowBrowserDatas.UseAccentColor = false;
            this.materialButtonShowBrowserDatas.UseVisualStyleBackColor = true;
            this.materialButtonShowBrowserDatas.Click += new System.EventHandler(this.buttonShowImportData_Click);
            // 
            // tabPageManualImport
            // 
            this.tabPageManualImport.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabPageManualImport.Controls.Add(this.groupBoxMyPassword);
            this.tabPageManualImport.Controls.Add(this.buttonAdd);
            resources.ApplyResources(this.tabPageManualImport, "tabPageManualImport");
            this.tabPageManualImport.Name = "tabPageManualImport";
            // 
            // groupBoxMyPassword
            // 
            resources.ApplyResources(this.groupBoxMyPassword, "groupBoxMyPassword");
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
            // tabPageSaveManagement
            // 
            this.tabPageSaveManagement.Controls.Add(this.materialButtonSave);
            resources.ApplyResources(this.tabPageSaveManagement, "tabPageSaveManagement");
            this.tabPageSaveManagement.Name = "tabPageSaveManagement";
            this.tabPageSaveManagement.UseVisualStyleBackColor = true;
            // 
            // materialButtonSave
            // 
            resources.ApplyResources(this.materialButtonSave, "materialButtonSave");
            this.materialButtonSave.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButtonSave.Depth = 0;
            this.materialButtonSave.HighEmphasis = true;
            this.materialButtonSave.Icon = ((System.Drawing.Image)(resources.GetObject("materialButtonSave.Icon")));
            this.materialButtonSave.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.materialButtonSave.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialButtonSave.Name = "materialButtonSave";
            this.materialButtonSave.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButtonSave.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButtonSave.UseAccentColor = true;
            this.materialButtonSave.UseVisualStyleBackColor = true;
            this.materialButtonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // tabPageAbout
            // 
            this.tabPageAbout.Controls.Add(this.webViewGitHubProfile);
            resources.ApplyResources(this.tabPageAbout, "tabPageAbout");
            this.tabPageAbout.Name = "tabPageAbout";
            this.tabPageAbout.UseVisualStyleBackColor = true;
            // 
            // webViewGitHubProfile
            // 
            this.webViewGitHubProfile.AllowExternalDrop = false;
            this.webViewGitHubProfile.CreationProperties = null;
            this.webViewGitHubProfile.DefaultBackgroundColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.webViewGitHubProfile, "webViewGitHubProfile");
            this.webViewGitHubProfile.Name = "webViewGitHubProfile";
            this.webViewGitHubProfile.Source = new System.Uri("https://github.com/DevElkami", System.UriKind.Absolute);
            this.webViewGitHubProfile.ZoomFactor = 1D;
            // 
            // imageListTabCtrl
            // 
            this.imageListTabCtrl.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListTabCtrl.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabCtrl.ImageStream")));
            this.imageListTabCtrl.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabCtrl.Images.SetKeyName(0, "search-mini.png");
            this.imageListTabCtrl.Images.SetKeyName(1, "GitHub-Mark.png");
            this.imageListTabCtrl.Images.SetKeyName(2, "baseline_fingerprint_black_24dp.png");
            this.imageListTabCtrl.Images.SetKeyName(3, "diskette-mini.png");
            this.imageListTabCtrl.Images.SetKeyName(4, "round_add_black_24dp.png");
            this.imageListTabCtrl.Images.SetKeyName(5, "import.png");
            // 
            // pictureBoxLogo
            // 
            resources.ApplyResources(this.pictureBoxLogo, "pictureBoxLogo");
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.TabStop = false;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.tabControlMain);
            this.DrawerAutoShow = true;
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.tabControlMain;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.poisonGridVault)).EndInit();
            this.poisonContextMenuStripGrid.ResumeLayout(false);
            this.tabPageBrowserInport.ResumeLayout(false);
            this.tabPageManualImport.ResumeLayout(false);
            this.groupBoxMyPassword.ResumeLayout(false);
            this.groupBoxMyPassword.PerformLayout();
            this.tabPageSaveManagement.ResumeLayout(false);
            this.tabPageAbout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.webViewGitHubProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myVaultBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialTabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageSearch;
        private System.Windows.Forms.TabPage tabPageManualImport;
        private System.Windows.Forms.TabPage tabPageBrowserInport;
        private System.Windows.Forms.BindingSource myVaultBindingSource;
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
        private System.Windows.Forms.ImageList imageListTabCtrl;
        private PoisonDataGridView poisonGridVault;
        private MaterialTextBoxEdit materialTextBoxFilter;
        private MaterialButton materialButtonSave;
        private PoisonContextMenuStrip poisonContextMenuStripGrid;
        private System.Windows.Forms.ToolStripMenuItem supprimerLélémentToolStripMenuItem1;
        private System.Windows.Forms.TabPage tabPageSaveManagement;
        private System.Windows.Forms.TabPage tabPageAbout;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewGitHubProfile;
        private MaterialButton materialButtonImportBrowserData;
        private MaterialButton materialButtonShowBrowserDatas;
        private MaterialCheckListBox materialCheckListBoxBrowserData;
    }
}

