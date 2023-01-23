using NLog;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Manager;
using System;
using System.Windows.Forms;

namespace WinformPasswordVault
{
    public partial class InitVaultForm : MaterialForm
    {
        public InitVaultForm()
        {
            try
            {
                InitializeComponent();

                MaterialSkinManager.Instance.AddFormToManage(this);
                this.FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_None;
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
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
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        private void buttonInitialize_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxPassword1.Text != textBoxPassword2.Text)
                {
                    MessageBox.Show(Properties.Resources.ResourceManager.GetString("ErrorMsgPasswordsMustSame"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (String.IsNullOrEmpty(textBoxPassword1.Text))
                {
                    MessageBox.Show(Properties.Resources.ResourceManager.GetString("ErrorMsgPasswordLengthMinimum"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (textBoxPassword1.Text.Length < 6)
                {
                    MessageBox.Show(Properties.Resources.ResourceManager.GetString("ErrorMsgPasswordLengthMinimum"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UserPassword = textBoxPassword1.Text;
                DialogResult = DialogResult.OK;
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }
    }
}
