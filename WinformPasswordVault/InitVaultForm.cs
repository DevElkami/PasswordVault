using NLog;
using ReaLTaiizor.Controls;
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
                if (materialTextBoxUserPassword1.Text != materialTextBoxUserPassword2.Text)
                {
                    MaterialSnackBar SnackBarMessage = new(Properties.Resources.ResourceManager.GetString("ErrorMsgPasswordsMustSame"), "OK", true);
                    SnackBarMessage.Show(this);
                    return;
                }

                if (String.IsNullOrEmpty(materialTextBoxUserPassword1.Text))
                {
                    MaterialSnackBar SnackBarMessage = new(Properties.Resources.ResourceManager.GetString("ErrorMsgPasswordLengthMinimum"), "OK", true);
                    SnackBarMessage.Show(this);
                    return;
                }

                if (materialTextBoxUserPassword1.Text.Length < 6)
                {
                    MaterialSnackBar SnackBarMessage = new(Properties.Resources.ResourceManager.GetString("ErrorMsgPasswordLengthMinimum"), "OK", true);
                    SnackBarMessage.Show(this);
                    return;
                }

                UserPassword = materialTextBoxUserPassword1.Text;
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
