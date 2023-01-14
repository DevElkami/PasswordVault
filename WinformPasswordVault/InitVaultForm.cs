using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformPasswordVault
{
    public partial class InitVaultForm : Form
    {
        public InitVaultForm()
        {
            InitializeComponent();
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
                    MessageBox.Show("");
                    return;
                }

                if (String.IsNullOrEmpty(textBoxPassword1.Text))
                {
                    MessageBox.Show("");
                    return;
                }

                if (textBoxPassword1.Text.Length < 6)
                {
                    MessageBox.Show("");
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
