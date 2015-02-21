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
        #region Attributs
        private string m_DofusPath;
        public string Lang = "FR";
        #endregion

        #region Propriétés
        public string DofusPath
        {
            get { return m_DofusPath; }
            set { m_DofusPath = value; }
        }
        #endregion

        #region Propriétés
        public static MainForm ActualMainForm { get; set; }
        #endregion

        #region Constructeurs
        public MainForm(int nbPremium, string version)
        {
            InitializeComponent();
            ActualMainForm = this;
            this.Text = "BlueSheep - " + version;
            CheckBlueSheepDatas();
            int jours = nbPremium - (nbPremium % 24);
            int heures = nbPremium % 24;
        }
        #endregion

        #region Méthodes publiques
        

        public void AddForm(Form frm)
        {

                if (((((this != null)) && !this.IsDisposed) && (((frm != null)) && !frm.IsDisposed)))
                {
                    frm.MdiParent = this;
                }

        }

   
        #endregion

        #region Méthodes interfaces

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
        #endregion

        #region Méthodes privées
        private static void CheckBlueSheepDatas()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep");
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Accounts"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Accounts");
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Groups"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Groups");
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp");
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\bs.conf"))
            {
                StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\bs.conf");
                ActualMainForm.DofusPath = sr.ReadLine();
                sr.Close();
            }
            else
            {
                //path = @"D:\Dofus2\app\data\i18n\i18n_fr.d2i";
                DofusPathForm frm = new DofusPathForm(ActualMainForm);
                frm.ShowDialog();
            }
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Paths"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Paths");
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\IAs"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\IAs");
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
                foreach (string pays_loopVariable in PaysList)
                {
                    string pays = pays_loopVariable;
                    if (File.Exists(ActualMainForm.DofusPath + @"\app\data\i18n\i18n_" + pays + ".d2i"))
                    {
                        i18NFileAccessor.Init(ActualMainForm.DofusPath + @"\app\data\i18n\i18n_" + pays + ".d2i");
                        break;
                    }
                }
                //i18NFileAccessor.Init(ActualMainForm.DofusPath + @"\app\data\i18n\i18n_fr.d2i");
                I18N i18N = new I18N(i18NFileAccessor);
                GameData.Init(ActualMainForm.DofusPath + @"\app\data\common");
                MapsManager.Init(ActualMainForm.DofusPath + @"\app\content\maps");
                
            }
            else
            {
                //path = @"D:\Dofus2\app\data\i18n\i18n_fr.d2i";
                //DofusPathForm frm = new DofusPathForm(ActualMainForm);
                //frm.Show();
                i18NFileAccessor.Init(ActualMainForm.DofusPath + @"\app\data\i18n\i18n_fr.d2i");
                I18N i18N = new I18N(i18NFileAccessor);
                GameData.Init(@"D:\Dofus2\app\data\common");
                MapsManager.Init(@"D:\Dofus2\app\content\maps");
            }
            IntelliSense.InitMonsters();
            IntelliSense.InitItems();
            
        }

        
        #endregion

        private void LanguageChoice_SelectedTextChanged(object sender, EventArgs e)
        {
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

        

        
    }
}