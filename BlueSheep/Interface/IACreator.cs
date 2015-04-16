using BlueSheep.Common.Types;
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

namespace BlueSheep.Interface
{
    public partial class IACreator : Form
    {
        /// <summary>
        /// AI Creation form.
        /// </summary>


        //TODO Traduire en anglais ces vieux noms
        #region Fields
        AccountUC Account;
        private List<BSpell> SpellsList { get; set; }
        #endregion

        #region Constructors
        public IACreator(AccountUC account)
        {
            InitializeComponent();
            MonsterTextBox.KeyUp += (s, e) =>
            {
                BlueSheep.Engine.Constants.IntelliSense.AutoCompleteTextBox(MonsterTextBox, lstPopup, BlueSheep.Engine.Constants.IntelliSense.MonstersList, e);
            };
            Account = account;
            string ApplicationDataPath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
            string combinedPath = System.IO.Path.Combine (ApplicationDataPath, "BlueSheep", "IAs");
            saveFileDialog1.InitialDirectory = combinedPath;
            FillSpells();
            switch (MainForm.ActualMainForm.Lang)
            {
                case "EN":
                    this.Text = "AI Manager";
                    DelBt.Text = "Delete";
                    SaveIABt.Text = "Save the AI";
                    AddBt.Text = "Add spell";
                    UpBt.Text = "Up";
                    Descendre.Text = "Down";
                    sadikLabel4.Text = "Name";
                    sadikLabel5.Text = "Author";
                    sadikLabel6.Text = "Breed";
                    GroupBox1.Text = "Spells";
                    sadikLabel11.Text = "Target";
                    sadikLabel10.Text = "Spells";
                    sadikLabel8.Text = "Turn";
                    sadikLabel3.Text = "Launch the spell if the monster's called : ";
                    sadikLabel1.Text = "Launch the spell if the target's life (%) is under or equals at ";
                    GroupBox3.Text = "Setting";
                    ResumeIA.Columns[1].Text = "Name";
                    ResumeIA.Columns[2].Text = "Turn";
                    ResumeIA.Columns[3].Text = "Launch numbers";
                    ResumeIA.Columns[4].Text = "Target";
                    break;
            }
        }
        #endregion

        #region Interface Methods
        private void AddBt_Click(object sender, EventArgs e)
        {
//            string Mob = null;
//            Spell iSpell = Account.Spells.FirstOrDefault(s => s.GetName() == ChoixSorts.SelectedItem.ToString());
//            BSpell spell = new BSpell(iSpell.Id, iSpell.GetName(), (TeamEnum)ChoixCible.SelectedIndex, Convert.ToInt32(ChoixTour.Value), Convert.ToInt32(ChoixNbLancer.Value), Convert.ToInt32(nudTargetLifePercent.Value), MonsterTextBox.Text);
//            SpellsList.Add(spell);
//            if (MonsterTextBox.Text == "Nom du monstre")
//            {
//                Mob = "All mobs";
//            }
//            else
//            {
//                Mob = MonsterTextBox.Text;
//            }
//            ResumeIA.Items.Add(spell.Id.ToString()).SubItems.AddRange(new string[] {
//    spell.Name,
//    spell.Turns.ToString(),
//    spell.Relaunch.ToString(),
//    spell.Target.ToString(),
//" Vie inférieure à " + spell.TargetLife.ToString() + "% Cible :" + Mob
//});
        }

        private void SaveIABt_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dynamic iapath = saveFileDialog1.FileName;
                //Display Disp = new Display(ChoixName.Text, ChoixAuteur.Text, ChoixClasse.Text, ChoixVersion.Value);
                FightConfig Config = new FightConfig(ChoixTactique.SelectedText, ChoixPlacement.SelectedText, (int)ChoixFarCells.Value);
                //string ApplicationDataPath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
                //string tempPath = iapath = System.IO.Path.Combine (ApplicationDataPath, "BlueSheep", "Temp");
                //string dispPath = System.IO.Path.Combine (tempPath, "disp.xml");
                //Serialize(Disp, dispPath);
                //string configPath = System.IO.Path.Combine (tempPath, "config.xml");
                //Serialize(Config, configPath);
                //string spellsPath = System.IO.Path.Combine (tempPath, "spells.xml");
                //Serialize(SpellsList, spellsPath);
                //compression(tempPath, iapath);
                //File.Delete(dispPath);
                //File.Delete(configPath);
                //File.Delete(spellsPath);
                //Serialize(Disp, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\disp.xml");
                Serialize(Config, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\config.xml");
                Serialize(SpellsList, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\spells.xml");
                compression(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\", iapath);
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\disp.xml");
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\config.xml");
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Temp\spells.xml");
                this.Close();
            }
        }

        private void ChoixTactique_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChoixTactique.SelectedItem == "Fuyard")
            {
                sadikLabel12.Show();
                ChoixFarCells.Show();
            }
            else
            {
                sadikLabel12.Hide();
                ChoixFarCells.Hide();
            }
        }

        private void UpBt_Click(object sender, EventArgs e)
        {
            if (ResumeIA.SelectedItems.Count == 0)
            {
                return;
            }

            //BSpell spell = SpellsList[ResumeIA.SelectedItems[0].Index];
            //if (spell.Id != 0)
            //{
            //    spell.Id -= 1;
            //    SpellsList[spell.Id].Id += 1;

            //    RefreshSpellsList();
            //    ResumeIA.Items[spell.Id].Selected = true;
            //}
        }

        private void Descendre_Click(object sender, EventArgs e)
        {

            if (ResumeIA.SelectedItems.Count == 0)
            {
                return;
            }

            //BSpell spell = SpellsList[ResumeIA.SelectedItems[0].Index];
            //if (spell.Id != SpellsList.Count - 1)
            //{
            //    spell.Id += 1;
            //    SpellsList[spell.Id].Id -= 1;

            //    RefreshSpellsList();
            //    ResumeIA.Items[spell.Id].Selected = true;
            //}
        }

        private void DelBt_Click(object sender, EventArgs e)
        {
            if (ResumeIA.SelectedItems.Count == 0)
            {
                return;
            }

            SpellsList.RemoveAt(ResumeIA.SelectedItems[0].Index);
            RefreshSpellsList();

            if (ResumeIA.Items.Count != 0)
            {
                ResumeIA.Items[0].Selected = true;
            }
        }

        private void MonsterTextBox_GotFocus(object sender, EventArgs e)
        {
            if (MonsterTextBox.Text == "Nom du monstre")
                MonsterTextBox.Text = "";
        }

        private void MonsterTextBox_LostFocus(object sender, EventArgs e)
        {
            if (MonsterTextBox.Text == "")
                MonsterTextBox.Text = "Nom du monstre";
        }
        #endregion

        #region Private methods
        private void compression(string DirectoryToZip, string TheZipFile)
        {
            int i = 0;
            // index des fichiers de filestozip()
            int mylength = 0;
            // longueur fichier
            // recupère le nom complet de tous les fichiers dans filestozip()
            string[] filestozip = Directory.GetFiles(DirectoryToZip, "*.*", SearchOption.TopDirectoryOnly);
            // mydirname : nom de la directory a compresser
            string mydirname = new DirectoryInfo(DirectoryToZip).Name;
            // on cree la sortie zip
            ZipOutputStream ZipStream = new ZipOutputStream(File.Create(TheZipFile));
            // on fait le boulot pour touss les fichiers
            for (i = 0; i < filestozip.Length; i++)
            {
                // on ouvre le fichier en lecture
                FileStream fs = File.OpenRead(filestozip[i]);
                mylength = (int)fs.Length;
                // Tableau de byte, de la taille du fichier à lire
                byte[] buffer = new byte[mylength + 1];
                // Lecture du fichier dans buffer puis fermeture
                fs.Read(buffer, 0, mylength);
                fs.Close();
                // definition de l'entrée dans le zip
                ZipEntry entry = new ZipEntry(filestozip[i].Replace(DirectoryToZip, ""));
                // On ajoute la nouvelle entrée
                ZipStream.PutNextEntry(entry);
                // On crée la nouvelle entrée dans l'archive
                ZipStream.Write(buffer, 0, mylength);
            }
            //On ferme le flux
            ZipStream.Finish();
            ZipStream.Close();
        }

        private void FillSpells()
        {
            ChoixSorts.Items.Clear();
            foreach (Spell spell in Account.Spells)
            {
                ChoixSorts.Items.Add(spell.GetName());
            }
            SpellsList = new List<BSpell>();
            ChoixTactique.SelectedIndex = 0;
            ChoixPlacement.SelectedIndex = 0;
            ChoixCible.SelectedIndex = 0;
            //ChoixMonsterName.Hide();
            //sadikLabel3.Hide();
            MonsterTextBox.Text = "Nom du monstre";
            ChoixSorts.SelectedIndex = 1;
            sadikLabel12.Hide();
            ChoixFarCells.Hide();
        }

        private void Serialize<T>(T obj, string sConfigFilePath)
        {

                System.Xml.Serialization.XmlSerializer XmlBuddy = new System.Xml.Serialization.XmlSerializer(typeof(T));
                System.Xml.XmlWriterSettings MySettings = new System.Xml.XmlWriterSettings();
                MySettings.Indent = true;
                MySettings.CloseOutput = true;
                MySettings.OmitXmlDeclaration = true;
                System.Xml.XmlWriter MyWriter = System.Xml.XmlWriter.Create(sConfigFilePath, MySettings);
                XmlBuddy.Serialize(MyWriter, obj);
                MyWriter.Flush();
                MyWriter.Close();

        }

        private void RefreshSpellsList()
        {
        //    ResumeIA.Items.Clear();
        //    SpellsList = SpellsList.OrderBy(s => s.Id).ToList();

        //    if (SpellsList.Count != ResumeIA.Items.Count)
        //    {
        //        for (int i = 0; i <= SpellsList.Count - 1; i++)
        //        {
        //            SpellsList[i].Id = i;
        //        }
        //    }

        //    foreach (BSpell spell in SpellsList)
        //    {
        //        ResumeIA.Items.Add(spell.Id.ToString()).SubItems.AddRange(new string[] {
        //    spell.Name,
        //    spell.Turns.ToString() + "/" + spell.Relaunch.ToString()
        //});
        //    }

        }

        #endregion

        
    }
}
