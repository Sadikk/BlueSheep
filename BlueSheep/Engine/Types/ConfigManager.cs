using BlueSheep.Common.Types;
using BlueSheep.Core.Fight;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BlueSheep.Engine.Types
{
    public class ConfigManager
    {
        #region Fields
        private AccountUC account;
        public bool Restored = false;
        #endregion

        #region Constructors
        public ConfigManager(AccountUC Account)
        {
            this.account = Account;
        }
        #endregion

        #region Public Methods
        public void RecoverConfig()
        {
            string spath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlueSheep", "Accounts", account.AccountName, account.CharacterBaseInformations.name + ".xml");
            if (File.Exists(spath))
            {
                Config conf = DeserializeConfig(spath);
                if (conf.m_Conf != null)
                {
                    account.Fight = new BFight(conf.m_Conf, conf.m_Spells, account);
                    //account.Fight.winLoseDic = conf.m_winLoseDic;
                    //account.Fight.xpWon = conf.m_xpWon;
                    account.NomIA.Text = conf.m_IA;
                }

                account.Path = new Core.Path.PathManager(account, conf.m_Path, conf.m_BotPath);
                if (conf.m_BotPath != null)
                    account.Log(new BotTextInformation("Trajet chargé : " + conf.m_BotPath), 0);
                else
                    account.Log(new BotTextInformation("Trajet chargé : UNKNOWN"), 0);
                if (account.Fight == null)
                {
                    account.Log(new ErrorTextInformation("WARNING : T'as chargé aucune IA, fait gaffe mon coco :p"), 0);
                }

                if (conf.m_FloodContent != "")
                {
                    account.FloodContentRbox.Text = conf.m_FloodContent;
                }
                if (conf.m_L1R != null)
                {
                    Dictionary<string, int> ressources = conf.m_L1R.ToDictionary(x => x, x => conf.m_L2R[conf.m_L1R.IndexOf(x)]);
                    account.Gather.Stats = ressources;
                }

                account.Log(new BotTextInformation("Configuration restauré."), 0);


            }
            else
            {
                account.Log(new BotTextInformation("Aucune config pour ce personnage."), 0);
            }
            Restored = true;
        }

        public void SaveConfig()
        {
            FightConfig fconf = null;
            List<BSpell> lspells = new List<BSpell>();
            string ia = "Aucune IA";
            //Dictionary<DateTime, int> exp = new Dictionary<DateTime,int>();
            //Dictionary<string,int> winLose = new Dictionary<string,int>();

            if (account.Fight != null)
            {
                fconf = account.Fight.m_Conf;
                lspells = account.Fight.m_Spells;
                ia = account.NomIA.Text;
                //exp = account.Fight.xpWon;
                //winLose = account.Fight.winLoseDic;
            }

            string path = "";
            if (account.Path != null)
            {
                path = account.Path.path;
            }

            string flood= "";
            if (account.FloodContent.Length > 0)
            {
                flood = account.FloodContent;
            }

            string pathBot = "";
            if (account.Path.pathBot != null)
                pathBot = account.Path.pathBot;

            

            Dictionary<string, int> ressources = account.Gather.Stats;
            List<string> L1R = ressources.Keys.ToList();
            List<int> L2R = ressources.Values.ToList();

            Config conf = new Config(fconf, lspells, path, flood, pathBot, ia,L1R,L2R);//, ressources, exp, winLose);

            string spath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlueSheep", "Accounts", account.AccountName, account.CharacterBaseInformations.name + ".xml");
            Serialize(conf, spath);
        }

        public void DeleteConfig()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlueSheep", "Accounts", account.AccountName, account.CharacterBaseInformations.name + ".xml");
            if (File.Exists(path))
                File.Delete(path);
        }
        #endregion

        #region Private Methods
        private void Serialize<T>(T obj, string sConfigFilePath)
        {
            try
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
            catch(Exception ex)
            {
                account.Log(new ErrorTextInformation(ex.Message + ex.StackTrace), 0);
            }
        }

        public Config DeserializeConfig(string file)
        {
                StreamReader sr = new StreamReader(file);
                XmlSerializer seria = new XmlSerializer(typeof(Config));
                Config conf = (Config)seria.Deserialize(sr);
                sr.Close();
                return conf;
        }
        #endregion
    }
}
