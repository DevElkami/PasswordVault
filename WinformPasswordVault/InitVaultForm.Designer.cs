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
            this.pictureBoxLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLogo.Image")));
            this.pictureBoxLogo.Location = new System.Drawing.Point(6, 375);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(96, 96);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 2;
            this.pictureBoxLogo.TabStop = false;
            // 
            // materialButtonInit
            // 
            this.materialButtonInit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.materialButtonInit.AutoSize = false;
            this.materialButtonInit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButtonInit.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButtonInit.Depth = 0;
            this.materialButtonInit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.materialButtonInit.HighEmphasis = true;
            this.materialButtonInit.Icon = null;
            this.materialButtonInit.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.materialButtonInit.Location = new System.Drawing.Point(252, 379);
            this.materialButtonInit.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.materialButtonInit.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialButtonInit.Name = "materialButtonInit";
            this.materialButtonInit.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButtonInit.Size = new System.Drawing.Size(196, 36);
            this.materialButtonInit.TabIndex = 3;
            this.materialButtonInit.Text = "Initialize";
            this.materialButtonInit.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButtonInit.UseAccentColor = true;
            this.materialButtonInit.UseVisualStyleBackColor = true;
            // 
            // materialTextBoxUserPassword1
            // 
            this.materialTextBoxUserPassword1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTextBoxUserPassword1.AnimateReadOnly = false;
            this.materialTextBoxUserPassword1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.materialTextBoxUserPassword1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.materialTextBoxUserPassword1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.materialTextBoxUserPassword1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxUserPassword1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialTextBoxUserPassword1.Depth = 0;
            this.materialTextBoxUserPassword1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTextBoxUserPassword1.HideSelection = true;
            this.materialTextBoxUserPassword1.Hint = "Password";
            this.materialTextBoxUserPassword1.LeadingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxUserPassword1.LeadingIcon")));
            this.materialTextBoxUserPassword1.Location = new System.Drawing.Point(30, 255);
            this.materialTextBoxUserPassword1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.materialTextBoxUserPassword1.MaxLength = 50;
            this.materialTextBoxUserPassword1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.materialTextBoxUserPassword1.Name = "materialTextBoxUserPassword1";
            this.materialTextBoxUserPassword1.PasswordChar = '●';
            this.materialTextBoxUserPassword1.PrefixSuffixText = null;
            this.materialTextBoxUserPassword1.ReadOnly = false;
            this.materialTextBoxUserPassword1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.materialTextBoxUserPassword1.SelectedText = "";
            this.materialTextBoxUserPassword1.SelectionLength = 0;
            this.materialTextBoxUserPassword1.SelectionStart = 0;
            this.materialTextBoxUserPassword1.ShortcutsEnabled = true;
            this.materialTextBoxUserPassword1.Size = new System.Drawing.Size(418, 48);
            this.materialTextBoxUserPassword1.TabIndex = 4;
            this.materialTextBoxUserPassword1.TabStop = false;
            this.materialTextBoxUserPassword1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxUserPassword1.TrailingIcon = null;
            this.materialTextBoxUserPassword1.UseSystemPasswordChar = true;
            // 
            // materialTextBoxUserPassword2
            // 
            this.materialTextBoxUserPassword2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTextBoxUserPassword2.AnimateReadOnly = false;
            this.materialTextBoxUserPassword2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.materialTextBoxUserPassword2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.materialTextBoxUserPassword2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.materialTextBoxUserPassword2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxUserPassword2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialTextBoxUserPassword2.Depth = 0;
            this.materialTextBoxUserPassword2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTextBoxUserPassword2.HideSelection = true;
            this.materialTextBoxUserPassword2.Hint = "Confirmed password";
            this.materialTextBoxUserPassword2.LeadingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxUserPassword2.LeadingIcon")));
            this.materialTextBoxUserPassword2.Location = new System.Drawing.Point(30, 317);
            this.materialTextBoxUserPassword2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.materialTextBoxUserPassword2.MaxLength = 50;
            this.materialTextBoxUserPassword2.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.materialTextBoxUserPassword2.Name = "materialTextBoxUserPassword2";
            this.materialTextBoxUserPassword2.PasswordChar = '●';
            this.materialTextBoxUserPassword2.PrefixSuffixText = null;
            this.materialTextBoxUserPassword2.ReadOnly = false;
            this.materialTextBoxUserPassword2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.materialTextBoxUserPassword2.SelectedText = "";
            this.materialTextBoxUserPassword2.SelectionLength = 0;
            this.materialTextBoxUserPassword2.SelectionStart = 0;
            this.materialTextBoxUserPassword2.ShortcutsEnabled = true;
            this.materialTextBoxUserPassword2.Size = new System.Drawing.Size(418, 48);
            this.materialTextBoxUserPassword2.TabIndex = 1;
            this.materialTextBoxUserPassword2.TabStop = false;
            this.materialTextBoxUserPassword2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxUserPassword2.TrailingIcon = null;
            this.materialTextBoxUserPassword2.UseSystemPasswordChar = true;
            // 
            // materialCardInformation
            // 
            this.materialCardInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialCardInformation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCardInformation.Controls.Add(this.materialLabel4);
            this.materialCardInformation.Controls.Add(this.materialLabel38);
            this.materialCardInformation.Depth = 0;
            this.materialCardInformation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCardInformation.Location = new System.Drawing.Point(20, 87);
            this.materialCardInformation.Margin = new System.Windows.Forms.Padding(8);
            this.materialCardInformation.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialCardInformation.Name = "materialCardInformation";
            this.materialCardInformation.Padding = new System.Windows.Forms.Padding(16);
            this.materialCardInformation.Size = new System.Drawing.Size(441, 155);
            this.materialCardInformation.TabIndex = 0;
            // 
            // materialLabel4
            // 
            this.materialLabel4.AutoSize = true;
            this.materialLabel4.Depth = 0;
            this.materialLabel4.Font = new System.Drawing.Font("Roboto Medium", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel4.FontType = ReaLTaiizor.Manager.MaterialSkinManager.FontType.H6;
            this.materialLabel4.HighEmphasis = true;
            this.materialLabel4.Location = new System.Drawing.Point(10, 16);
            this.materialLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.materialLabel4.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(247, 24);
            this.materialLabel4.TabIndex = 1;
            this.materialLabel4.Text = "Choose a strong password !";
            this.materialLabel4.UseAccent = true;
            // 
            // materialLabel38
            // 
            this.materialLabel38.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialLabel38.Depth = 0;
            this.materialLabel38.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel38.Location = new System.Drawing.Point(12, 59);
            this.materialLabel38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.materialLabel38.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialLabel38.Name = "materialLabel38";
            this.materialLabel38.Size = new System.Drawing.Size(416, 80);
            this.materialLabel38.TabIndex = 2;
            this.materialLabel38.Text = "Help prevent a security threat by getting a strong password...and remember: it\'s " +
    "impossible to recover once you choose it.";
            // 
            // InitVaultForm
            // 
            this.AcceptButton = this.materialButtonInit;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(480, 477);
            this.Controls.Add(this.materialCardInformation);
            this.Controls.Add(this.materialTextBoxUserPassword2);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.materialButtonInit);
            this.Controls.Add(this.materialTextBoxUserPassword1);
            this.DrawerAutoHide = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitVaultForm";
            this.Sizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "First launch";
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