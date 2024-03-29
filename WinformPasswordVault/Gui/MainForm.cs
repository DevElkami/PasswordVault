﻿using NLog;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                    MaterialDialog materialDialog = new(this, this.Text, Properties.Resources.ResourceManager.GetString("AskMsgExitWithoutSave"), Properties.Resources.ResourceManager.GetString("ButtonYes"), true, Properties.Resources.ResourceManager.GetString("ButtonNo"), true);
                    if (materialDialog.ShowDialog(this) != DialogResult.OK)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        this.Text = this.Text.TrimEnd('*');
                    }
                }
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

        #region Hotkey / shortcut
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs e = new KeyEventArgs(keyData);
            if (e.Control && e.KeyCode == Keys.S)
            {
                buttonSave_Click(materialButtonSave, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);

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

                    MaterialDialog materialDialog = new(this, this.Text, Properties.Resources.ResourceManager.GetString("AskMsgDeletePassword") + Environment.NewLine + selectedPwd.ToString(), Properties.Resources.ResourceManager.GetString("ButtonYes"), true, Properties.Resources.ResourceManager.GetString("ButtonNo"), true);
                    if (materialDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        myVault.Remove(selectedPwd);

                        // Little hack to force refresh
                        materialTextBoxFilter.Text += " ";
                        materialTextBoxFilter.Text.TrimEnd(' ');

                        MaterialSnackBar SnackBarMessage = new(Properties.Resources.ResourceManager.GetString("MsgRowDeleted"), "OK", true);
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
                materialButtonSave.Visible = true;
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        /// <summary>
        /// Import from browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonShowImportData_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialogCsvFiles = new()
                {
                    FileName = Properties.Resources.ResourceManager.GetString("SelectCSVFile"),
                    Filter = "Csv (*.csv)|*.csv",
                    Title = Properties.Resources.ResourceManager.GetString("DlgOpenFileTitle"),
                };

                if (openFileDialogCsvFiles.ShowDialog() == DialogResult.OK)
                {
                    foreach (MyPassword myPassword in MyVault.GetMyPasswordsFromBrowsers(openFileDialogCsvFiles.FileName))
                    {
                        MaterialCheckBox materialCheckBox = new MaterialCheckBox();
                        materialCheckBox.DataContext = myPassword;
                        materialCheckBox.Text = myPassword.ToStr();
                        materialCheckBox.UseAccentColor = true;
                        materialCheckBox.Checked = true;
                        materialCheckListBoxBrowserData.Items.Add(materialCheckBox);
                    }
                }
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
                MessageBox.Show(except.Message);
            }
        }

        /// <summary>
        /// Import selected password in my vault
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonImportData_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (MaterialCheckBox materialCheckBox in materialCheckListBoxBrowserData.Items)
                {
                    if (materialCheckBox.Checked)
                        myVault.Add(materialCheckBox.DataContext as MyPassword);
                }

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
                if (materialButtonSave.Visible)
                {
                    this.Text = this.Text.TrimEnd('*');
                    myVault.Save();
                    materialButtonSave.Visible = false;

                    MaterialSnackBar SnackBarMessage = new(Properties.Resources.ResourceManager.GetString("MsgFileSave"), "OK", true);
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
                    UserName = materialTextBoxEditLogin.Text,
                    Password = materialTextBoxEditPassword.Text,
                    Data = materialTextBoxEditUrl.Text,
                    Keyword = materialTextBoxEditKeyword.Text
                };

                myVault.Add(myPassword);

                materialTextBoxEditLogin.Text = "";
                materialTextBoxEditPassword.Text = "";
                materialTextBoxEditUrl.Text = "";
                materialTextBoxEditKeyword.Text = "";

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
