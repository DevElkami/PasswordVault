using NLog;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VaultCore;
using VaultCore.Models;

namespace WinformPasswordVault
{
    public partial class MainForm : MaterialForm
    {
        private readonly MyVault myVault = new();

        #region Form function
        /// <summary>
        /// Main form constructor
        /// </summary>
        public MainForm(ref MyVault vault)
        {
            try
            {
                myVault = vault;
                InitializeComponent();

                MaterialSkinManager.Instance.AddFormToManage(this);
                this.FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_40;

                poisonGridVault.BackgroundColor = MaterialSkinManager.Instance.BackdropColor;
                poisonGridVault.Font = new Font("Segoe UI", 14f, FontStyle.Regular, GraphicsUnit.Pixel);

                poisonContextMenuStripGrid.BackColor = MaterialSkinManager.Instance.BackdropColor;
                poisonContextMenuStripGrid.Font = new Font("Segoe UI", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
                poisonContextMenuStripGrid.ForeColor = Color.White;
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
                myVault.Load();
                BindGrid();

                RegisterHotKey(Handle, HOTKEY_ID_SAVE, HOTKEY_MODIFIER_CTRL, Keys.S.GetHashCode());
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        /// <summary>
        /// Manage form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.Text.Contains("*"))
                {
                    MaterialDialog materialDialog = new(this, this.Text, "Voulez-vous quitter sans sauvegarder ?", DialogResult.Yes.ToString(), true, DialogResult.Cancel.ToString());
                    if (materialDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                UnregisterHotKey(Handle, HOTKEY_ID_SAVE);
                poisonGridVault.DataSource = null;
                myVault.ListChanged -= MyVault_ListChanged;
                myVault.Clear();
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }
        #endregion

        #region Hotkey
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int HOTKEY_ID_SAVE = 1;

        const int HOTKEY_MODIFIER_CTRL = 2;

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == 0x0312)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case HOTKEY_ID_SAVE: buttonSave_Click(materialButtonSave, null); break;
                    }
                }

                base.WndProc(ref m);
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
            }
        }
        #endregion

        #region Grid function
        /// <summary>
        /// Data binding
        /// </summary>
        private void BindGrid()
        {
            poisonGridVault.AutoGenerateColumns = false;

            DataGridViewCell cell = new DataGridViewTextBoxCell();
            DataGridViewTextBoxColumn colUserName = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = Properties.Resources.ResourceManager.GetString("Login"),
                DataPropertyName = "UserName"
            };
            poisonGridVault.Columns.Add(colUserName);

            DataGridViewTextBoxColumn colPassword = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = Properties.Resources.ResourceManager.GetString("Password"),
                DataPropertyName = "Password"
            };
            poisonGridVault.Columns.Add(colPassword);

            DataGridViewTextBoxColumn colData = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = Properties.Resources.ResourceManager.GetString("Data"),
                DataPropertyName = "Data"
            };
            poisonGridVault.Columns.Add(colData);

            DataGridViewTextBoxColumn colKeyword = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Value",
                HeaderText = Properties.Resources.ResourceManager.GetString("Keyword"),
                DataPropertyName = "Keyword"
            };
            poisonGridVault.Columns.Add(colKeyword);

            poisonGridVault.DataSource = myVault;
            poisonGridVault.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            materialTextBoxFilter.Visible = true;
            myVault.ListChanged += MyVault_ListChanged;
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
                foreach (DataGridViewCell cell in poisonGridVault.SelectedCells)
                {

                    MyPassword selectedPwd = (MyPassword)poisonGridVault.Rows[cell.RowIndex].DataBoundItem;

                    MaterialDialog materialDialog = new(this, this.Text, "Voulez-vous supprimer ce mot de passe ?" + Environment.NewLine + selectedPwd.ToString(), DialogResult.Yes.ToString(), true, DialogResult.Cancel.ToString());
                    if (materialDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        myVault.Remove(selectedPwd);

                        // Little hack to force refresh
                        materialTextBoxFilter.Text += " ";
                        materialTextBoxFilter.Text.TrimEnd(' ');

                        MaterialSnackBar SnackBarMessage = new("Row supprimée", "OK", true);
                        SnackBarMessage.Show(this);
                    }
                    break;
                }
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }
        #endregion

        private void MyVault_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            try
            {
                this.Text = this.Text.TrimEnd('*') + '*';
                materialButtonSave.Enabled = true;
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
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
                foreach (MyPassword item in checkedListBoxMdpFireFox.Items)
                    myVault.Add(item);

                MaterialSnackBar SnackBarMessage = new(Properties.Resources.ResourceManager.GetString("ConfirmMsgImport"), "OK", true);
                SnackBarMessage.Show(this);
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
                if (materialTextBoxFilter.Text.Length < 3)
                    poisonGridVault.DataSource = myVault;
                else
                {
                    List<MyPassword> filterList = new List<MyPassword>();
                    foreach (MyPassword myPassword in myVault)
                    {
                        if (myPassword.ToStr().Contains(materialTextBoxFilter.Text))
                            filterList.Add(myPassword);
                    }

                    poisonGridVault.DataSource = filterList;
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
                if (materialButtonSave.Enabled)
                {
                    this.Text = this.Text.TrimEnd('*');
                    myVault.Save();
                    materialButtonSave.Enabled = false;

                    MaterialSnackBar SnackBarMessage = new("Fichier enregistré", "OK", true);
                    SnackBarMessage.Show(this);
                }
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

                textBoxMdpLogin.Text = "";
                textBoxMdpPassword.Text = "";
                textBoxMdpData.Text = "";
                textBoxMdpKeyword.Text = "";

                MaterialSnackBar SnackBarMessage = new(Properties.Resources.ResourceManager.GetString("ConfirmMsgAddDone"), "OK", true);
                SnackBarMessage.Show(this);
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }
    }
}
