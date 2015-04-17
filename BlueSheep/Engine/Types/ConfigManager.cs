using BlueSheep.Common.Types;
using BlueSheep.Core.Fight;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
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
                if (conf == null)
                    return;
                if (conf.m_AIPath != "")
                {
                    account.FightParser = new FightParser(account, conf.m_AIPath, conf.m_IA);
                    account.Fight = new BFight(account, account.FightParser, account.FightData);
                    account.ModLabel(conf.m_IA, account.NomIA);
                }
                if (conf.m_Path != null && conf.m_BotPath != null && conf.m_Path != "")
                {
                    account.Path = new Core.Path.PathManager(account, conf.m_Path, conf.m_BotPath);
                    if (conf.m_BotPath != null)
                        account.Log(new BotTextInformation("Trajet chargé : " + conf.m_BotPath), 0);
                    else
                        account.Log(new BotTextInformation("Trajet chargé : UNKNOWN"), 0);
                }
                
                if (account.Fight == null)
                {
                    account.Log(new ErrorTextInformation("WARNING : T'as chargé aucune IA, fait gaffe mon coco :p"), 0);
                }

                if (conf.m_FloodContent != "")
                {
                    account.FloodUC.FloodContentRbox.Text = conf.m_FloodContent;
                }
                //if (conf.m_L1R != null)
                //{
                //    Dictionary<string, int> ressources = conf.m_L1R.ToDictionary(x => x, x => conf.m_L2R[conf.m_L1R.IndexOf(x)]);
                //    account.Gather.Stats = ressources;
                //}

                if (conf.m_AutoUp.Count > 0)
                {
                    if (conf.m_AutoUp[0])
                        account.CaracUC.VitaRb.Checked = true;
                    else if (conf.m_AutoUp[1])
                        account.CaracUC.WisRb.Checked = true;
                    else if (conf.m_AutoUp[2])
                        account.CaracUC.StreRb.Checked = true;
                    else if (conf.m_AutoUp[3])
                        account.CaracUC.InteRb.Checked = true;
                    else if (conf.m_AutoUp[4])
                        account.CaracUC.LuckRb.Checked = true;
                    else if (conf.m_AutoUp[5])
                        account.CaracUC.AgiRb.Checked = true;
                }
                if (conf.m_Elevage != null)
                    account.checkBoxBegin.Checked = conf.m_Elevage;
                if (conf.m_IsLockingFight)
                    account.IsLockingFight.Checked = conf.m_IsLockingFight;
                if (conf.m_RegenValue != null)
                    account.RegenChoice.Value = conf.m_RegenValue;
                if (conf.m_Restrictions.Count > 0)
                {
                    account.nudMinMonstersLevel.Value = conf.m_Restrictions[0];
                    account.nudMaxMonstersLevel.Value = conf.m_Restrictions[1];
                    account.nudMinMonstersNumber.Value = conf.m_Restrictions[2];
                    account.nudMaxMonstersNumber.Value = conf.m_Restrictions[3];
                }
                if (conf.m_AutoDeletion != null)
                    account.GestItemsUC.AutoDeletionBox.Checked = conf.m_AutoDeletion;
                if (conf.m_RelaunchPath && account.Path != null)
                    account.Path.Relaunch = true;
                //if (conf.m_GestItems != null)
                //{
                //    foreach (ListViewItem i in conf.m_GestItems)
                //        account.GestItemsUC.LVGestItems.Items.Add(i);
                //}

                account.Log(new BotTextInformation("Configuration restaurée."), 0);
            }
            else
            {
                account.Log(new BotTextInformation("Aucune config pour ce personnage."), 0);
            }
            Restored = true;
        }

        public void SaveConfig()
        {
            if (!account.Enabled)
                return;
            List<BSpell> lspells = new List<BSpell>();
            string ia = "Aucune IA";
            string AIpath = "";
            //Dictionary<DateTime, int> exp = new Dictionary<DateTime,int>();
            //Dictionary<string,int> winLose = new Dictionary<string,int>();

            if (account.FightParser != null)
            {
                ia = account.FightParser.Name;
                AIpath = account.FightParser.path;
                //exp = account.Fight.xpWon;
                //winLose = account.Fight.winLoseDic;
            }

            string path = "";
            if (account.Path != null && !string.IsNullOrEmpty(account.Path.path))
            {
                path = account.Path.path;
            }

            string flood = "";
            if (account.FloodUC.FloodContent.Length > 0)
            {
                flood = account.FloodUC.FloodContent;
            }

            string pathBot = "";
            if (account.Path != null)
                pathBot = account.Path.pathBot;

            List<bool> AutoUp = new List<bool>() { false, false, false, false, false, false };
            if (account.CaracUC.VitaRb.Checked)
                AutoUp[0] = true;
            else if (account.CaracUC.WisRb.Checked)
                AutoUp[1] = true;
            else if (account.CaracUC.StreRb.Checked)
                AutoUp[2] = true;
            else if (account.CaracUC.InteRb.Checked)
                AutoUp[3] = true;
            else if (account.CaracUC.LuckRb.Checked)
                AutoUp[4] = true;
            else if (account.CaracUC.AgiRb.Checked)
                AutoUp[5] = true;

            bool isLockingFight = account.IsLockingFight.Checked;
            decimal RegenValue = account.RegenChoice.Value;

            List<decimal> Restrictions = new List<decimal>(){account.nudMinMonstersLevel.Value,
                account.nudMaxMonstersLevel.Value,
                account.nudMinMonstersNumber.Value,
                account.nudMaxMonstersNumber.Value};

            bool AutoDeletion = account.GestItemsUC.AutoDeletionBox.Checked;
            //System.Windows.Forms.ListView.ListViewItemCollection GestItems = account.GestItemsUC.LVGestItems.Items;

            bool RelaunchPath = account.RelaunchPath.Checked;
            //Dictionary<string, int> ressources = account.Gather.Stats;
            //List<string> L1R = ressources.Keys.ToList();
            //List<int> L2R = ressources.Values.ToList();
            foreach (JobUC uc in account.JobsUC)
                uc.exportToXml();

            Config conf = new Config(path, flood, pathBot, ia, AIpath, account.checkBoxBegin.Checked, 
                AutoUp, isLockingFight, RegenValue, Restrictions, AutoDeletion, RelaunchPath/*,GestItems*/);//, ressources, exp, winLose); 
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
                try
                {
                    Config conf = (Config)seria.Deserialize(sr);
                    return conf;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur de configuration. Supprimez votre configuration et réessayez.");
                    sr.Close();
                }
                return null;
                
        }

        
        #endregion
    }
}
