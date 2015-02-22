using BlueSheep.Core.Fight;
using ICSharpCode.SharpZipLib.Zip;
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
using System.Xml.Serialization;

namespace BlueSheep.Interface
{
    public partial class IAChoice : Form
    {
        /// <summary>
        /// AI Choice form.
        /// </summary>

        #region Fields
        AccountUC Account;
        #endregion

        #region Constructors
        public IAChoice(AccountUC account)
        {
            InitializeComponent();
            Account = account;
            switch (MainForm.ActualMainForm.Lang)
            {
                case "EN":
                    this.Text = "AI Manager";
                    DelBt.Text = "Delete";
                    LoadBt.Text = "Load";
                    AddBt.Text = "Add";
                    FilesList.Columns[0].Text = "Name";
                    FilesList.Columns[1].Text = "Author";
                    FilesList.Columns[2].Text = "Breed";
                    break;
            }
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\IAs");
            if (di.GetFiles().Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show("Aucune IA, veuillez en télécharger sur le forum ou créer la vôtre :) ");
            }
            else
            {
                foreach (System.IO.FileInfo file in di.GetFiles())
                {
                    if (file.Extension == ".bs")
                        LoadIA(file);
                }
            }
        }
        #endregion

        #region Public methods
        public object DeserializeDisp(string file)
        {
            try
            {
                StreamReader sr = new StreamReader(file);
                XmlSerializer seria = new XmlSerializer(typeof(Display));
                List<string> Infos = new List<string>();
                Display disp = (Display)seria.Deserialize(sr);
                Infos.Add(disp.Name);
                Infos.Add(disp.Author);
                Infos.Add(disp.Race);
                Infos.Add(Convert.ToString(disp.Version));
                sr.Close();
                return Infos;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur dans l'IA, veuillez la recréer." + ex.Message);
                return null;
            }
            
        }

        public void LoadIA(FileInfo fi)
        {
            Decompress(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp", fi.FullName);
            FileInfo file1 = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\disp.xml");
            List<string> infos = (List<string>)DeserializeDisp(file1.FullName);
            FilesList.Items.Add(infos[0]).SubItems.AddRange(new string[] {
		infos[1],
		infos[2],
		infos[3],
		fi.FullName
	});
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\disp.xml");
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\config.xml");
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\spells.xml");
        }

        // Method to decompress.
        public void Decompress(string destinationDirectory, string myzipfile)
        {
            // on cree l'entree zip
            ZipInputStream zipIStream = new ZipInputStream(File.OpenRead(myzipfile));
            ZipEntry theEntry = default(ZipEntry);
            // pour toutes les entrees
            while (true)
            {
                // recuperation de l'entree
                theEntry = zipIStream.GetNextEntry();
                // si l'entree vaut nothing => c'est fini
                if (theEntry == null)
                    break; 
                // test si l'entrée est un fichier
                if (theEntry.IsFile)
                {
                    // definition du fichier de sortie
                    FileInfo myFile = new FileInfo(destinationDirectory + "\\" + theEntry.Name);
                    // on crée le(s) répertoire(s) si besoin
                    Directory.CreateDirectory(myFile.DirectoryName);
                    // creation du fichier de sortie
                    FileStream fs = new FileStream(myFile.FullName, FileMode.Create);
                    int size = 2048;
                    byte[] data = new byte[size + 1];
                    while (!((size <= 0)))
                    {
                        size = zipIStream.Read(data, 0, data.Length);
                        fs.Write(data, 0, size);
                    }
                    fs.Flush();
                    fs.Close();
                }
            }
            // on ferme le flux
            zipIStream.Close();
        }

        public object DeserializeConfig(string file)
        {
            StreamReader sr = new StreamReader(file);
            XmlSerializer seria = new XmlSerializer(typeof(FightConfig));
            List<object> Infos = new List<object>();
            FightConfig conf = (FightConfig)seria.Deserialize(sr);
            Infos.Add(conf.Placement);
            Infos.Add(conf.Tactic);
            sr.Close();
            return conf;
        }

        public object DeserializeSpells(string file)
        {
            StreamReader sr = new StreamReader(file);
            XmlSerializer seria = new XmlSerializer(typeof(List<BSpell>));
            List<BSpell> spells = (List<BSpell>)seria.Deserialize(sr);
            sr.Close();
            return spells;
        }
        #endregion

        #region Méthodes d'interface
        private void FilesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilesList.SelectedItems.Count > 0)
            {
                string path = FilesList.SelectedItems[0].SubItems[4].Text;
                Decompress(Environment.SpecialFolder.ApplicationData + @"\BlueSheep\Temp\", path);
                string file1 = Environment.SpecialFolder.ApplicationData + @"\BlueSheep\Temp\config.xml";
                string file2 = Environment.SpecialFolder.ApplicationData + @"\BlueSheep\Temp\spells.xml";
                FightConfig Conf = (FightConfig)DeserializeConfig(file1);
                List<BSpell> Spells = (List<BSpell>) DeserializeSpells(file2);
                Account.Fight = new BFight(Conf, Spells, Account);
                Account.NomIA.Text = FilesList.SelectedItems[0].SubItems[0].Text;
                File.Delete(Environment.SpecialFolder.ApplicationData + @"\BlueSheep\Temp\disp.xml");
                File.Delete(Environment.SpecialFolder.ApplicationData + @"\BlueSheep\Temp\config.xml");
                File.Delete(Environment.SpecialFolder.ApplicationData + @"\BlueSheep\Temp\spells.xml");
                this.Close();
            }
        }
        

        private void LoadBt_Click(object sender, EventArgs e)
        {
            FilesList_SelectedIndexChanged(null, null);
        }

        private void DelBt_Click(object sender, EventArgs e)
        {
            if (FilesList.SelectedItems.Count > 0)
            {
                string path = FilesList.SelectedItems[0].SubItems[4].Text;
                File.Delete(path);
                FilesList.Items.Remove(FilesList.SelectedItems[0]);
            }

        }

        private void AddBt_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(openFileDialog1.FileName);
                LoadIA(file);

            }
        }

        #endregion

    }
}
