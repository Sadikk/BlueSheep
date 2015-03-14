using BlueSheep.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Common.Types
{
    public class MonsterGroup
    {
        public MonsterGroup(GroupMonsterStaticInformations staticInfos, int cellId, int contextualId)
        {
            m_staticInfos = staticInfos;
            m_cellId = cellId;
            m_contextualId = contextualId;
        }

        public GroupMonsterStaticInformations m_staticInfos;
        public int m_cellId;
        public int m_contextualId;
    }
}
