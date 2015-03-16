using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Core.Fight
{
    [Serializable()]
    public class BSpell
    {
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        private int m_Id;
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
        public TeamEnum Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }
        private TeamEnum m_Target;
        public int Turns
        {
            get { return m_Turns; }
            set { m_Turns = value; }
        }
        private int m_Turns;
        public int Relaunch
        {
            get { return m_Relaunch; }
            set { m_Relaunch = value; }
        }
        private int m_Relaunch;
        public int LastTurn
        {
            get { return m_LastTurn; }
            set { m_LastTurn = value; }
        }
        private int m_LastTurn;
        //public bool IsHandToHand
        //{
        //    get { return m_IsHandToHand; }
        //    set { m_IsHandToHand = value; }
        //}
        //private bool m_IsHandToHand;
        public int TargetLife
        {
            get { return m_TargetLife; }
            set { m_TargetLife = value; }
        }

        private int m_TargetLife;
        public string TargetName
        {
            get { return m_TargetName; }
            set { m_TargetName = value; }
        }

        private string m_TargetName;
        public BSpell()
        {
        }

        public BSpell(int spellId, string name, TeamEnum target, int turns, int relaunch, int targetLife, string targetName)
        {
            this.SpellId = spellId;
            this.Name = name;
            this.Target = target;
            this.Turns = turns;
            this.Relaunch = relaunch;
            this.LastTurn = 1;    
            this.TargetLife = targetLife;
            this.TargetName = targetName;
            //this.IsHandToHand = isHandToHand;
            //Me.Id = id
        }
    }



    [Serializable()]
    public class Display
    {
        public string Name;
        public string Author;
        public string Race;
        //Public Race As RebirthAPI.Game.Entity.Infos.Breed

        public decimal Version;

        public Display()
        {
        }

        public Display(string Nom, string auteur, string classe, decimal version)
        {
            this.Name = Nom;
            this.Author = auteur;
            this.Race = classe;
            this.Version = version;
        }
    }

    [Serializable()]
    public class ListSpells
    {

        public List<BSpell> ListOfSpells = new List<BSpell>();
        public ListSpells()
        {
        }
    }

}
