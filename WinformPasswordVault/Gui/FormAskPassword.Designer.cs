namespace WinformPasswordVault
{
    partial class FormAskPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAskPassword));
            this.materialTextBoxUserPassword = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            this.materialButtonCheck = new ReaLTaiizor.Controls.MaterialButton();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // materialTextBoxUserPassword
            // 
            this.materialTextBoxUserPassword.AnimateReadOnly = false;
            this.materialTextBoxUserPassword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.materialTextBoxUserPassword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.materialTextBoxUserPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.materialTextBoxUserPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxUserPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.materialTextBoxUserPassword.Depth = 0;
            this.materialTextBoxUserPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTextBoxUserPassword.HideSelection = true;
            this.materialTextBoxUserPassword.Hint = "Mot de passe";
            this.materialTextBoxUserPassword.LeadingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxUserPassword.LeadingIcon")));
            this.materialTextBoxUserPassword.Location = new System.Drawing.Point(31, 87);
            this.materialTextBoxUserPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.materialTextBoxUserPassword.MaxLength = 50;
            this.materialTextBoxUserPassword.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.materialTextBoxUserPassword.Name = "materialTextBoxUserPassword";
            this.materialTextBoxUserPassword.PasswordChar = '●';
            this.materialTextBoxUserPassword.PrefixSuffixText = null;
            this.materialTextBoxUserPassword.ReadOnly = false;
            this.materialTextBoxUserPassword.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.materialTextBoxUserPassword.SelectedText = "";
            this.materialTextBoxUserPassword.SelectionLength = 0;
            this.materialTextBoxUserPassword.SelectionStart = 0;
            this.materialTextBoxUserPassword.ShortcutsEnabled = true;
            this.materialTextBoxUserPassword.Size = new System.Drawing.Size(402, 48);
            this.materialTextBoxUserPassword.TabIndex = 4;
            this.materialTextBoxUserPassword.TabStop = false;
            this.materialTextBoxUserPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxUserPassword.TrailingIcon = null;
            this.materialTextBoxUserPassword.UseSystemPasswordChar = true;
            // 
            // materialButtonCheck
            // 
            this.materialButtonCheck.AutoSize = false;
            this.materialButtonCheck.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButtonCheck.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButtonCheck.Depth = 0;
            this.materialButtonCheck.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.materialButtonCheck.HighEmphasis = true;
            this.materialButtonCheck.Icon = null;
            this.materialButtonCheck.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.materialButtonCheck.Location = new System.Drawing.Point(237, 151);
            this.materialButtonCheck.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.materialButtonCheck.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialButtonCheck.Name = "materialButtonCheck";
            this.materialButtonCheck.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButtonCheck.Size = new System.Drawing.Size(196, 42);
            this.materialButtonCheck.TabIndex = 10;
            this.materialButtonCheck.Text = "Vérifier";
            this.materialButtonCheck.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButtonCheck.UseAccentColor = true;
            this.materialButtonCheck.UseVisualStyleBackColor = true;
            this.materialButtonCheck.Click += new System.EventHandler(this.materialButtonCheck_Click);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLogo.Image")));
            this.pictureBoxLogo.Location = new System.Drawing.Point(6, 168);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(96, 96);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // FormAskPassword
            // 
            this.AcceptButton = this.materialButtonCheck;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 270);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.materialButtonCheck);
            this.Controls.Add(this.materialTextBoxUserPassword);
            this.DrawerAutoHide = false;
            this.DrawerHighlightWithAccent = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAskPassword";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Déverrouillage";
            this.Load += new System.EventHandler(this.FormAskPassword_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.MaterialTextBoxEdit materialTextBoxUserPassword;
        private ReaLTaiizor.Controls.MaterialButton materialButtonCheck;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
    }
}