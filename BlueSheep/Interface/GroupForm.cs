using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Common.Types;
using BlueSheep.Engine.Types;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BlueSheep.Interface
{
    public partial class GroupForm : MetroFramework.Forms.MetroForm
    {
        /// <summary>
        /// Container for multiple accountUC.
        /// </summary>

        #region Fields
        public List<AccountUC> listAccounts;
        public List<Account> list;
        #endregion

        #region Constructors
        public GroupForm(List<AccountUC> accounts, string name)
        {
            InitializeComponent();
            this.Text = name;
            list = new List<Account>();
            foreach (AccountUC account in accounts)
            {
                TabPage tab = new TabPage(account.AccountName);
                AccountUC naccount = new AccountUC(account.AccountName, account.AccountPassword, true);
                AccountTabs.TabPages.Add(tab);
                tab.Controls.Add(naccount);
                naccount.Dock = DockStyle.Fill;
                naccount.Show();
                naccount.MyGroup = new Group(accounts, this.Name);
                list.Add(new Account(account.AccountName, account.AccountPassword));
                naccount.Init();
            }
        }
        #endregion

        #region Public Methods
        public void AddMember(string name)
        {
            MasterChoice.Items.Add(name);
        }
        #endregion

        #region Interface methods
        private void MasterChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < MasterChoice.Items.Count; i++)
            {
                string item = (string)MasterChoice.Items[i];
                foreach (AccountUC account in listAccounts)
                {
                    if (item == account.AccountName)
                    {
                        account.IsSlave = true;
                        account.IsMaster = false;
                        account.MyGroup = new Group(listAccounts, this.Name);
                    }
                }
                if (item == (string)MasterChoice.SelectedItem)
                {
                    foreach (AccountUC account in listAccounts)
                    {
                        if (item == account.AccountName)
                        {
                            account.IsSlave = false;
                            account.IsMaster = true;
                            account.MyGroup = new Group(listAccounts, this.Name);
                            account.Log(new BotTextInformation("Je suis le chef de groupe biatch !"), 1);
                            account.Focus();
                            foreach (AccountUC slave in listAccounts)
                            {
                                // Si le compte n'est pas le compte chef
                                if (account.AccountName != slave.AccountName)
                                {
                                    // On l'invite en groupe
                                    Invite(slave.CharacterBaseInformations.name, account);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Dofus group methods
        private void Invite(string name, AccountUC account)
        {
            PartyInvitationRequestMessage msg = new PartyInvitationRequestMessage(name);
            account.SocketManager.Send(msg);
        }
        private void QuitGroup(int partyid, AccountUC account)
        {
            PartyLeaveRequestMessage msg = new PartyLeaveRequestMessage(partyid);
            account.SocketManager.Send(msg);
        }
        #endregion
    }
}