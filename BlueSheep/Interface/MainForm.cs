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
    public partial class MainForm : MetroFramework.Forms.MetroForm
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
            string applicationDataPath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
            string blueSheepPath = Path.Combine (applicationDataPath, "BlueSheep");
            if (!Directory.Exists(blueSheepPath))
                Directory.CreateDirectory(blueSheepPath);
            if (!Directory.Exists(Path.Combine(blueSheepPath, "Accounts")))
                Directory.CreateDirectory(Path.Combine(blueSheepPath, "Accounts")).Attributes = FileAttributes.Normal;
            if (!Directory.Exists(Path.Combine(blueSheepPath, "Groups")))
                Directory.CreateDirectory(Path.Combine(blueSheepPath, "Groups")).Attributes = FileAttributes.Normal;
            if (!Directory.Exists(Path.Combine(blueSheepPath, "Temp")))
                Directory.CreateDirectory(Path.Combine(blueSheepPath, "Temp")).Attributes = FileAttributes.Normal;
            if (!Directory.Exists(Path.Combine(blueSheepPath, "Paths")))
                Directory.CreateDirectory(Path.Combine(blueSheepPath, "Paths")).Attributes = FileAttributes.Normal;
            if (!Directory.Exists(Path.Combine(blueSheepPath, "IAs")))
                Directory.CreateDirectory(Path.Combine(blueSheepPath, "IAs")).Attributes = FileAttributes.Normal;
            if (!Directory.Exists(Path.Combine(blueSheepPath, "Logs")))
                Directory.CreateDirectory(Path.Combine(blueSheepPath, "Logs")).Attributes = FileAttributes.Normal;

            string bsConfPath = Path.Combine (blueSheepPath, "bs.conf");
            if (File.Exists(bsConfPath))
            {
                StreamReader sr = new StreamReader(bsConfPath);
                string path = sr.ReadLine();
                if (Directory.Exists(Path.Combine(path, "app", "content", "maps")))
                    ActualMainForm.DofusPath = path;
                else
                {
                    sr.Close();
                    DofusPathForm frm = new DofusPathForm(ActualMainForm);             
                    frm.ShowDialog();
                }
                
            }
            else
            {
                DofusPathForm frm = new DofusPathForm(ActualMainForm);
                frm.ShowDialog();
            }


            FileInfo fileInfo = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Logs.txt");
            fileInfo.Delete();
            using (fileInfo.Create())
            {
            }

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
            else if (File.Exists(bsConfPath))
            {
                List<string> PaysList = new List<string>();
                PaysList.AddRange(new List<string>() { "fr", "en", "ja", "es", "de" });
                foreach (string pays in PaysList)
                {
                    string combinedPath = Path.Combine(ActualMainForm.DofusPath, "app", "data", "i18n", "i18n_" + pays + ".d2i");
                    if (File.Exists(combinedPath))
                    {
                        i18NFileAccessor.Init(combinedPath);
                        break;
                    }
                }
                I18N i18N = new I18N(i18NFileAccessor);
                GameData.Init(Path.Combine (ActualMainForm.DofusPath, "app","data", "common"));
                MapsManager.Init(Path.Combine(ActualMainForm.DofusPath, "app", "content", "maps"));

            }
            //else
            //{
            //    i18NFileAccessor.Init(Path.Combine(ActualMainForm.DofusPath, "app", "data", "i18n", "i18n_fr.d2i"));
            //    I18N i18N = new I18N(i18NFileAccessor);
            //    GameData.Init(@"D:\Dofus2\app\data\common");
            //    MapsManager.Init(@"D:\Dofus2\app\content\maps");
            //}
            IntelliSense.InitMonsters();
            IntelliSense.InitItems();
            
        }
        
        #endregion

      

        

        
    }
}