using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BlueSheep.Core.Path
{
    public class PathManager
    {
        #region Fields
        private AccountUC Account;
        public string path;
        private string flag;
        public bool Relaunch = false;
        private List<PathCondition> conditions;
        public List<Action> ActionsStack;
        public Action Current_Action;
        public string Current_Map;
        public string Current_Flag
        {
            get { return flag; }
            internal set { flag = value; }
        }
        private List<string> m_content;
        public string pathBot;
        public Thread Thread;
        public bool Launched;
        private bool Stop;

        public static readonly IList<String> flags = new ReadOnlyCollection<string>
        (new List<String> {"<Move>","<Fight>","<Gather>","<Dialog>"});

        public static readonly IList<String> Endflags = new ReadOnlyCollection<string>
        (new List<String> { "</Move>", "</Fight>", "</Gather>", "</Dialog>" });

        public static readonly IList<String> Actions = new ReadOnlyCollection<string>
        (new List<String> { "exchange(", "npc(","cell(", "object(", "zaap(", "zaapi(", "use(", "move(" });

        public static readonly IList<Char> operateurs = new ReadOnlyCollection<char>
        (new List<Char> { '<', '>', '=' });
        #endregion

        

        #region Constructors
        public PathManager(AccountUC account, string Path, string name)
        {
            Account = account;
            path = Path;
            Account.PathDownBt.Text = name;
            m_content = File.ReadAllLines(Path).ToList();
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Start the path's parsing
        /// </summary>
        public void Start()
        {
            Launched = true;
            Stop = false;
            ParsePath();
            Account.WatchDog.StartPathDog();
        }

        /// <summary>
        /// Stop the path and clear the current actions stack.
        /// </summary>
        public void StopPath()
        {
            Stop = true;
            Launched = false;
            ClearStack();
            Account.WatchDog.StopPathDog();
        }

        /// <summary>
        /// Search for the answer of a given npc's question in the path and send it.
        /// </summary>
        public void SearchReplies(string question)
        {
            if (Account.Npc != null)
            {
                StreamReader sr = new StreamReader(path);
                int flag = 0;
                while (sr.Peek() > 1)
                {
                    string line = sr.ReadLine();
                    if (line.Contains("</Dialog>"))
                        break;
                    if (line.Contains("<Dialog>"))
                        flag = 1;
                    if (flag == 1 && question.Contains(line.Split('|')[0]))
                    {
                        string[] l = line.Split('|');
                        foreach (BlueSheep.Core.Npc.NpcReply rep in Account.Npc.Replies)
                        {
                            string resp = rep.GetText();
                            if (resp.Contains(l[1]))
                            {
                                Account.Npc.SendReply(rep.Id);
                                Account.Log(new BotTextInformation("Envoi de la réponse : " + rep.GetText()),1);
                                sr.Close();
                                return;
                            }
                        }
                    }
                }
                sr.Close();
                Account.Log(new ErrorTextInformation("Aucune réponse disponible dans le trajet"),0);
            }
        }

        /// <summary>
        /// Perform the action associed with the current path's flag.
        /// </summary>
        public void PerformFlag()
        {
            if (Account.state == Engine.Enums.Status.Fighting)
                return;
            switch (Current_Flag)
            {
                case "<Move>":
                    //Aucune action spécifique au flag, on éxécute directement les actions
                    if (Account.IsMaster == true && Account.MyGroup != null)
                    {
                        PerformActionsStack();
                    }
                    else if (Account.IsSlave == false)
                    {
                        PerformActionsStack();
                    }
                    else
                    {
                        Account.Log(new ErrorTextInformation("Impossible d'enclencher le déplacement. (mûle ?)"), 0);
                    }
                    break;
                case "<Fight>":
                    //On lance un combat, les actions seront effectuées après le combat
                    if (Account.IsMaster == true && Account.MyGroup != null && Account.Fight != null)
                    {
                        if (!Account.Fight.SearchFight())
                            PerformActionsStack();
                    }
                    else if (Account.IsSlave == false && Account.Fight != null)
                    {
                        if (Account.Fight.SearchFight() == false)
                            PerformActionsStack();
                    }
                    else
                    {
                        Account.Log(new ErrorTextInformation("Impossible d'enclencher le déplacement. (mûle ? Aucune IA ?)"), 0);
                    }
                    break;
                case "<Gather>":
                    //On récolte la map, les actions seront effectuées après la récolte
                    if (!Account.PerformGather())
                        PerformActionsStack();
                    break;
            }
            Account.WatchDog.Update();
        }

        /// <summary>
        /// Clear the current action stack.
        /// </summary>
        public void ClearStack()
        {
            if (ActionsStack != null)
                ActionsStack.Clear();
        }

        /// <summary>
        /// Parse the path's file.
        /// </summary>
        public void ParsePath()
        {
            //if (!File.Exists(path))
            //    return;
            //StreamReader sr = new StreamReader(path);
            //string line = "";
            conditions = new List<PathCondition>();
            ActionsStack = new List<Action>();

            //while (sr.Peek() > 0)
            //{
            foreach (string line in m_content)
            {
                //line = sr.ReadLine();
                if (line == "" || line == string.Empty || line == null || line.StartsWith("#"))
                    continue;
                if (line.Contains("+Condition "))
                {
                    ParseCondition(line);
                    continue;
                }
                if (((line.Contains(Account.MapData.Pos + ":") && CheckMinusNumber(line)) || line.Contains(Account.MapData.Id.ToString() + ":")) && CheckConditions(false))
                {
                    Current_Map = Account.MapData.Pos;
                    AnalyseLine(line);
                    //sr.Close(); 
                    return;
                }
                foreach (string f in flags)
                {
                    if (line.Contains(f))
                    {
                        flag = f;
                        continue;
                    }
                }
                foreach (string f in Endflags)
                {
                    if (line.Contains(f))
                    {
                        conditions.Clear();
                        flag = "";
                        continue;
                    }
                }
            }
            //sr.Close();
            Lost();
        }

        /// <summary>
        /// Pull the last action from the stack and perform it.
        /// </summary>
        public void PerformActionsStack()
        {
            if (ActionsStack.Count() == 0 || Stop)
                return;
            while (ActionsStack.Count() > 0)
            {
                Current_Action = ActionsStack[0];
                ActionsStack[0].PerformAction();
                ActionsStack.Remove(Current_Action);
            }
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Parse the line with the current pos.
        /// </summary>
        private void AnalyseLine(string line)
        {
            if (line.Contains("#"))
                return;
            if (CheckConditions(true) && flag != "")
            {
                ParseAction(line);
                PerformFlag();
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
                    PathConditionEnum e = PathConditionEnum.Null;
                    string b = line.Substring(0, line.IndexOf(op));
                    switch (b)
                    {
                        case "Aucune":
                            e = PathConditionEnum.Null;
                            break;
                        case "LastMap":
                            e = PathConditionEnum.LastMapId;
                            break;
                        case "Level":
                            e = PathConditionEnum.Level;
                            break;
                        case "Pods":
                            e = PathConditionEnum.Pods;
                            break;
                        case "%Pods":
                            e = PathConditionEnum.PodsPercent;
                            break;
                        case "Alive":
                            e = PathConditionEnum.Alive;
                            break;
                    }
                    line = line.Remove(0,line.IndexOf(op) + 1);
                    PathCondition c = new PathCondition(e, line, op, Account);
                    conditions.Add(c);
                    return;
                }
            }
        }

        /// <summary>
        /// Parse an action's line
        /// </summary>
        private void ParseAction(string line)
        {
            if (line.IndexOf(':') != -1)
            {
                line = line.Remove(0, line.IndexOf(':') + 1);              
            }
            line = line.Trim();
            foreach (string s in Actions)
            {
                int index = line.IndexOf(s);
                if (index != -1 && line.IndexOf('+') != -1)
                {
                    line = line.Remove(0, s.Length);
                    // on supprime de la ligne la string d'action
                    Action a = new Action(s, line.Substring(0, line.IndexOf('+')), Account);
                    ActionsStack.Add(a);
                    line = line.Remove(0, line.IndexOf('+') + 1);
                    ParseAction(line);
                    return;
                }
                else if (index != -1)
                {
                    line = line.Remove(0, s.Length);
                    // on supprime de la ligne la string d'action
                    Action a = new Action(s, line.Substring(0, line.IndexOf(')')), Account);
                    ActionsStack.Add(a);
                    return;
                }
                
            }

        }

        /// <summary>
        /// Check if all the conditions in the conditions are respected or not.
        /// </summary>
        private bool CheckConditions(bool analysed)
        {
            foreach (PathCondition c in conditions)
            {
                if (c.CheckCondition() == false)
                {
                    if (analysed)
                        Account.Log(new BotTextInformation("Trajet : Condition non respectée"), 5);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if the line is with the same number of '-' char as the position.
        /// </summary>
        private bool CheckMinusNumber(string line)
        {
            if (!line.Contains(','))
                return true;
            line = line.Remove(line.IndexOf(':'));
            int n = line.ToList().FindAll(c => c == '-').Count;
            int comp = Account.MapData.Pos.ToList().FindAll(c => c == '-').Count;
            return n == comp;
        }

        /// <summary>
        /// Alert the user that we are lost.
        /// </summary>
        private void Lost()
        {
            Account.Log(new ErrorTextInformation("Aucune action disponible dans le trajet, le bot est perdu."), 0);
            StopPath();
        }
        #endregion


    }
}
