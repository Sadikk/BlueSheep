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
    public partial class GroupForm : Form
    {
        public List<AccountUC> listAccounts;
        public List<Account> list;
        public GroupForm()
        {
            InitializeComponent();
        }
        public GroupForm(List<AccountUC> accounts)
        {
            InitializeComponent();
            list = new List<Account>();
            MasterChoice.SelectedIndex = 0;
            MasterChoice.Items.Remove("Blah");
            listAccounts = accounts;
            foreach (AccountUC account in accounts)
            {
                MasterChoice.Items.Add(account.AccountName);
                TabPage tab = new TabPage(account.AccountName);
                AccountTabs.TabPages.Add(tab);
                tab.Controls.Add(account);
                account.Dock = DockStyle.Left;
                account.Show();
                list.Add(new Account(account.AccountName, account.AccountPassword));
                account.Init();
            }
        }
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
                        account.MyGroup = new Group(listAccounts, null);
                    }
                }
                if (item == MasterChoice.SelectedText)
                {
                    foreach (AccountUC account in listAccounts)
                    {
                        if (item == account.AccountName)
                        {
                            account.IsSlave = false;
                            account.IsMaster = true;
                            account.MyGroup = new Group(listAccounts, null);
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
    }
}