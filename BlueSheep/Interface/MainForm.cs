using BlueSheep.AccountsManager;
using BlueSheep.Common.Data;
using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.Types;
using BlueSheep.Core;
using BlueSheep.Data.D2p;
using BlueSheep.Engine.Constants;
using BlueSheep.Engine.Network;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace BlueSheep.Interface
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Main form.
        /// </summary>

        #region Fields
        private string m_DofusPath;
        public string Lang = "FR";
        #endregion

        #region Properties
        public string DofusPath
        {
            get { return m_DofusPath; }
            set { m_DofusPath = value; }
        }
        public static MainForm ActualMainForm { get; set; }
        #endregion

        #region Constructors
        public MainForm(string version)
        {
            InitializeComponent();
            ActualMainForm = this;
            this.Text = "BlueSheep - " + version;
            CheckBlueSheepDatas();
        }
        #endregion

        #region Public methods
        
        public void AddForm(Form frm)
        {
            // Add the form in the main form using MDI layers.
                if (((((this != null)) && !this.IsDisposed) && (((frm != null)) && !frm.IsDisposed)))
                {
                    frm.MdiParent = this;
                }

        }
        #endregion

        #region Interface methods

        private void AccountsBt_Click(object sender, EventArgs e)
        {
            GestAccounts frm = new GestAccounts(this);
            frm.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void GroupsBt_Click(object sender, EventArgs e)
        {
            GestGroupe frm = new GestGroupe();
            frm.Show();
        }

        private void LanguageChoice_SelectedTextChanged(object sender, EventArgs e)
        {
            /* Switch the language */
            Lang = LanguageChoice.Text;
            switch (Lang)
            {
                case "EN":
                    AccountsBt.Text = "Accounts";
                    GroupsBt.Text = "Groups";
                    break;
                case "FR":
                    AccountsBt.Text = "Comptes";
                    GroupsBt.Text = "Groupes";
                    break;
                case "ES":
                    AccountsBt.Text = "Cuentas";
                    GroupsBt.Text = "Grupos";
                    break;
                case "PT":
                    AccountsBt.Text = "Contas";
                    GroupsBt.Text = "Grupos";
                    break;
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(3000);
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }
        #endregion

        #region Private methods
        private static void CheckBlueSheepDatas()
        {
            // Create the BlueSheep needed folders
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep");
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Accounts"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Accounts");
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Groups"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Groups");
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp");
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Paths"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Paths");
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\IAs"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\IAs");


            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\bs.conf"))
            {
                StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\bs.conf");
                ActualMainForm.DofusPath = sr.ReadLine();
                sr.Close();
            }
            else
            {
                DofusPathForm frm = new DofusPathForm(ActualMainForm);
                frm.ShowDialog();
            }


            //FileInfo fileInfo = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Logs.txt");
            //fileInfo.Delete();
            //using (fileInfo.Create())
            //{
            //}

            //fileInfo = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Packets.txt");
            //fileInfo.Delete();
            //using (fileInfo.Create())
            //{
            //}


            I18NFileAccessor i18NFileAccessor = new I18NFileAccessor();

            if (File.Exists(@"C:\Program Files (x86)\Dofus2\app\data\i18n\i18n_fr.d2i"))
            {
                string path = @"C:\Program Files (x86)\Dofus2\app\data\i18n\i18n_fr.d2i";
                i18NFileAccessor.Init(path);
                I18N i18N = new I18N(i18NFileAccessor);
                GameData.Init(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                    + @"\Dofus2\app\data\common");
                MapsManager.Init(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                    + @"\Dofus2\app\content\maps");
            }
            else if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\bs.conf"))
            {
                List<string> PaysList = new List<string>();
                PaysList.AddRange(new List<string>() { "fr", "en", "ja", "es", "de" });
                foreach (string pays in PaysList)
                {
                    if (File.Exists(ActualMainForm.DofusPath + @"\app\data\i18n\i18n_" + pays + ".d2i"))
                    {
                        i18NFileAccessor.Init(ActualMainForm.DofusPath + @"\app\data\i18n\i18n_" + pays + ".d2i");
                        break;
                    }
                }
                I18N i18N = new I18N(i18NFileAccessor);
                GameData.Init(ActualMainForm.DofusPath + @"\app\data\common");
                MapsManager.Init(ActualMainForm.DofusPath + @"\app\content\maps");
                
            }
            else
            {
                i18NFileAccessor.Init(ActualMainForm.DofusPath + @"\app\data\i18n\i18n_fr.d2i");
                I18N i18N = new I18N(i18NFileAccessor);
                GameData.Init(@"D:\Dofus2\app\data\common");
                MapsManager.Init(@"D:\Dofus2\app\content\maps");
            }
            IntelliSense.InitMonsters();
            IntelliSense.InitItems();
            
        }
        
        #endregion

      

        

        
    }
}