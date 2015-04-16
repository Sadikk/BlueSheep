using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.Protocol.Types;
using System;
using System.Collections;
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

        public int monstersCount
        {
            get { return m_staticInfos.underlings.Count() + 1; }
        }

        public int monstersLevel
        {
            get
            {
                int monstersLevel = 0;
                foreach (MonsterInGroupInformations monster in m_staticInfos.underlings)
                {
                    DataClass monsterData = GameData.GetDataObject(D2oFileEnum.Monsters, monster.creatureGenericId);
                    object monsterGrades = monsterData.Fields["grades"];
                    ArrayList monsterGrades2 = (ArrayList)monsterGrades;
                    DataClass monsterGradeData = (DataClass)monsterGrades2[Convert.ToInt32(monster.grade) - 1];
                    monstersLevel += Convert.ToInt32(monsterGradeData.Fields["level"]);
                }
                DataClass mainmonsterData = GameData.GetDataObject(D2oFileEnum.Monsters, m_staticInfos.mainCreatureLightInfos.creatureGenericId);
                object mainmonsterGrades = mainmonsterData.Fields["grades"];
                ArrayList mainmonsterGrades2 = (ArrayList)mainmonsterGrades;
                DataClass mainmonsterGradeData = (DataClass)mainmonsterGrades2[Convert.ToInt32(m_staticInfos.mainCreatureLightInfos.grade) - 1];
                monstersLevel += Convert.ToInt32(mainmonsterGradeData.Fields["level"]);
                return monstersLevel;
            }
        }

        public string monstersName(bool withLevels)
        {
            List<string> monstersname = new List<string>();

            foreach (MonsterInGroupInformations monster in m_staticInfos.underlings)
            {
                DataClass monsterData = GameData.GetDataObject(D2oFileEnum.Monsters, monster.creatureGenericId);
                object monsterGrades = monsterData.Fields["grades"];
                ArrayList monsterGrades2 = (ArrayList)monsterGrades;
                DataClass monsterGradeData = (DataClass)monsterGrades2[Convert.ToInt32(monster.grade) - 1];
                if (withLevels)
                    monstersname.Add(BlueSheep.Common.Data.I18N.GetText((int)monsterData.Fields["nameId"]) + "(" + monsterGradeData.Fields["level"] + ")");
                else
                    monstersname.Add(BlueSheep.Common.Data.I18N.GetText((int)monsterData.Fields["nameId"]));
            }
            DataClass mainmonsterData = GameData.GetDataObject(D2oFileEnum.Monsters, m_staticInfos.mainCreatureLightInfos.creatureGenericId);
            object mainmonsterGrades = mainmonsterData.Fields["grades"];
            ArrayList mainmonsterGrades2 = (ArrayList)mainmonsterGrades;
            DataClass mainmonsterGradeData = (DataClass)mainmonsterGrades2[Convert.ToInt32(m_staticInfos.mainCreatureLightInfos.grade) - 1];
            if (withLevels)
                monstersname.Add(BlueSheep.Common.Data.I18N.GetText((int)mainmonsterData.Fields["nameId"]) + "(" + mainmonsterGradeData.Fields["level"] + ")");
            else
                monstersname.Add(BlueSheep.Common.Data.I18N.GetText((int)mainmonsterData.Fields["nameId"]));

            string names = "";
            foreach (string item in monstersname)
            {
                names = names + item;
                if (monstersname.IndexOf(item) != monstersname.Count - 1)
                    names = names + ",";
            }
            return names;
        }

        public List<string> NameList()
        {
            List<string> monstersname = new List<string>();

            foreach (MonsterInGroupInformations monster in m_staticInfos.underlings)
            {
                DataClass monsterData = GameData.GetDataObject(D2oFileEnum.Monsters, monster.creatureGenericId);
                monstersname.Add(BlueSheep.Common.Data.I18N.GetText((int)monsterData.Fields["nameId"]));
            }
            DataClass mainmonsterData = GameData.GetDataObject(D2oFileEnum.Monsters, m_staticInfos.mainCreatureLightInfos.creatureGenericId);
            monstersname.Add(BlueSheep.Common.Data.I18N.GetText((int)mainmonsterData.Fields["nameId"]));
            return monstersname;
        }
                    
    }
}
