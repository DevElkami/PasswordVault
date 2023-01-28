using NLog;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Manager;
using System;
using System.Windows.Forms;
using VaultCore;

namespace WinformPasswordVault
{
    public partial class FormAskPassword : MaterialForm
    {
        private MyVault myVault = new();
        private int nbTry = 3;

        public FormAskPassword(ref MyVault vault)
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

        private void FormAskPassword_Load(object sender, System.EventArgs e)
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

        private void materialButtonCheck_Click(object sender, EventArgs e)
        {
            try
            {
                if (myVault.CheckVaultKey(materialTextBoxUserPassword.Text) == true)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MaterialSnackBar SnackBarMessage = new(Properties.Resources.ResourceManager.GetString("ErrorMsgCountTry") + (--nbTry).ToString(), "OK", true);
                    SnackBarMessage.Show(this);

                    DialogResult = (nbTry > 0) ? DialogResult.None : DialogResult.Abort;
                }
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }
    }
}
