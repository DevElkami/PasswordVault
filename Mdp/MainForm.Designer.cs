namespace Mdp
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
            this.myVaultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonAdd = new System.Windows.Forms.Button();
            this.groupBoxMyPassword = new System.Windows.Forms.GroupBox();
            this.labelMdpLogin = new System.Windows.Forms.Label();
            this.textBoxMdpLogin = new System.Windows.Forms.TextBox();
            this.textBoxMdpPassword = new System.Windows.Forms.TextBox();
            this.labelMdpPassword = new System.Windows.Forms.Label();
            this.textBoxMdpData = new System.Windows.Forms.TextBox();
            this.labelMdpData = new System.Windows.Forms.Label();
            this.textBoxMdpKeyword = new System.Windows.Forms.TextBox();
            this.labelMdpKeyword = new System.Windows.Forms.Label();
            this.tabControlMain.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVault)).BeginInit();
            this.contextMenuStripRow.SuspendLayout();
            this.tabPageFirefox.SuspendLayout();
            this.tabPageOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myVaultBindingSource)).BeginInit();
            this.groupBoxMyPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageSearch);
            this.tabControlMain.Controls.Add(this.tabPageFirefox);
            this.tabControlMain.Controls.Add(this.tabPageOption);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(832, 450);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabPageSearch.Controls.Add(this.buttonSave);
            this.tabPageSearch.Controls.Add(this.textBoxFilter);
            this.tabPageSearch.Controls.Add(this.dataGridViewVault);
            this.tabPageSearch.Location = new System.Drawing.Point(4, 22);
            this.tabPageSearch.Name = "tabPageSearch";
            this.tabPageSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSearch.Size = new System.Drawing.Size(838, 424);
            this.tabPageSearch.TabIndex = 0;
            this.tabPageSearch.Text = "Recherche";
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonSave.BackgroundImage = global::Mdp.Properties.Resources.save;
            this.buttonSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSave.Location = new System.Drawing.Point(807, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(23, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilter.Location = new System.Drawing.Point(8, 6);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(793, 20);
            this.textBoxFilter.TabIndex = 1;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // dataGridViewVault
            // 
            this.dataGridViewVault.AllowUserToAddRows = false;
            this.dataGridViewVault.AllowUserToDeleteRows = false;
            this.dataGridViewVault.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewVault.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVault.ContextMenuStrip = this.contextMenuStripRow;
            this.dataGridViewVault.Location = new System.Drawing.Point(8, 32);
            this.dataGridViewVault.Name = "dataGridViewVault";
            this.dataGridViewVault.ShowCellErrors = false;
            this.dataGridViewVault.ShowCellToolTips = false;
            this.dataGridViewVault.ShowEditingIcon = false;
            this.dataGridViewVault.ShowRowErrors = false;
            this.dataGridViewVault.Size = new System.Drawing.Size(822, 384);
            this.dataGridViewVault.TabIndex = 0;
            // 
            // contextMenuStripRow
            // 
            this.contextMenuStripRow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.supprimerLélémentToolStripMenuItem});
            this.contextMenuStripRow.Name = "contextMenuStrip1";
            this.contextMenuStripRow.Size = new System.Drawing.Size(182, 26);
            // 
            // supprimerLélémentToolStripMenuItem
            // 
            this.supprimerLélémentToolStripMenuItem.Name = "supprimerLélémentToolStripMenuItem";
            this.supprimerLélémentToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.supprimerLélémentToolStripMenuItem.Text = "Supprimer l\'élément";
            this.supprimerLélémentToolStripMenuItem.Click += new System.EventHandler(this.deleteRowToolStripMenuItem_Click);
            // 
            // tabPageFirefox
            // 
            this.tabPageFirefox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabPageFirefox.Controls.Add(this.checkedListBoxMdpFireFox);
            this.tabPageFirefox.Controls.Add(this.buttonViewFirefox);
            this.tabPageFirefox.Controls.Add(this.labelViewFirefox);
            this.tabPageFirefox.Controls.Add(this.buttonImportFromFirefox);
            this.tabPageFirefox.Controls.Add(this.labelImportFromFirefox);
            this.tabPageFirefox.Location = new System.Drawing.Point(4, 22);
            this.tabPageFirefox.Name = "tabPageFirefox";
            this.tabPageFirefox.Size = new System.Drawing.Size(838, 424);
            this.tabPageFirefox.TabIndex = 2;
            this.tabPageFirefox.Text = "Firefox";
            // 
            // checkedListBoxMdpFireFox
            // 
            this.checkedListBoxMdpFireFox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxMdpFireFox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.checkedListBoxMdpFireFox.FormattingEnabled = true;
            this.checkedListBoxMdpFireFox.Location = new System.Drawing.Point(8, 75);
            this.checkedListBoxMdpFireFox.Name = "checkedListBoxMdpFireFox";
            this.checkedListBoxMdpFireFox.Size = new System.Drawing.Size(822, 319);
            this.checkedListBoxMdpFireFox.TabIndex = 6;
            // 
            // buttonViewFirefox
            // 
            this.buttonViewFirefox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonViewFirefox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonViewFirefox.Location = new System.Drawing.Point(135, 17);
            this.buttonViewFirefox.Name = "buttonViewFirefox";
            this.buttonViewFirefox.Size = new System.Drawing.Size(75, 23);
            this.buttonViewFirefox.TabIndex = 5;
            this.buttonViewFirefox.Text = "Voir";
            this.buttonViewFirefox.UseVisualStyleBackColor = false;
            this.buttonViewFirefox.Click += new System.EventHandler(this.buttonImportFirefox_Click);
            // 
            // labelViewFirefox
            // 
            this.labelViewFirefox.AutoSize = true;
            this.labelViewFirefox.Location = new System.Drawing.Point(19, 22);
            this.labelViewFirefox.Name = "labelViewFirefox";
            this.labelViewFirefox.Size = new System.Drawing.Size(64, 13);
            this.labelViewFirefox.TabIndex = 4;
            this.labelViewFirefox.Text = "Voir les mdp";
            // 
            // buttonImportFromFirefox
            // 
            this.buttonImportFromFirefox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonImportFromFirefox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImportFromFirefox.Location = new System.Drawing.Point(135, 46);
            this.buttonImportFromFirefox.Name = "buttonImportFromFirefox";
            this.buttonImportFromFirefox.Size = new System.Drawing.Size(75, 23);
            this.buttonImportFromFirefox.TabIndex = 3;
            this.buttonImportFromFirefox.Text = "Import";
            this.buttonImportFromFirefox.UseVisualStyleBackColor = false;
            this.buttonImportFromFirefox.Click += new System.EventHandler(this.buttonImportFromFirefox_Click);
            // 
            // labelImportFromFirefox
            // 
            this.labelImportFromFirefox.AutoSize = true;
            this.labelImportFromFirefox.Location = new System.Drawing.Point(19, 51);
            this.labelImportFromFirefox.Name = "labelImportFromFirefox";
            this.labelImportFromFirefox.Size = new System.Drawing.Size(97, 13);
            this.labelImportFromFirefox.TabIndex = 2;
            this.labelImportFromFirefox.Text = "Importer dans l\'outil";
            // 
            // tabPageOption
            // 
            this.tabPageOption.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabPageOption.Controls.Add(this.groupBoxMyPassword);
            this.tabPageOption.Controls.Add(this.buttonAdd);
            this.tabPageOption.Location = new System.Drawing.Point(4, 22);
            this.tabPageOption.Name = "tabPageOption";
            this.tabPageOption.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOption.Size = new System.Drawing.Size(824, 424);
            this.tabPageOption.TabIndex = 1;
            this.tabPageOption.Text = "Option";
            // 
            // myVaultBindingSource
            // 
            this.myVaultBindingSource.DataSource = typeof(Mdp.MyVault);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdd.Location = new System.Drawing.Point(743, 12);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Ajouter";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // groupBoxMyPassword
            // 
            this.groupBoxMyPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMyPassword.Controls.Add(this.textBoxMdpKeyword);
            this.groupBoxMyPassword.Controls.Add(this.labelMdpKeyword);
            this.groupBoxMyPassword.Controls.Add(this.textBoxMdpData);
            this.groupBoxMyPassword.Controls.Add(this.labelMdpData);
            this.groupBoxMyPassword.Controls.Add(this.textBoxMdpPassword);
            this.groupBoxMyPassword.Controls.Add(this.labelMdpPassword);
            this.groupBoxMyPassword.Controls.Add(this.textBoxMdpLogin);
            this.groupBoxMyPassword.Controls.Add(this.labelMdpLogin);
            this.groupBoxMyPassword.Location = new System.Drawing.Point(8, 6);
            this.groupBoxMyPassword.Name = "groupBoxMyPassword";
            this.groupBoxMyPassword.Size = new System.Drawing.Size(727, 410);
            this.groupBoxMyPassword.TabIndex = 2;
            this.groupBoxMyPassword.TabStop = false;
            this.groupBoxMyPassword.Text = "Ajout d\'un mdp";
            // 
            // labelMdpLogin
            // 
            this.labelMdpLogin.AutoSize = true;
            this.labelMdpLogin.Location = new System.Drawing.Point(19, 28);
            this.labelMdpLogin.Name = "labelMdpLogin";
            this.labelMdpLogin.Size = new System.Drawing.Size(36, 13);
            this.labelMdpLogin.TabIndex = 0;
            this.labelMdpLogin.Text = "Login:";
            // 
            // textBoxMdpLogin
            // 
            this.textBoxMdpLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMdpLogin.Location = new System.Drawing.Point(166, 25);
            this.textBoxMdpLogin.Name = "textBoxMdpLogin";
            this.textBoxMdpLogin.Size = new System.Drawing.Size(542, 20);
            this.textBoxMdpLogin.TabIndex = 1;
            // 
            // textBoxMdpPassword
            // 
            this.textBoxMdpPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMdpPassword.Location = new System.Drawing.Point(166, 51);
            this.textBoxMdpPassword.Name = "textBoxMdpPassword";
            this.textBoxMdpPassword.Size = new System.Drawing.Size(542, 20);
            this.textBoxMdpPassword.TabIndex = 3;
            // 
            // labelMdpPassword
            // 
            this.labelMdpPassword.AutoSize = true;
            this.labelMdpPassword.Location = new System.Drawing.Point(19, 54);
            this.labelMdpPassword.Name = "labelMdpPassword";
            this.labelMdpPassword.Size = new System.Drawing.Size(74, 13);
            this.labelMdpPassword.TabIndex = 2;
            this.labelMdpPassword.Text = "Mot de passe:";
            // 
            // textBoxMdpData
            // 
            this.textBoxMdpData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMdpData.Location = new System.Drawing.Point(166, 77);
            this.textBoxMdpData.Name = "textBoxMdpData";
            this.textBoxMdpData.Size = new System.Drawing.Size(542, 20);
            this.textBoxMdpData.TabIndex = 5;
            // 
            // labelMdpData
            // 
            this.labelMdpData.AutoSize = true;
            this.labelMdpData.Location = new System.Drawing.Point(19, 80);
            this.labelMdpData.Name = "labelMdpData";
            this.labelMdpData.Size = new System.Drawing.Size(53, 13);
            this.labelMdpData.TabIndex = 4;
            this.labelMdpData.Text = "Données:";
            // 
            // textBoxMdpKeyword
            // 
            this.textBoxMdpKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMdpKeyword.Location = new System.Drawing.Point(166, 103);
            this.textBoxMdpKeyword.Name = "textBoxMdpKeyword";
            this.textBoxMdpKeyword.Size = new System.Drawing.Size(542, 20);
            this.textBoxMdpKeyword.TabIndex = 7;
            // 
            // labelMdpKeyword
            // 
            this.labelMdpKeyword.AutoSize = true;
            this.labelMdpKeyword.Location = new System.Drawing.Point(19, 106);
            this.labelMdpKeyword.Name = "labelMdpKeyword";
            this.labelMdpKeyword.Size = new System.Drawing.Size(58, 13);
            this.labelMdpKeyword.TabIndex = 6;
            this.labelMdpKeyword.Text = "Mots clefs:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(832, 450);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pour ne plus oublier les mots de passe";
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
            ((System.ComponentModel.ISupportInitialize)(this.myVaultBindingSource)).EndInit();
            this.groupBoxMyPassword.ResumeLayout(false);
            this.groupBoxMyPassword.PerformLayout();
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
    }
}

