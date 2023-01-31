using NLog;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Manager;
using System;
using System.Threading;
using System.Windows.Forms;
using VaultCore;

namespace WinformPasswordVault
{
    public partial class InitVaultForm : MaterialForm
    {
        private readonly MyVault myVault = new();

        public InitVaultForm(ref MyVault vault)
        {
            try
            {
                myVault = vault;
                InitializeComponent();

                MaterialSkinManager.Instance.AddFormToManage(this);
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

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
                DialogResult = DialogResult.None;

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

                myVault.Initialize(materialTextBoxUserPassword1.Text);

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
