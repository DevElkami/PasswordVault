using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Mdp
{
    public partial class MainForm : Form
    {
        System.ComponentModel.ComponentResourceManager resources = null;

        public MainForm()
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

                resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

                InitializeComponent();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                String mainPwd = Microsoft.VisualBasic.Interaction.InputBox(resources.GetString("labelMsgPassword.Text"), resources.GetString("labelMsgPasswordTitle.Text"), "");
                if (Security.Instance.CheckPassword(mainPwd) == false)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }

                MyVault.Instance.Load();
                BindGrid();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
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
                HeaderText = resources.GetString("labelGridLogin.Text"),
                DataPropertyName = "UserName"
            };
            dataGridViewVault.Columns.Add(colUserName);

            DataGridViewTextBoxColumn colPassword = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = resources.GetString("labelGridPassword.Text"),
                DataPropertyName = "DecryptPwd",
                ReadOnly = true
            };
            dataGridViewVault.Columns.Add(colPassword);

            DataGridViewTextBoxColumn colData = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = resources.GetString("labelGridData.Text"),
                DataPropertyName = "Data"
            };
            dataGridViewVault.Columns.Add(colData);

            DataGridViewTextBoxColumn colKeyword = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = resources.GetString("labelGridKeyword.Text"),
                DataPropertyName = "Keyword"
            };
            dataGridViewVault.Columns.Add(colKeyword);

            dataGridViewVault.DataSource = MyVault.Instance.Vault;
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
                foreach (MyPassword myPassword in FirefoxImporter.Import())
                    checkedListBoxMdpFireFox.Items.Add(myPassword, true);
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
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
                    MyVault.Instance.Vault.Add(item);

                RefreshList();

                MessageBox.Show(resources.GetString("labelMsgImport.Text"), resources.GetString("labelMsgImportTitle.Text"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
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
                    dataGridViewVault.DataSource = MyVault.Instance.Vault;
                else
                {
                    List<MyPassword> filterList = new List<MyPassword>();
                    foreach (MyPassword myPassword in MyVault.Instance.Vault)
                    {
                        if (myPassword.ToStr().Contains(textBoxFilter.Text))
                            filterList.Add(myPassword);
                    }

                    dataGridViewVault.DataSource = filterList;
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
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
                MyVault.Instance.Save();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
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
                    foreach (MyPassword mp in MyVault.Instance.Vault)
                    {
                        if (mp.ToStr() == selectedPwd.ToStr())
                        {
                            MyVault.Instance.Vault.Remove(mp);
                            break;
                        }
                    }
                }

                RefreshList();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
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
                    Password = textBoxMdpPassword.Text.ToSecureString(),
                    Data = textBoxMdpData.Text,
                    Keyword = textBoxMdpKeyword.Text
                };

                MyVault.Instance.Vault.Add(myPassword);

                RefreshList();

                textBoxMdpLogin.Text = "";
                textBoxMdpPassword.Text = "";
                textBoxMdpData.Text = "";
                textBoxMdpKeyword.Text = "";

                MessageBox.Show(resources.GetString("labelMsgAddDone.Text"), resources.GetString("labelMsgAddDoneTitle.Text"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
            }
        }

        private void RefreshList()
        {
            MyVault.Instance.Save();
            MyVault.Instance.Load();
            dataGridViewVault.DataSource = null;
            textBoxFilter.Text = "";
            dataGridViewVault.DataSource = MyVault.Instance.Vault;
        }
    }
}
