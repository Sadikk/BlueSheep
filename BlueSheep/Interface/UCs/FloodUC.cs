using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BlueSheep.Interface.Text;

namespace BlueSheep.Interface.UCs
{
    public partial class FloodUC : MetroFramework.Controls.MetroUserControl
    {
        #region Fields
        private AccountUC m_Account;
        public string FloodContent;
        private int m_MpCount = 0;
        private int m_MessageCount = 0;
        #endregion

        #region Constructors
        public FloodUC(AccountUC account)
        {
            InitializeComponent();
            m_Account = account;
            string pathPlayers = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlueSheep", "Accounts", m_Account.AccountName, "Flood");
            if (!Directory.Exists(pathPlayers))
                Directory.CreateDirectory(pathPlayers);
            PrivateExitBox.Hide();
            FloodContent = "";
            Dictionary<string, int> temp = new Dictionary<string, int>();
            if (File.Exists(pathPlayers + @"\Players.txt"))
            {
                var sr = new StreamReader(pathPlayers + @"\Players.txt");
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] parsed = line.Split(',');
                    if (parsed.Length > 1)
                    {
                        temp.Add(parsed[0], int.Parse(parsed[1]));
                        PlayerListLb.Items.Add(line);
                    }
                    else
                    {
                        sr.Close();
                        File.Delete(pathPlayers + @"\Players.txt");
                        return;
                    }
                    
                }
                sr.Close();
                m_Account.Log(new DebugTextInformation("[ADVANCED FLOOD] Players loaded."), 5);
            }
        }
        #endregion

        #region Public Methods
        public void AddItem(string line)
        {
           PlayerListLb.Items.Add(line);
        }

        public void Increase(bool mp)
        {
            if (mp)
                m_MpCount++;
            else
                m_MessageCount++;
        }
        #endregion

        #region UI Methods
        private void RemovePlayerBt_Click(object sender, EventArgs e)
        {

            string pathPlayers = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlueSheep", "Accounts", m_Account.AccountName, "Flood");
            if (File.Exists(pathPlayers + "\\Players.txt"))
            {
                this.DeleteLine(pathPlayers + "\\Players.txt", PlayerListLb.SelectedItem.ToString());
            }
            if (PlayerListLb.SelectedItem != null)
            {
                PlayerListLb.Items.Remove(PlayerListLb.SelectedItem);
            }
        }

        private void FloodContentRbox_TextChanged(object sender, EventArgs e)
        {
            FloodContent = FloodContentRbox.Text;
        }

        private void ClearListeBt_Click(object sender, EventArgs e)
        {
            if (PlayerListLb.Items.Count != 0)
            {
                PlayerListLb.Items.Clear();
            }
            string pathPlayers = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlueSheep", "Accounts", m_Account.AccountName, "Flood");
            if (File.Exists(pathPlayers + "\\Players.txt"))
            {
                var sw = new StreamWriter(pathPlayers + "\\Players.txt");
                sw.Write("");
                sw.Close();
            }
        }

        private void FloodPlayersBt_Click(object sender, EventArgs e)
        {
            this.FloodContent = this.FloodContentRbox.Text;
            foreach (var elem in PlayerListLb.Items)
            {
                try
                {
                    string[] parsed = ((string)elem).Split(',');
                    this.FloodContent = this.FloodContent.Replace("%name%", parsed[0]).Replace("%level%", parsed[1]);
                    m_Account.Flood.SendPrivateTo((string)parsed[0], this.FloodContent);
                }
                catch (Exception)
                {
                    m_Account.Log(new ErrorTextInformation("Impossible d'envoyer le message à: " + (string)elem), 3);
                }
            }
        }

        private void StartStopFloodingBox_CheckedChanged(object sender)
        {
            if (StartStopFloodingBox.Checked == false)
            {
                m_Account.Flood.stop = true;
                m_Account.Log(new BotTextInformation("Flood arrêté"), 1);
                m_Account.Log(new BotTextInformation(m_MpCount + " mps envoyés."), 0);
                m_Account.Log(new BotTextInformation(m_MessageCount + " messages envoyés."), 0);
                m_MessageCount = 0;
                m_MpCount = 0;
                return;
            }
            m_Account.Log(new BotTextInformation("Flood activé"), 1);
            if (CommerceBox.Checked)
                m_Account.Flood.StartFlood(5, IsRandomingSmileyBox.Checked, IsRandomingNumberBox.Checked, FloodContentRbox.Text, (int)NUDFlood.Value);
            if (RecrutementBox.Checked)
                m_Account.Flood.StartFlood(6, IsRandomingSmileyBox.Checked, IsRandomingNumberBox.Checked, FloodContentRbox.Text, (int)NUDFlood.Value);
            if (GeneralBox.Checked)
                m_Account.Flood.StartFlood(0, IsRandomingSmileyBox.Checked, IsRandomingNumberBox.Checked, FloodContentRbox.Text, (int)NUDFlood.Value);
        }
        #endregion

        #region Private Methods
        private void DeleteLine(string path, string ligne)
        {

            string texte = null;
            string ligneActuelle = null;
            StreamReader sr = new StreamReader(path);

            while ((sr.Peek() != -1))
            {
                ligneActuelle = sr.ReadLine();
                if (!(ligneActuelle == ligne))
                {
                    texte = (texte + (ligneActuelle + "\r\n"));
                }
            }
            sr.Close();

            File.WriteAllText(path, texte);
        }
        #endregion


    }
}
