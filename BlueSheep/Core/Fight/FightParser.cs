using BlueSheep.Common.Data;
using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace BlueSheep.Core.Fight
{
    public class FightParser
    {
        #region Fields
        private AccountUC m_Account;
        public string path;
        public string Name;
        public BFighter target;
        private string m_flag;
        private List<FightCondition> m_cond;

        /// <summary>
        /// Store the spell and the associated id.
        /// </summary>
        private Dictionary<BSpell, int> spells;

        /// <summary>
        /// Store the target and the associated id.
        /// </summary>
        private Dictionary<string, int> targets;

        /// <summary>
        /// Store the positioning to do before fight start.
        /// </summary>
        private List<PlacementEnum> positions;

        /// <summary>
        /// Store the strategys.
        /// </summary>
        private Dictionary<TacticEnum, int> strategy;

        /// <summary>
        /// Store the challenges we must try to do.
        /// </summary>
        private List<ChallengeEnum> challenges;

        /// <summary>
        /// Store the conditions for each spell.
        /// </summary>
        private Dictionary<BSpell, List<FightCondition>> SpellsCondition;

        /// <summary>
        /// Store the conditions for each spell.
        /// </summary>
        private Dictionary<string, List<FightCondition>> TargetsCondition;


        public static readonly IList<String> flags = new ReadOnlyCollection<string>
        (new List<String> { "<Spells>", "<Targets>", "<Strategy>", "<Position>"/*, "<Challenges>" */});

        public static readonly IList<String> Endflags = new ReadOnlyCollection<string>
        (new List<String> { "</Spells>", "</Targets>", "</Strategy>", "</Position>"/*, "</Challenges>"*/ });

        public static readonly IList<Char> operateurs = new ReadOnlyCollection<char>
        (new List<Char> { '<', '>', '=' });
        #endregion

        #region Constructors
        public FightParser(AccountUC Account, string Path, string name)
        {
            m_Account = Account;
            path = Path;
            spells = new Dictionary<BSpell, int>();
            targets = new Dictionary<string, int>();
            positions = new List<PlacementEnum>();
            strategy = new Dictionary<TacticEnum, int>();
            challenges = new List<ChallengeEnum>();
            SpellsCondition = new Dictionary<BSpell, List<FightCondition>>();
            TargetsCondition = new Dictionary<string,List<FightCondition>>();
            m_cond = new List<FightCondition>();
            m_flag = "";
            ParseAI();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get the spell to launch and the associated target.
        /// </summary>
        public Dictionary<BSpell, BFighter> GetPlan()
        {
            Dictionary<BSpell, BFighter> result = new Dictionary<BSpell, BFighter>();
            foreach (KeyValuePair<BSpell,int> pair in spells)
            {
                string t = GetTargetAssociatedTo(pair.Key);
                if (t.StartsWith("#"))
                    target = GetSpecialTarget(t);
                else if (t != "")
                    target = GetTargetFromName(t);
                else
                    target = GetTarget();
                List<FightCondition> cond = SpellsCondition[pair.Key];
                if (CheckConditions(cond))
                {
                    result.Add(pair.Key, target);
                }
                else
                    continue;
            }
            return result;
        }

        /// <summary>
        /// Returns the strategy to use.
        /// </summary>
        public TacticEnum GetStrategy()
        {
            foreach (KeyValuePair<TacticEnum, int> pair in strategy)
            {
                switch (pair.Key)
                {
                    case TacticEnum.Immobile:
                        return TacticEnum.Immobile;
                    case TacticEnum.Barbare:
                        return TacticEnum.Barbare;
                    case TacticEnum.Fuyard:
                        if (m_Account.FightData.DistanceFrom() < pair.Value)
                            return TacticEnum.Fuyard;
                        else
                            return TacticEnum.Barbare;
                }
            }
            return TacticEnum.Immobile;
        }

        /// <summary>
        /// Returns positioning to use.
        /// </summary>
        public PlacementEnum GetPositioning()
        {
            if (positions.Count > 0)
                return positions[0];
            else
                return PlacementEnum.Immobile;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Parse the ai's file.
        /// </summary>
        private void ParseAI()
        {
            try
            {
                if (!File.Exists(path))
                    return;
                StreamReader sr = new StreamReader(path, Encoding.Default);
                string line = "";
                //ActionsStack = new List<Action>();

                while (sr.Peek() > 0)
                {
                    line = sr.ReadLine().Trim();
                    if (line == "" || line == string.Empty || line == null || line.StartsWith("//") || line.StartsWith("@"))
                        continue;
                    if (line.Contains("+Condition "))
                    {
                        ParseCondition(line);
                        continue;
                    }
                    if (line.Contains("EndCondition"))
                    {
                        m_cond.Clear();
                        continue;
                    }
                    if (flags.ToList().Find(d => d == line) != null)
                    {
                        m_flag = line;
                        continue;
                    }
                    if (Endflags.ToList().Find(d => d == line) != null)
                    {
                        m_flag = "";
                        continue;
                    }
                    //foreach (string f in flags)
                    //{
                    //    if (line.Contains(f))
                    //    {
                    //        m_flag = f;
                    //        continue;
                    //    }
                    //}
                    //foreach (string f in Endflags)
                    //{
                    //    if (line.Contains(f))
                    //    {
                    //        m_flag = "";
                    //        continue;
                    //    }
                    //}
                    ParseLine(line);
                }
                sr.Close();
                m_Account.Log(new BotTextInformation("IA chargée avec succès."), 3);
            }
            catch (Exception ex)
            {
                m_Account.Log(new ErrorTextInformation("Erreur lors du parsing de l'IA. " + ex.Message + ex.StackTrace), 0);
            }
        }

        /// <summary>
        /// Parse a line and fill the differents fields
        /// </summary>
        private void ParseLine(string line)
        {
            int i = -1;
            try
            {
                line = line.Trim();
                switch (m_flag)
                {
                    case "<Spells>":
                        i = line.IndexOf('=');
                        if (i != -1)
                        {
                            string[] splitted = line.Split('=');
                            i = Int32.Parse(splitted[1]);
                            line = splitted[0];
                        }
                        BSpell spell = new BSpell(GetSpellIdFromName(line), line);
                        spells.Add(spell, i);
                        SpellsCondition.Add(spell, m_cond);
                        break;
                    case "<Targets>":
                        i = line.IndexOf('=');
                        if (i != -1)
                        {
                            string[] splitted = line.Split('=');
                            i = Int32.Parse(splitted[1]);
                            line = splitted[0];
                        }
                        targets.Add(line, i);
                        TargetsCondition.Add(line, m_cond);
                        break;
                    case "<Strategy>":
                        i = line.IndexOf('=');
                        if (i != -1)
                        {
                            string[] splitted = line.Split('=');
                            i = Int32.Parse(splitted[1]);
                            line = splitted[0];
                        }
                        TacticEnum tactic = (TacticEnum)Enum.Parse(typeof(TacticEnum), line);
                        if (Enum.IsDefined(typeof(TacticEnum), tactic) | tactic.ToString().Contains(","))
                            strategy.Add(tactic, i);
                        else
                            throw new Exception("AI Script : Invalid strategy at line : " + line);
                        break;
                    case "<Position>":
                        PlacementEnum position = (PlacementEnum)Enum.Parse(typeof(PlacementEnum), line);
                        if (Enum.IsDefined(typeof(PlacementEnum), position) | position.ToString().Contains(","))
                            positions.Add(position);
                        else
                            throw new Exception("AI Script : Invalid position at line : " + line);
                        break;
                    case "<Challenges>":
                        ChallengeEnum challenge = (ChallengeEnum)Enum.Parse(typeof(ChallengeEnum), line);
                        if (Enum.IsDefined(typeof(ChallengeEnum), challenge) | challenge.ToString().Contains(","))
                            challenges.Add(challenge);
                        else
                            throw new Exception("AI Script : Invalid challenge at line : " + line);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Parse a condition's line
        /// </summary>
        private void ParseCondition(string line)
        {
            line = line.Remove(0, 10);
            line = line.Trim();
            foreach (char op in operateurs)
            {
                if (line.IndexOf(op) != -1)
                {
                    FightConditionEnum e = FightConditionEnum.Null;
                    string b = line.Substring(0, line.IndexOf(op));
                    FightConditionEnum condition = (FightConditionEnum)Enum.Parse(typeof(FightConditionEnum), b);
                    if (Enum.IsDefined(typeof(FightConditionEnum), condition) | condition.ToString().Contains(","))
                    {
                        line = line.Remove(0, line.IndexOf(op) + 1);
                        FightCondition c = new FightCondition(condition, line, op, m_Account);
                        m_cond.Add(c);
                    }
                    else
                        throw new Exception("AI Script : Invalid strategy at line : " + line);
                    return;
                }
            }
        }

        /// <summary>
        /// Get the spellid from the spell's name using I18n.
        /// </summary>
        private int GetSpellIdFromName(string name)
        {
            DataClass[] datas = GameData.GetDataObjects(D2oFileEnum.Spells);
            foreach (DataClass d in datas)
            {
                if (I18N.GetText((int)d.Fields["nameId"]).ToUpper() == name.ToUpper())
                    return (int)d.Fields["id"];
            }
            return -1;
        }

        /// <summary>
        /// Convert a special target to a fighter.
        /// </summary>
        private BFighter GetSpecialTarget(string target)
        {
            target = target.Replace("#", "").Replace(" ", "").Trim();
            switch (target)
            {
                case "NearestMonster":
                    return m_Account.FightData.NearestMonster();
                case "WeakestMonster":
                    return m_Account.FightData.WeakestMonster();
                case "Self":
                    return m_Account.FightData.Fighter;
                case "FreeCell":
                    return new BFighter(0, m_Account.FightData.NearestCellFrom(), 0, null, false, 0, 0, 0, 0, 0);
                case "NearestAlly":
                    return m_Account.FightData.NearestAlly();
                case "WeakestAlly":
                    return m_Account.FightData.WeakestAlly();
                case "Summon":
                    return new BFighter(0, m_Account.FightData.NearestCellFrom(m_Account.FightData.Fighter), 0, null, false, 0, 0, 0, 0, 0);
            }
            return null;
        }

        /// <summary>
        /// Returns the target associated to a spell.
        /// </summary>
        private string GetTargetAssociatedTo(BSpell spell)
        {
            int id = spells[spell];
            if (id == -1)
                return "";
            foreach (KeyValuePair<string,int> pair in targets)
            {
                if (pair.Value == id)
                    return pair.Key;
            }
            return "";
        }

        /// <summary>
        /// Returns the target.
        /// </summary>
        private BFighter GetTarget()
        {
            BFighter target = null;
            foreach (KeyValuePair<string, int> pair in targets)
            {
                List<FightCondition> cond = TargetsCondition[pair.Key];
                if (!CheckConditions(cond))
                    continue;
                else
                {
                    if (pair.Key.StartsWith("#"))
                        target = GetSpecialTarget(pair.Key);
                    else if (pair.Key != "")
                        target = GetTargetFromName(pair.Key);
                }
            }
            if (target == null)
                target = m_Account.FightData.NearestMonster();
            return target;
        }

        /// <summary>
        /// Returns the target from its name.
        /// </summary>
        private BFighter GetTargetFromName(string name)
        {
           List<BFighter> result = m_Account.FightData.Fighters.FindAll(t => t.Name == name);
           return m_Account.FightData.NearestMonster(result);
        }

        /// <summary>
        /// Returns true if all the conditions are respected, false otherwise.
        /// </summary>
        private bool CheckConditions(List<FightCondition> cond)
        {
            foreach (FightCondition c in cond)
            {
                if (!c.CheckCondition())
                    return false;
            }
            return true;
        }
        #endregion

    }
}
