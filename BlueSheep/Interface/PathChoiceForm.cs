using BlueSheep.Interface.Text;
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
    public partial class PathChoiceForm : MetroFramework.Forms.MetroForm
    {
        /// <summary>
        /// Bot's path choose form.
        /// </summary>

        #region Fields
        private AccountUC Account;
        #endregion

        #region Constructors
        public PathChoiceForm(AccountUC account)
        {
            InitializeComponent();
            Account = account;
            switch (MainForm.ActualMainForm.Lang)
            {
                case "EN":
                    this.Text = "Path manager";
                    DelBt.Text = "Delete";
                    LoadBt.Text = "Load";
                    AddBt.Text = "Add";
                    FilesList.Columns[0].Text = "Name";
                    FilesList.Columns[4].Text = "Author";
                    break;
            }
        }
        #endregion

        #region Interface methods
        private void PathChoiceForm_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void AddBt_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string combinedPath = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "BlueSheep", "Paths", openFileDialog1.SafeFileName);
                if (!File.Exists(combinedPath))
                {
                    File.Copy(openFileDialog1.FileName, combinedPath);
                    FilesList.Items.Clear();
                    Init();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Le trajet a déjà été ajouté !");
                }
            }
        }

        private void LoadBt_Click(object sender, EventArgs e)
        {
            LaunchPath(null, null);
        }

        private void DelBt_Click(object sender, EventArgs e)
        {
            if (FilesList.SelectedItems.Count > 0)
            {
                string path = FilesList.SelectedItems[0].SubItems[5].Text;
                File.Delete(path);
                FilesList.Items.Remove(FilesList.SelectedItems[0]);
            }
        }
        #endregion

        #region Private methods
        private void Init()
        {
            string combinedPath = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "BlueSheep", "Paths");
            openFileDialog1.InitialDirectory = combinedPath;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(combinedPath);
            if (di.GetFiles().Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show("Aucun trajet, veuillez en télécharger sur le forum ou créer le vôtre :) ");
            }
            else
            {
                foreach (System.IO.FileInfo file in di.GetFiles())
                {
                    LoadPath(file);
                }
            }
        }

        private void LoadPath(FileInfo fi)
        {
            StreamReader sr = new StreamReader(fi.FullName);
            string line = null;
            string[] infos = new string[6];
            for (int i = 0; i < 5; i++)
            {
                line = sr.ReadLine();
                if (line == null || line.Length < 5)
                {
                    MessageBox.Show("Malformated header. Check your path's syntax. " + fi.FullName);
                    return;
                }
                line.Replace(" ", "");
                switch (i)
                {
                    case 0:
                        infos[i] = line.Remove(0, 5).Trim();
                        //enleve @NAME
                        break;
                    case 1:
                        infos[i] = line.Remove(0, 9).Trim();
                        //@LOCATION
                        break;
                    case 2:
                        infos[i] = line.Remove(0, 5).Trim();
                        //enleve @TYPE
                        break;
                    case 3:
                        infos[i] = line.Remove(0, 8).Trim();
                        //enleve @VERSION
                        break;
                    case 4:
                        infos[i] = line.Remove(0, 9).Trim();
                        //enleve @AUTHOR
                        break;
                }
            }
            infos[5] = fi.FullName;
            sr.Close();
            FilesList.Items.Add(infos[0]).SubItems.AddRange(new string[] {
		infos[1],
		infos[2],
		infos[3],
		infos[4],
        infos[5]
	});
        }


        private void LaunchPath(object sender, EventArgs e)
        {
            if (FilesList.SelectedItems.Count > 0 && Account != null)
            {
                Account.Path = new Core.Path.PathManager(Account, FilesList.SelectedItems[0].SubItems[5].Text, FilesList.SelectedItems[0].SubItems[0].Text);
                Account.Log(new BotTextInformation("Trajet chargé : " + FilesList.SelectedItems[0].Text),0);
                Account.Path.pathBot = FilesList.SelectedItems[0].Text;
                if (Account.Fight == null)
                {
                    Account.Log(new ErrorTextInformation("WARNING : T'as chargé aucune IA, fait gaffe mon coco :p"), 0);
                }
                this.Close();
            }
        }
        #endregion



    }
}
