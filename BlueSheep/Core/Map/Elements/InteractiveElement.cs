using BlueSheep.Common.Data;
using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Core.Map.Elements
{
    public class InteractiveElement
    {
        public InteractiveElement(uint id, int typeId, List<InteractiveElementSkill> enabledSkills, List<InteractiveElementSkill> disabledSkills)
        {
            Id = id;
            TypeId = typeId;
            EnabledSkills = enabledSkills;
            DisabledSkills = disabledSkills;
        }

        public List<InteractiveElementSkill> DisabledSkills { get; private set; }
        public List<InteractiveElementSkill> EnabledSkills { get; private set; }
        public uint Id { get; private set; }

        public bool IsUsable
        {
            get { return EnabledSkills.Count > 0; }
        }

        public int TypeId { get; private set; }

        public string Name
        {
            get
            {
                List<DataClass> ld = GameData.GetDataObjects(D2oFileEnum.Skills).ToList();

                foreach (DataClass d in ld)
                {
                    if ((int)d.Fields["interactiveId"] == TypeId)
                        return I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Items, (int)d.Fields["gatheredRessourceItem"]).Fields["nameId"]);
                }
                return "Unknown";
            }
        }
    }
}
