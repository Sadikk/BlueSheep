using BlueSheep.Core.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Common.Types
{
    [Serializable()]
    public class Config
    {
        public string m_AIPath;
        public string m_Path;
        public string m_FloodContent;
        public string m_BotPath;
        public string m_IA;
        public bool m_Elevage;
        public List<bool> m_AutoUp;
        public bool m_IsLockingFight;
        public decimal m_RegenValue;
        public List<decimal> m_Restrictions;
        public bool m_AutoDeletion;
        public bool m_RelaunchPath;
        //public System.Windows.Forms.ListView.ListViewItemCollection m_GestItems;
        //public Dictionary<DateTime, int> m_xpWon;
        //public Dictionary<string, int> m_winLoseDic;

        public Config(string path, string flood, string botpath, string ia,string aipath
            , bool elevage, List<bool> autoUp, bool isLockingFight, decimal RegenValue, List<decimal> restrictions,
            bool autoDel, bool RelaunchPath /*, System.Windows.Forms.ListView.ListViewItemCollection gestItems*/)//,Dictionary<string, int> ressources,Dictionary<DateTime, int> xpWon,
        //Dictionary<string, int> winLoseDic)
        {
            this.m_Path = path;
            this.m_FloodContent = flood;
            this.m_BotPath = botpath;
            this.m_IA = ia;
            //this.m_L1R = L1R;
            //this.m_L2R = L2R;
            this.m_AIPath = aipath;
            this.m_Elevage = elevage;
            this.m_AutoUp = autoUp;
            this.m_IsLockingFight = isLockingFight;
            this.m_RegenValue = RegenValue;
            this.m_Restrictions = restrictions;
            this.m_AutoDeletion = autoDel;
            this.m_RelaunchPath = RelaunchPath;
            //this.m_GestItems = gestItems;
            //this.m_Ressources = ressources;
            //this.m_xpWon = xpWon;
            //this.m_winLoseDic = winLoseDic;
        }

        public Config()
        { }
    }
}
