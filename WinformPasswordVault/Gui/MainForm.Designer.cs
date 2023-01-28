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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.materialTextBoxEditKeyword = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            this.materialTextBoxEditPassword = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            this.materialTextBoxEditUrl = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            this.materialTextBoxEditLogin = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            this.materialButtonPasswordAdd = new ReaLTaiizor.Controls.MaterialButton();
            this.tabPageAbout = new System.Windows.Forms.TabPage();
            this.webViewGitHubProfile = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.imageListTabCtrl = new System.Windows.Forms.ImageList(this.components);
            this.materialButtonSave = new ReaLTaiizor.Controls.MaterialButton();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.myVaultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControlMain.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.poisonGridVault)).BeginInit();
            this.poisonContextMenuStripGrid.SuspendLayout();
            this.tabPageBrowserInport.SuspendLayout();
            this.tabPageManualImport.SuspendLayout();
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
            this.tabPageSearch.BackColor = System.Drawing.SystemColors.Control;
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
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(101)))), ((int)(((byte)(190)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.poisonGridVault.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.poisonGridVault.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.poisonGridVault.ContextMenuStrip = this.poisonContextMenuStripGrid;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(101)))), ((int)(((byte)(190)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.poisonGridVault.DefaultCellStyle = dataGridViewCellStyle5;
            this.poisonGridVault.EnableHeadersVisualStyles = false;
            this.poisonGridVault.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.poisonGridVault.MultiSelect = false;
            this.poisonGridVault.Name = "poisonGridVault";
            this.poisonGridVault.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(65)))), ((int)(((byte)(153)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(101)))), ((int)(((byte)(190)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.poisonGridVault.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
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
            resources.ApplyResources(this.supprimerLélémentToolStripMenuItem1, "supprimerLélémentToolStripMenuItem1");
            this.supprimerLélémentToolStripMenuItem1.Name = "supprimerLélémentToolStripMenuItem1";
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
            this.tabPageManualImport.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageManualImport.Controls.Add(this.materialTextBoxEditKeyword);
            this.tabPageManualImport.Controls.Add(this.materialTextBoxEditPassword);
            this.tabPageManualImport.Controls.Add(this.materialTextBoxEditUrl);
            this.tabPageManualImport.Controls.Add(this.materialTextBoxEditLogin);
            this.tabPageManualImport.Controls.Add(this.materialButtonPasswordAdd);
            resources.ApplyResources(this.tabPageManualImport, "tabPageManualImport");
            this.tabPageManualImport.Name = "tabPageManualImport";
            // 
            // materialTextBoxEditKeyword
            // 
            this.materialTextBoxEditKeyword.AnimateReadOnly = false;
            this.materialTextBoxEditKeyword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.materialTextBoxEditKeyword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            resources.ApplyResources(this.materialTextBoxEditKeyword, "materialTextBoxEditKeyword");
            this.materialTextBoxEditKeyword.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxEditKeyword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialTextBoxEditKeyword.Depth = 0;
            this.materialTextBoxEditKeyword.HideSelection = true;
            this.materialTextBoxEditKeyword.LeadingIcon = null;
            this.materialTextBoxEditKeyword.MaxLength = 50;
            this.materialTextBoxEditKeyword.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.materialTextBoxEditKeyword.Name = "materialTextBoxEditKeyword";
            this.materialTextBoxEditKeyword.PasswordChar = '\0';
            this.materialTextBoxEditKeyword.ReadOnly = false;
            this.materialTextBoxEditKeyword.SelectedText = "";
            this.materialTextBoxEditKeyword.SelectionLength = 0;
            this.materialTextBoxEditKeyword.SelectionStart = 0;
            this.materialTextBoxEditKeyword.ShortcutsEnabled = true;
            this.materialTextBoxEditKeyword.TabStop = false;
            this.materialTextBoxEditKeyword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxEditKeyword.TrailingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxEditKeyword.TrailingIcon")));
            this.materialTextBoxEditKeyword.UseSystemPasswordChar = false;
            // 
            // materialTextBoxEditPassword
            // 
            this.materialTextBoxEditPassword.AnimateReadOnly = false;
            this.materialTextBoxEditPassword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.materialTextBoxEditPassword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            resources.ApplyResources(this.materialTextBoxEditPassword, "materialTextBoxEditPassword");
            this.materialTextBoxEditPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxEditPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialTextBoxEditPassword.Depth = 0;
            this.materialTextBoxEditPassword.HideSelection = true;
            this.materialTextBoxEditPassword.LeadingIcon = null;
            this.materialTextBoxEditPassword.MaxLength = 50;
            this.materialTextBoxEditPassword.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.materialTextBoxEditPassword.Name = "materialTextBoxEditPassword";
            this.materialTextBoxEditPassword.PasswordChar = '\0';
            this.materialTextBoxEditPassword.ReadOnly = false;
            this.materialTextBoxEditPassword.SelectedText = "";
            this.materialTextBoxEditPassword.SelectionLength = 0;
            this.materialTextBoxEditPassword.SelectionStart = 0;
            this.materialTextBoxEditPassword.ShortcutsEnabled = true;
            this.materialTextBoxEditPassword.TabStop = false;
            this.materialTextBoxEditPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxEditPassword.TrailingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxEditPassword.TrailingIcon")));
            this.materialTextBoxEditPassword.UseSystemPasswordChar = false;
            // 
            // materialTextBoxEditUrl
            // 
            this.materialTextBoxEditUrl.AnimateReadOnly = false;
            this.materialTextBoxEditUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.materialTextBoxEditUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            resources.ApplyResources(this.materialTextBoxEditUrl, "materialTextBoxEditUrl");
            this.materialTextBoxEditUrl.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxEditUrl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialTextBoxEditUrl.Depth = 0;
            this.materialTextBoxEditUrl.HideSelection = true;
            this.materialTextBoxEditUrl.LeadingIcon = null;
            this.materialTextBoxEditUrl.MaxLength = 50;
            this.materialTextBoxEditUrl.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.materialTextBoxEditUrl.Name = "materialTextBoxEditUrl";
            this.materialTextBoxEditUrl.PasswordChar = '\0';
            this.materialTextBoxEditUrl.ReadOnly = false;
            this.materialTextBoxEditUrl.SelectedText = "";
            this.materialTextBoxEditUrl.SelectionLength = 0;
            this.materialTextBoxEditUrl.SelectionStart = 0;
            this.materialTextBoxEditUrl.ShortcutsEnabled = true;
            this.materialTextBoxEditUrl.TabStop = false;
            this.materialTextBoxEditUrl.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxEditUrl.TrailingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxEditUrl.TrailingIcon")));
            this.materialTextBoxEditUrl.UseSystemPasswordChar = false;
            // 
            // materialTextBoxEditLogin
            // 
            this.materialTextBoxEditLogin.AnimateReadOnly = false;
            this.materialTextBoxEditLogin.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.materialTextBoxEditLogin.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            resources.ApplyResources(this.materialTextBoxEditLogin, "materialTextBoxEditLogin");
            this.materialTextBoxEditLogin.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxEditLogin.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialTextBoxEditLogin.Depth = 0;
            this.materialTextBoxEditLogin.HideSelection = true;
            this.materialTextBoxEditLogin.LeadingIcon = null;
            this.materialTextBoxEditLogin.MaxLength = 50;
            this.materialTextBoxEditLogin.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.materialTextBoxEditLogin.Name = "materialTextBoxEditLogin";
            this.materialTextBoxEditLogin.PasswordChar = '\0';
            this.materialTextBoxEditLogin.ReadOnly = false;
            this.materialTextBoxEditLogin.SelectedText = "";
            this.materialTextBoxEditLogin.SelectionLength = 0;
            this.materialTextBoxEditLogin.SelectionStart = 0;
            this.materialTextBoxEditLogin.ShortcutsEnabled = true;
            this.materialTextBoxEditLogin.TabStop = false;
            this.materialTextBoxEditLogin.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxEditLogin.TrailingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxEditLogin.TrailingIcon")));
            this.materialTextBoxEditLogin.UseSystemPasswordChar = false;
            // 
            // materialButtonPasswordAdd
            // 
            resources.ApplyResources(this.materialButtonPasswordAdd, "materialButtonPasswordAdd");
            this.materialButtonPasswordAdd.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButtonPasswordAdd.Depth = 0;
            this.materialButtonPasswordAdd.HighEmphasis = true;
            this.materialButtonPasswordAdd.Icon = ((System.Drawing.Image)(resources.GetObject("materialButtonPasswordAdd.Icon")));
            this.materialButtonPasswordAdd.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.materialButtonPasswordAdd.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialButtonPasswordAdd.Name = "materialButtonPasswordAdd";
            this.materialButtonPasswordAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButtonPasswordAdd.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButtonPasswordAdd.UseAccentColor = true;
            this.materialButtonPasswordAdd.UseVisualStyleBackColor = true;
            this.materialButtonPasswordAdd.Click += new System.EventHandler(this.buttonAdd_Click);
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
            this.Controls.Add(this.materialButtonSave);
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
        private System.Windows.Forms.ImageList imageListTabCtrl;
        private PoisonDataGridView poisonGridVault;
        private MaterialTextBoxEdit materialTextBoxFilter;
        private MaterialButton materialButtonSave;
        private PoisonContextMenuStrip poisonContextMenuStripGrid;
        private System.Windows.Forms.ToolStripMenuItem supprimerLélémentToolStripMenuItem1;
        private System.Windows.Forms.TabPage tabPageAbout;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewGitHubProfile;
        private MaterialButton materialButtonImportBrowserData;
        private MaterialButton materialButtonShowBrowserDatas;
        private MaterialCheckListBox materialCheckListBoxBrowserData;
        private MaterialTextBoxEdit materialTextBoxEditUrl;
        private MaterialTextBoxEdit materialTextBoxEditLogin;
        private MaterialButton materialButtonPasswordAdd;
        private MaterialTextBoxEdit materialTextBoxEditKeyword;
        private MaterialTextBoxEdit materialTextBoxEditPassword;
    }
}

