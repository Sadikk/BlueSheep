using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Core.Fight
{
    [Serializable()]
    public class FightConfig
    {
        public TacticEnum Tactic
        {
            get { return m_Tactic; }
            set { m_Tactic = value; }
        }
        private TacticEnum m_Tactic;
        public PlacementEnum Placement;
        public int FarCells
        {
            get { return m_FarCells; }
            set { m_FarCells = value; }
        }

        private int m_FarCells;
        public FightConfig()
        {
        }

        public FightConfig(string tactic, string placement, int cells)
        {
            switch (tactic)
            {
                case "Fuyard":
                    this.m_Tactic = TacticEnum.Fuyard;
                    break;
                case "Barbare":
                    this.m_Tactic = TacticEnum.Barbare;
                    break;
                case "Immobile":
                    this.m_Tactic = TacticEnum.Immobile;
                    break;
            }
            switch (placement)
            {
                case "Immobile":
                    this.Placement = PlacementEnum.Immobile;
                    break;
                case "Rapproché":
                    this.Placement = PlacementEnum.Rapproché;
                    break;
                case "Eloigné":
                    this.Placement = PlacementEnum.Eloigné;
                    break;
            }
            this.m_FarCells = cells;
        }
    }
}
