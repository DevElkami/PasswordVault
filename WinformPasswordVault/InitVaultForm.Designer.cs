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
            this.labelPassword1 = new System.Windows.Forms.Label();
            this.labelPassword2 = new System.Windows.Forms.Label();
            this.textBoxPassword1 = new System.Windows.Forms.TextBox();
            this.textBoxPassword2 = new System.Windows.Forms.TextBox();
            this.buttonInitialize = new System.Windows.Forms.Button();
            this.labelErrorMsgNotSame = new System.Windows.Forms.Label();
            this.labelErrorMsgTooShort = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPassword1
            // 
            resources.ApplyResources(this.labelPassword1, "labelPassword1");
            this.labelPassword1.Name = "labelPassword1";
            // 
            // labelPassword2
            // 
            resources.ApplyResources(this.labelPassword2, "labelPassword2");
            this.labelPassword2.Name = "labelPassword2";
            // 
            // textBoxPassword1
            // 
            resources.ApplyResources(this.textBoxPassword1, "textBoxPassword1");
            this.textBoxPassword1.Name = "textBoxPassword1";
            this.textBoxPassword1.UseSystemPasswordChar = true;
            // 
            // textBoxPassword2
            // 
            resources.ApplyResources(this.textBoxPassword2, "textBoxPassword2");
            this.textBoxPassword2.Name = "textBoxPassword2";
            this.textBoxPassword2.UseSystemPasswordChar = true;
            // 
            // buttonInitialize
            // 
            resources.ApplyResources(this.buttonInitialize, "buttonInitialize");
            this.buttonInitialize.Name = "buttonInitialize";
            this.buttonInitialize.UseVisualStyleBackColor = true;
            this.buttonInitialize.Click += new System.EventHandler(this.buttonInitialize_Click);
            // 
            // labelErrorMsgNotSame
            // 
            resources.ApplyResources(this.labelErrorMsgNotSame, "labelErrorMsgNotSame");
            this.labelErrorMsgNotSame.Name = "labelErrorMsgNotSame";
            // 
            // labelErrorMsgTooShort
            // 
            resources.ApplyResources(this.labelErrorMsgTooShort, "labelErrorMsgTooShort");
            this.labelErrorMsgTooShort.Name = "labelErrorMsgTooShort";
            // 
            // InitVaultForm
            // 
            this.AcceptButton = this.buttonInitialize;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelErrorMsgTooShort);
            this.Controls.Add(this.labelErrorMsgNotSame);
            this.Controls.Add(this.buttonInitialize);
            this.Controls.Add(this.textBoxPassword2);
            this.Controls.Add(this.textBoxPassword1);
            this.Controls.Add(this.labelPassword2);
            this.Controls.Add(this.labelPassword1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InitVaultForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.InitVaultForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPassword1;
        private System.Windows.Forms.Label labelPassword2;
        private System.Windows.Forms.TextBox textBoxPassword1;
        private System.Windows.Forms.TextBox textBoxPassword2;
        private System.Windows.Forms.Button buttonInitialize;
        private System.Windows.Forms.Label labelErrorMsgNotSame;
        private System.Windows.Forms.Label labelErrorMsgTooShort;
    }
}