namespace WinformPasswordVault
{
    partial class InitVaultForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitVaultForm));
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.materialButtonInit = new ReaLTaiizor.Controls.MaterialButton();
            this.materialTextBoxUserPassword1 = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            this.materialTextBoxUserPassword2 = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            this.materialCardInformation = new ReaLTaiizor.Controls.MaterialCard();
            this.materialLabel4 = new ReaLTaiizor.Controls.MaterialLabel();
            this.materialLabel38 = new ReaLTaiizor.Controls.MaterialLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.materialCardInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            resources.ApplyResources(this.pictureBoxLogo, "pictureBoxLogo");
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.TabStop = false;
            // 
            // materialButtonInit
            // 
            resources.ApplyResources(this.materialButtonInit, "materialButtonInit");
            this.materialButtonInit.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButtonInit.Depth = 0;
            this.materialButtonInit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.materialButtonInit.HighEmphasis = true;
            this.materialButtonInit.Icon = null;
            this.materialButtonInit.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.materialButtonInit.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialButtonInit.Name = "materialButtonInit";
            this.materialButtonInit.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButtonInit.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButtonInit.UseAccentColor = true;
            this.materialButtonInit.UseVisualStyleBackColor = true;
            this.materialButtonInit.Click += new System.EventHandler(this.buttonInitialize_Click);
            // 
            // materialTextBoxUserPassword1
            // 
            resources.ApplyResources(this.materialTextBoxUserPassword1, "materialTextBoxUserPassword1");
            this.materialTextBoxUserPassword1.AnimateReadOnly = false;
            this.materialTextBoxUserPassword1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.materialTextBoxUserPassword1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.materialTextBoxUserPassword1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxUserPassword1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialTextBoxUserPassword1.Depth = 0;
            this.materialTextBoxUserPassword1.HideSelection = true;
            this.materialTextBoxUserPassword1.LeadingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxUserPassword1.LeadingIcon")));
            this.materialTextBoxUserPassword1.MaxLength = 50;
            this.materialTextBoxUserPassword1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.materialTextBoxUserPassword1.Name = "materialTextBoxUserPassword1";
            this.materialTextBoxUserPassword1.PasswordChar = '●';
            this.materialTextBoxUserPassword1.ReadOnly = false;
            this.materialTextBoxUserPassword1.SelectedText = "";
            this.materialTextBoxUserPassword1.SelectionLength = 0;
            this.materialTextBoxUserPassword1.SelectionStart = 0;
            this.materialTextBoxUserPassword1.ShortcutsEnabled = true;
            this.materialTextBoxUserPassword1.TabStop = false;
            this.materialTextBoxUserPassword1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxUserPassword1.TrailingIcon = null;
            this.materialTextBoxUserPassword1.UseSystemPasswordChar = true;
            // 
            // materialTextBoxUserPassword2
            // 
            resources.ApplyResources(this.materialTextBoxUserPassword2, "materialTextBoxUserPassword2");
            this.materialTextBoxUserPassword2.AnimateReadOnly = false;
            this.materialTextBoxUserPassword2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.materialTextBoxUserPassword2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.materialTextBoxUserPassword2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxUserPassword2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialTextBoxUserPassword2.Depth = 0;
            this.materialTextBoxUserPassword2.HideSelection = true;
            this.materialTextBoxUserPassword2.LeadingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxUserPassword2.LeadingIcon")));
            this.materialTextBoxUserPassword2.MaxLength = 50;
            this.materialTextBoxUserPassword2.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.materialTextBoxUserPassword2.Name = "materialTextBoxUserPassword2";
            this.materialTextBoxUserPassword2.PasswordChar = '●';
            this.materialTextBoxUserPassword2.ReadOnly = false;
            this.materialTextBoxUserPassword2.SelectedText = "";
            this.materialTextBoxUserPassword2.SelectionLength = 0;
            this.materialTextBoxUserPassword2.SelectionStart = 0;
            this.materialTextBoxUserPassword2.ShortcutsEnabled = true;
            this.materialTextBoxUserPassword2.TabStop = false;
            this.materialTextBoxUserPassword2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxUserPassword2.TrailingIcon = null;
            this.materialTextBoxUserPassword2.UseSystemPasswordChar = true;
            // 
            // materialCardInformation
            // 
            resources.ApplyResources(this.materialCardInformation, "materialCardInformation");
            this.materialCardInformation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCardInformation.Controls.Add(this.materialLabel4);
            this.materialCardInformation.Controls.Add(this.materialLabel38);
            this.materialCardInformation.Depth = 0;
            this.materialCardInformation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCardInformation.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialCardInformation.Name = "materialCardInformation";
            // 
            // materialLabel4
            // 
            resources.ApplyResources(this.materialLabel4, "materialLabel4");
            this.materialLabel4.Depth = 0;
            this.materialLabel4.FontType = ReaLTaiizor.Manager.MaterialSkinManager.FontType.H6;
            this.materialLabel4.HighEmphasis = true;
            this.materialLabel4.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.UseAccent = true;
            // 
            // materialLabel38
            // 
            resources.ApplyResources(this.materialLabel38, "materialLabel38");
            this.materialLabel38.Depth = 0;
            this.materialLabel38.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialLabel38.Name = "materialLabel38";
            // 
            // InitVaultForm
            // 
            this.AcceptButton = this.materialButtonInit;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.materialCardInformation);
            this.Controls.Add(this.materialTextBoxUserPassword2);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.materialButtonInit);
            this.Controls.Add(this.materialTextBoxUserPassword1);
            this.DrawerAutoHide = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitVaultForm";
            this.Sizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.InitVaultForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.materialCardInformation.ResumeLayout(false);
            this.materialCardInformation.PerformLayout();
            this.ResumeLayout(false);

        }       
        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private ReaLTaiizor.Controls.MaterialButton materialButtonInit;
        private ReaLTaiizor.Controls.MaterialTextBoxEdit materialTextBoxUserPassword1;
        private ReaLTaiizor.Controls.MaterialTextBoxEdit materialTextBoxUserPassword2;
        private ReaLTaiizor.Controls.MaterialCard materialCardInformation;
        private ReaLTaiizor.Controls.MaterialLabel materialLabel4;
        private ReaLTaiizor.Controls.MaterialLabel materialLabel38;
    }
}