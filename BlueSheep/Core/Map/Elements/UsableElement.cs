using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Core.Map.Elements
{
    public class UsableElement
    {
        public UsableElement(int cellId, InteractiveElement element, List<InteractiveElementSkill> skills)
        {
            CellId = cellId;
            Element = element;
            Skills = skills;
        }

        public int CellId { get; private set; }
        public InteractiveElement Element { get; private set; }
        public List<InteractiveElementSkill> Skills { get; private set; }
    }
}
