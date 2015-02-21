using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Common.Data.DataCenter.Fight
{
    class Monsters : IDataCenter
    {
        public string Module
        {
            get { return "Monsters"; }
        }

            public int m_id;
            public uint m_nameId;
            public uint m_gfxId;
            public int m_race;
            public List<MonsterGrade> m_grades;
            public String m_look;
            public Boolean m_useSummonSlot;
            public Boolean m_useBombSlot;
            public Boolean m_canPlay;
            public Boolean m_canTackle;
           // public List<AnimFunMonsterData> m_animFunList;
            public Boolean m_isBoss;
            //public List<MonsterDrop> m_drops;
            public List<uint> m_subareas;
            public List<uint> m_spells;
            public int m_favoriteSubareaId;
            public Boolean m_isMiniBoss;
            public Boolean m_isQuestMonster;
            public uint m_correspondingMiniBossId;
            public float m_speedAdjust = 0;
            public int m_creatureBoneId;
            public Boolean m_canBePushed;
            public Boolean m_fastAnimsFun;


            public Monsters(int id, uint nameId, uint gfxId, int race, List<MonsterGrade> grades, String look, Boolean useSummonSlot, Boolean useBombSlot, Boolean canPlay, Boolean canTackle, Boolean isBoss, List<uint> subareas, List<uint> spells, int favoriteSubareaId, Boolean isMiniBoss, Boolean isQuestMonster, uint correspondingMiniBossId, float speedAdjust, int creatureBoneId, Boolean canBePushed, Boolean fastAnimsFun)
            {

                m_id = id;
                m_nameId = nameId;
                m_gfxId = gfxId;
                m_race = race;
                m_grades = grades;
                m_look = look;
                m_useSummonSlot = useSummonSlot;
                m_useBombSlot = useBombSlot;
                m_canPlay = canPlay;
                m_canTackle = canTackle;
                //m_animFunList = animFunList;
                m_isBoss = isBoss;
                //m_drops = drops;
                m_subareas = subareas;
                m_spells = spells;
                m_favoriteSubareaId = favoriteSubareaId;
                m_isMiniBoss = isMiniBoss;
                m_isQuestMonster = isQuestMonster;
                m_correspondingMiniBossId = correspondingMiniBossId;
                m_speedAdjust = speedAdjust;
                m_creatureBoneId = creatureBoneId;
                m_canBePushed = canBePushed;
                m_fastAnimsFun = fastAnimsFun;
            }

            public Monsters()
            {

            }


    }
}
