using System;
using System.Threading;
using System.Windows.Forms;

namespace WinformPasswordVault
{
    public partial class InitVaultForm : Form
    {
        private System.ComponentModel.ComponentResourceManager resources = null;

        public InitVaultForm()
        {
            try
            {
                System.Globalization.CultureInfo culture = null;
                if (Thread.CurrentThread.CurrentCulture.IetfLanguageTag.Contains("fr"))
                    culture = new System.Globalization.CultureInfo("");
                else
                    culture = new System.Globalization.CultureInfo("en");

                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                resources = new System.ComponentModel.ComponentResourceManager(typeof(InitVaultForm));
                InitializeComponent();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
            }
        }

        public String UserPassword { get; private set; }

        private void InitVaultForm_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
            }
        }

        private void buttonInitialize_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxPassword1.Text != textBoxPassword2.Text)
                {
                    MessageBox.Show(resources.GetString("labelErrorMsgNotSame.Text"), resources.GetString("$this.Text"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (String.IsNullOrEmpty(textBoxPassword1.Text))
                {
                    MessageBox.Show(resources.GetString("labelErrorMsgTooShort.Text"), resources.GetString("$this.Text"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (textBoxPassword1.Text.Length < 6)
                {
                    MessageBox.Show(resources.GetString("labelErrorMsgTooShort.Text"), resources.GetString("$this.Text"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UserPassword = textBoxPassword1.Text;
                DialogResult = DialogResult.OK;
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
            }
        }
    }
}
