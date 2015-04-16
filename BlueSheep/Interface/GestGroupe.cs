using BlueSheep.AccountsManager;
using BlueSheep.Common.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlueSheep.Interface
{
    public partial class GestGroupe : MetroFramework.Forms.MetroForm
    {
        /// <summary>
        /// Group manager.
        /// </summary>

        #region Fields
        List<Group> groups = new List<Group>();
        #endregion

        #region Constructors
        public GestGroupe()
        {
            InitializeComponent();
            LoadGroups();
            LoadAccounts();
            listViewAccounts.HideSelection = true;
            listViewGroups.HideSelection = true;

            switch (MainForm.ActualMainForm.Lang)
            {
                case "FR":
                    listViewAccounts.Columns.Add("Nom", 200, HorizontalAlignment.Center);
                    listViewAccounts.Columns.Add("Mot de passe", 0, HorizontalAlignment.Center);
                    listViewGroups.Columns.Add("Nom", -2, HorizontalAlignment.Center);
                    break;
                case "EN":
                    listViewAccounts.Columns.Add("Username", 200, HorizontalAlignment.Center);
                    listViewAccounts.Columns.Add("Password", 0, HorizontalAlignment.Center);
                    listViewGroups.Columns.Add("Name", -2, HorizontalAlignment.Center);
                    DelBt.Text = "Delete";
                    LaunchGroupsSelecBt.Text = "Launch selected groups";
                    this.Text = "Groups manager";
                    this.sadikTabControl1.TabPages[0].Text = "Launch";
                    this.sadikTabControl1.TabPages[1].Text = "Create";
                    AddBt.Text = "Create";
                    break;
                default:
                    listViewAccounts.Columns.Add("Username", 200, HorizontalAlignment.Center);
                    listViewAccounts.Columns.Add("Password", 0, HorizontalAlignment.Center);
                    listViewGroups.Columns.Add("Name", -2, HorizontalAlignment.Center);
                    DelBt.Text = "Delete";
                    LaunchGroupsSelecBt.Text = "Launch selected groups";
                    this.Text = "Groups manager";
                    this.sadikTabControl1.TabPages[0].Text = "Launch";
                    this.sadikTabControl1.TabPages[1].Text = "Create";
                    AddBt.Text = "Create";
                    break;
            }
            
        }
        #endregion

        #region Interface methods
        private void LaunchGroupsSelecBt_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewGroups.SelectedItems)
            {
                Group group = SearchGroups(item.Text);
                List<AccountUC> listaccounts = new List<AccountUC>();
                //foreach (AccountUC account in group.accounts)
                //{
                //    //AccountUC acc = new AccountUC(account.Name, account.Password);
                //    listaccounts.Add(account);
                //}
                GroupForm frm = new GroupForm(group.accounts, group.name);
                //foreach (AccountUC account in frm.listAccounts)
                //{
                //    account.Init();
                //}
                frm.Show();
                MainForm.ActualMainForm.AddForm(frm);
            }
            this.Close();
        }

        private void DelBt_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewGroups.SelectedItems.Count; i++)
            //parcours des groupes sélectionnés
            {
                ListViewItem listViewItem2 = listViewGroups.SelectedItems[i];
                string ApplicationDataPath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
                string combinedPath = System.IO.Path.Combine (ApplicationDataPath, "BlueSheep", "Groups", listViewItem2.Text + ".bs");
                File.Delete(combinedPath);
                listViewGroups.Items.Remove(listViewItem2);
            }
        }

        private void AddBt_Click(object sender, EventArgs e)
        {
            if (listViewAccounts.SelectedItems.Count > 0)
            {
                List<AccountUC> listaccounts = new List<AccountUC>();
                foreach (ListViewItem account in listViewAccounts.SelectedItems)
                {
                    listaccounts.Add(new AccountUC(account.SubItems[0].Text, account.SubItems[1].Text, false));
                }
                if (NameBox.Text.Length > 0)
                {
                    AccountsFileInteractions accountsFileInteractions = new AccountsFileInteractions();
                    accountsFileInteractions.SaveGroup(listaccounts, NameBox.Text + ".bs");
                    listViewGroups.Items.Add(NameBox.Text);
                    groups.Add(new Group(listaccounts, NameBox.Text));
                    listViewAccounts.SelectedItems.Clear();
                    NameBox.Clear();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Merci de rajouter un nom pour le groupe. Merci pour eux. :3");
                }
            }
        }
        #endregion

        #region Private methods
        private Group SearchGroups(string groupname)
        {
            foreach (Group group in groups)
            {
                if (group.name == groupname)
                    return group;
            }
            return null;
        }

        private void LoadGroups()
        {
            AccountsFileInteractions accountsFileInteractions = new AccountsFileInteractions();

            accountsFileInteractions.RecoverGroups();

            foreach (Group Groupobject in accountsFileInteractions.Groups)
            {
                string[] row1 = { Groupobject.name };
                ListViewItem li = new ListViewItem(row1);
                listViewGroups.Items.Add(li);
                groups.Add(Groupobject);
            }
        }

        

        private void LoadAccounts()
        {
            AccountsFileInteractions accountsFileInteractions = new AccountsFileInteractions();
            accountsFileInteractions.RecoverAccountsInfos();
            foreach (Account accountObject in accountsFileInteractions.Accounts)
            {
                string[] row1 = { accountObject.Name, accountObject.Password };
                ListViewItem li = new ListViewItem(row1);
                listViewAccounts.Items.Add(li);
            }
        }

        #endregion
    }
}
