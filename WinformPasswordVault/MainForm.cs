﻿using NLog;
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
        private MyVault myVault = new();

        public MainForm()
        {
            try
            {
                InitializeComponent();

                MaterialSkinManager.Instance.AddFormToManage(this);
                this.FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_None;

                poisonGridVault.BackgroundColor = MaterialSkinManager.Instance.BackdropColor;
                poisonGridVault.Font = new Font("Segoe UI", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
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

            poisonGridVault.DataSource = new List<MyPassword>(myVault);
            poisonGridVault.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
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
                poisonGridVault.DataSource = null;
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
                    poisonGridVault.DataSource = new List<MyPassword>(myVault);
                else
                {
                    List<MyPassword> filterList = new List<MyPassword>();
                    foreach (MyPassword myPassword in myVault)
                    {
                        if (myPassword.ToStr().Contains(textBoxFilter.Text))
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
                foreach (DataGridViewRow row in poisonGridVault.SelectedRows)
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
            poisonGridVault.DataSource = null;
            textBoxFilter.Text = "";
            poisonGridVault.DataSource = new List<MyPassword>(myVault);
        }
    }
}
