using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueSheep.Core;
using BlueSheep.Engine;
using BlueSheep.Common.Types;
using BlueSheep.AccountsManager;
using System.Diagnostics;
using BlueSheep.Engine.Constants;
namespace BlueSheep.Interface
{
    public partial class GestAccounts : MetroFramework.Forms.MetroForm
    {
        /// <summary>
        /// Account manager.
        /// </summary>

        #region Fields
        MainForm m_Form;
        #endregion

        #region Constructors
        public GestAccounts(MainForm mainfrm)
        {
            InitializeComponent();
            switch (mainfrm.Lang)
            {
                case "FR":
                    listViewAccounts.Columns.Add("Nom", 200, HorizontalAlignment.Center);
                    listViewAccounts.Columns.Add("Mot de passe", 0, HorizontalAlignment.Center);
                    break;
                case "EN":
                    listViewAccounts.Columns.Add("Username", 200, HorizontalAlignment.Center);
                    listViewAccounts.Columns.Add("Password", 0, HorizontalAlignment.Center);
                    DelBt.Text = "Delete";
                    LaunchAccountsSelecBt.Text = "Launch selected accounts";
                    this.Text = "Accounts manager";
                    this.sadikTabControl1.TabPages[0].Text = "Launch";
                    this.sadikTabControl1.TabPages[1].Text = "Add";
                    AddBt.Text = "Add";
                    sadikLabel1.Text = "Username";
                    sadikLabel2.Text = "Password";
                    break;
                case "ES":
                    listViewAccounts.Columns.Add("Nombre de cuenta", 200, HorizontalAlignment.Center);
                    listViewAccounts.Columns.Add("Contraseña", 0, HorizontalAlignment.Center);
                    DelBt.Text = "Suprimir";
                    LaunchAccountsSelecBt.Text = "Lanzar las cuentas seleccionadas";
                    this.Text = "Gestión de cuentas";
                    this.sadikTabControl1.TabPages[0].Text = "Lanzar";
                    this.sadikTabControl1.TabPages[1].Text = "Añadir";
                    AddBt.Text = "Añadir";
                    sadikLabel1.Text = "Nombre de cuenta";
                    sadikLabel2.Text = "Contraseña";
                    break;
                case "PT":
                    listViewAccounts.Columns.Add("Nome de conta", 200, HorizontalAlignment.Center);
                    listViewAccounts.Columns.Add("Contra-senha", 0, HorizontalAlignment.Center);
                    DelBt.Text = "Escamotear";
                    LaunchAccountsSelecBt.Text = "Lance as contas selecionadas";
                    this.Text = "Administração de contas";
                    this.sadikTabControl1.TabPages[0].Text = "Lance";
                    this.sadikTabControl1.TabPages[1].Text = "Acrescentar";
                    AddBt.Text = "Acrescentar";
                    sadikLabel1.Text = "Nome de conta";
                    sadikLabel2.Text = "Contra-senha";
                    break;
            }
            listViewAccounts.Columns[1].Width = 0;

            LoadAccounts();
            m_Form = mainfrm;
        }
        #endregion

        #region Interface methods

        private void LaunchAccountsSelecBt_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem account in listViewAccounts.SelectedItems)
            {
                if (!IsMITM.Checked)
                {
                    AccountFrm frm = new AccountFrm(account.SubItems[0].Text, account.SubItems[1].Text, true);
                    frm.Show();
                    MainForm.ActualMainForm.AddForm(frm);
                }
                else
                {
                    // TODO : Automate the dll injection
                    Process proc = new Process();
                    string directoryPath = System.IO.Path.Combine (MainForm.ActualMainForm.DofusPath, "app", "Dofus.exe");
                    proc.StartInfo.FileName = directoryPath;
                    proc.Start();
                    //BlueSheep.Engine.Constants.DllInjector Injector = new Engine.Constants.DllInjector();
                    //String path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\No.Ankama.dll";
                    //List<Process> lp = Process.GetProcessesByName("Dofus").ToList();
                    //foreach (Process p in lp)
                    //{
                    //    Engine.Constants.DllInjectionResult result = Injector.Inject(p.ProcessName, path);
                    //    if (result != Engine.Constants.DllInjectionResult.Success)
                    //    {
                    //        System.Windows.Forms.MessageBox.Show("Erreur lors de l'injection.");
                    //        proc.Kill();
                    //        this.Close();
                    //    }
                    //}
                    //DllInjector.GetInstance.Inject(proc.ProcessName, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\No.Ankama.dll");
                 //BlueSheep.Engine.Constants.DllInjector Injector = new Engine.Constants.D);
                 //   String path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\No.Ankama.dll";
                    
                    
                 //   BlueSheep.Engine.Injection.StartInjection(path, proc.Id);

                    AccountFrm frm = new AccountFrm(account.SubItems[0].Text, account.SubItems[1].Text, false);
                    frm.Show();
                    MainForm.ActualMainForm.AddForm(frm);
                }
            }
            this.Close();
        }
        private void buttonAddAccount_Click(object sender, EventArgs e)
        {
            if ((textBoxAccountName.Text == string.Empty) || (textBoxAccountName.Text == "Entrez le nom de compte..."))
                MessageBox.Show("Merci d'entrer un nom de compte.", "BS Error");
            else if ((textBoxPassword.Text == string.Empty) || (textBoxPassword.Text == "Entrez le mot de passe..."))
                MessageBox.Show("Merci d'entrer un mot de passe.", "BS Error");
            else
            {
                foreach (ListViewItem bot in listViewAccounts.Items)
                {
                    if (bot.Text == textBoxAccountName.Text)
                    {
                        MessageBox.Show("Ce compte a déjà été ajouté.", "BS Error");
                        textBoxAccountName.Text = string.Empty;
                        textBoxPassword.Text = string.Empty;
                        return;
                    }
                }
                string[] row1 = { textBoxAccountName.Text, textBoxPassword.Text };
                ListViewItem li = new ListViewItem(row1);
                listViewAccounts.Items.Add(li);
                AccountsFileInteractions accountsFileInteractions = new AccountsFileInteractions();
                List<Bot> listaccount = new List<Bot>();
                foreach (ListViewItem item in listViewAccounts.Items)
                {
                    listaccount.Add(new Bot(new Account(item.SubItems[0].Text, item.SubItems[1].Text)));
                }
                accountsFileInteractions.SaveAccountsInfos(listaccount);
                textBoxAccountName.Text = string.Empty;
                textBoxPassword.Text = string.Empty;
                listViewAccounts.Items[0].Focused = true;
                listViewAccounts.Items[0].Selected = true;
            }
        }
        private void textBoxPassword_GotFocus(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "Entrez le mot de passe...")
            {
                // Vide la TextBox de commande
                textBoxPassword.Text = string.Empty;
                // Change la mise en forme
                textBoxPassword.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            }
        }
        private void textBoxPassword_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonAddAccount_Click(null, null);
        }
        private void textBoxPassword_LostFocus(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "")
            {
                // Réinitialise le text
                textBoxPassword.Text = "Entrez le mot de passe...";
                // Réinitialise la mise en forme
                textBoxPassword.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            }
        }
        private void textBoxAccountName_GotFocus(object sender, EventArgs e)
        {
            if (textBoxAccountName.Text == "Entrez le nom de compte...")
            {
                // Vide la TextBox de commande
                textBoxAccountName.Text = string.Empty;
                // Change la mise en forme
                textBoxAccountName.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            }
        }
        private void textBoxAccountName_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Touche "Enter" saisie
                buttonAddAccount_Click(null, null);
        }
        private void textBoxAccountName_LostFocus(object sender, EventArgs e)
        {
            if (textBoxAccountName.Text == "")
            {
                // Réinitialise le text
                textBoxAccountName.Text = "Entrez le nom de compte...";
                // Réinitialise la mise en forme
                textBoxAccountName.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            }
        }
        private void DelBt_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewAccounts.SelectedItems.Count; i++)
            //parcours des comptes sélectionnés
            {
                ListViewItem listViewItem2 = listViewAccounts.SelectedItems[i];
                // Sauvegarde des comptes
                AccountsFileInteractions accountsFileInteractions = new AccountsFileInteractions();
                List<Bot> listaccount = new List<Bot>();
                foreach (ListViewItem item in listViewAccounts.Items)
                {
                    listaccount.Add(new Bot(new Account(item.SubItems[0].Text, item.SubItems[1].Text)));
                }
                accountsFileInteractions.SaveAccountsInfos(listaccount);
                // suppression de l'interface
                listViewAccounts.Items.Remove(listViewItem2);
            }
        }

        private void IsMITM_CheckedChanged(object sender)
        {
            MessageBox.Show("La fonction MITM est encore experimentale.");
        }

        #endregion

        #region Private methods
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