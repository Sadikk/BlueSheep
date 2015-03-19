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
        //public Dictionary<string, int> m_Ressources;
        //public Dictionary<DateTime, int> m_xpWon;
        //public Dictionary<string, int> m_winLoseDic;

        public Config(FightConfig myconf, List<BSpell> myspells, string path, string flood)//,Dictionary<string, int> ressources,Dictionary<DateTime, int> xpWon,
        //Dictionary<string, int> winLoseDic)
        {
            this.m_Conf = myconf;
            this.m_Spells = myspells;
            this.m_Path = path;
            this.m_FloodContent = flood;
            //this.m_Ressources = ressources;
            //this.m_xpWon = xpWon;
            //this.m_winLoseDic = winLoseDic;
        }

        public Config()
        { }
    }
}
