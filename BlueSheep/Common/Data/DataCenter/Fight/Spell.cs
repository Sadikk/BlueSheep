using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Common.Data.DataCenter.Fight
{
    public class Spell : IDataCenter
    {
        public string Module
        {
            get { return "Spells"; }
        }
        public int id;
        public uint nameId;
        public uint descriptionId;
        public uint typeId;
        public String scriptParams;
        public String scriptParamsCritical;
        public int scriptId;
        public int scriptIdCritical;
        public int iconId;
        public List<uint> spellLevels;
        public Boolean useParamCache = true;
        public Boolean verbose_cast;

        public Spell(GameDataFileAccessor gameDataFileAccessor,int bid,uint bnameId,uint bdescriptionId,uint btypeId,String bscriptParams,String bscriptParamsCritical,int bscriptId,int bscriptIdCritical,int biconId,List<uint> bspellLevels,Boolean bverbose_cast)
        {
            id = bid;
            nameId = bnameId;
            descriptionId = bdescriptionId;
            typeId = btypeId;
            scriptParams = bscriptParams;
            scriptParamsCritical = bscriptParamsCritical;
            scriptId = bscriptId;
            scriptIdCritical = bscriptIdCritical;
            iconId = biconId;
            List<uint> spellLevels = bspellLevels;
            Boolean verbose_cast = bverbose_cast;
        }
    }
}
