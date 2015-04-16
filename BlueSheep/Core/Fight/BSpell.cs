using BlueSheep.Common.Data.D2o;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Core.Fight
{
    public class BSpell
    {
        public int SpellId
        {
            get { return m_SpellId; }
            set { m_SpellId = value; }
        }
        private int m_SpellId;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        private string m_Name;
        
        public BSpell()
        {
        }

        public BSpell(int spellId, string name)
        {
            this.SpellId = spellId;
            this.Name = name;
        }

        public DataClass data
        {
            get
            {
                return GameData.GetDataObject(D2oFileEnum.Spells, this.m_SpellId);
            }
        }
    }

}
