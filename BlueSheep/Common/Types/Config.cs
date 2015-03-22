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
        public FightConfig m_Conf;
        public List<BSpell> m_Spells;
        public string m_Path;
        public string m_FloodContent;
        public string m_BotPath;
        public string m_IA;
        //public Dictionary<string, int> m_Ressources;
        public List<string> m_L1R;
        public List<int> m_L2R;
        //public Dictionary<DateTime, int> m_xpWon;
        //public Dictionary<string, int> m_winLoseDic;

        public Config(FightConfig myconf, List<BSpell> myspells, string path, string flood, string botpath, string ia, List<string> L1R, List<int> L2R)//,Dictionary<string, int> ressources,Dictionary<DateTime, int> xpWon,
        //Dictionary<string, int> winLoseDic)
        {
            this.m_Conf = myconf;
            this.m_Spells = myspells;
            this.m_Path = path;
            this.m_FloodContent = flood;
            this.m_BotPath = botpath;
            this.m_IA = ia;
            this.m_L1R = L1R;
            this.m_L2R = L2R;
            //this.m_Ressources = ressources;
            //this.m_xpWon = xpWon;
            //this.m_winLoseDic = winLoseDic;
        }

        public Config()
        { }
    }
}
