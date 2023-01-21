using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using VaultCore;
using VaultCore.Models;

namespace WinformPasswordVault
{
    public partial class MainForm : Form
    {
        private MyVault myVault = new();

        public MainForm()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (myVault.IsInitialized() == false)
                {
                    InitVaultForm initVaultForm = new();
                    if (initVaultForm.ShowDialog() != DialogResult.OK)
                    {
                        DialogResult = DialogResult.Cancel;
                        Close();
                    }
                    else
                    {
                        if (MessageBox.Show(Properties.Resources.ResourceManager.GetString("AskMsgImportOldData"), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (myVault.Initialize(initVaultForm.UserPassword))
                                myVault.ImportOldData(initVaultForm.UserPassword);
                        }
                    }
                }

                if (myVault.CheckVaultKey(Microsoft.VisualBasic.Interaction.InputBox(Properties.Resources.ResourceManager.GetString("AskPassword"), this.Text, "")) == false)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }

                myVault.Load();
                BindGrid();
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        /// <summary>
        /// Data binding
        /// </summary>
        private void BindGrid()
        {
            dataGridViewVault.AutoGenerateColumns = false;

            //create the column programatically
            DataGridViewCell cell = new DataGridViewTextBoxCell();
            DataGridViewTextBoxColumn colUserName = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = Properties.Resources.ResourceManager.GetString("Login"),
                DataPropertyName = "UserName"
            };
            dataGridViewVault.Columns.Add(colUserName);

            DataGridViewTextBoxColumn colPassword = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = Properties.Resources.ResourceManager.GetString("Password"),
                DataPropertyName = "Password"
            };
            dataGridViewVault.Columns.Add(colPassword);

            DataGridViewTextBoxColumn colData = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = Properties.Resources.ResourceManager.GetString("Data"),
                DataPropertyName = "Data"
            };
            dataGridViewVault.Columns.Add(colData);

            DataGridViewTextBoxColumn colKeyword = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = Properties.Resources.ResourceManager.GetString("Keyword"),
                DataPropertyName = "Keyword"
            };
            dataGridViewVault.Columns.Add(colKeyword);

            dataGridViewVault.DataSource = new List<MyPassword>(myVault);
            dataGridViewVault.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        /// <summary>
        /// Import from firefox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonShowImportData_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (MyPassword myPassword in myVault.GetMyPasswordsFromBrowsers())
                    checkedListBoxMdpFireFox.Items.Add(myPassword, true);
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        /// <summary>
        /// Import selected firefox password in my vault
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonImportData_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewVault.DataSource = null;
                foreach (MyPassword item in checkedListBoxMdpFireFox.Items)
                    myVault.Add(item);

                RefreshList();

                MessageBox.Show(Properties.Resources.ResourceManager.GetString("ConfirmMsgImport"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
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

        /// <summary>
        /// Auto completion (3 letters minimum)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxFilter.Text.Length < 3)
                    dataGridViewVault.DataSource = new List<MyPassword>(myVault);
                else
                {
                    List<MyPassword> filterList = new List<MyPassword>();
                    foreach (MyPassword myPassword in myVault)
                    {
                        if (myPassword.ToStr().Contains(textBoxFilter.Text))
                            filterList.Add(myPassword);
                    }

                    dataGridViewVault.DataSource = filterList;
                }
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        /// <summary>
        /// Save password into file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                myVault.Save();
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        /// <summary>
        /// Delete a password in vault
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridViewVault.SelectedRows)
                {
                    MyPassword selectedPwd = (MyPassword)row.DataBoundItem;
                    foreach (MyPassword mp in myVault)
                    {
                        if (mp.ToStr() == selectedPwd.ToStr())
                        {
                            myVault.Remove(mp);
                            break;
                        }
                    }
                }

                RefreshList();
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        /// <summary>
        /// Add password in vault
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MyPassword myPassword = new MyPassword()
                {
                    UserName = textBoxMdpLogin.Text,
                    Password = textBoxMdpPassword.Text,
                    Data = textBoxMdpData.Text,
                    Keyword = textBoxMdpKeyword.Text
                };

                myVault.Add(myPassword);

                RefreshList();

                textBoxMdpLogin.Text = "";
                textBoxMdpPassword.Text = "";
                textBoxMdpData.Text = "";
                textBoxMdpKeyword.Text = "";

                MessageBox.Show(Properties.Resources.ResourceManager.GetString("ConfirmMsgAddDone"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        private void RefreshList()
        {
            myVault.Save();
            myVault.Load();
            dataGridViewVault.DataSource = null;
            textBoxFilter.Text = "";
            dataGridViewVault.DataSource = new List<MyPassword>(myVault);
        }
    }
}
