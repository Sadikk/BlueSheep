using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlueSheep.Core.Path
{
    public class PathManager
    {
        #region Fields
        private AccountUC Account;
        public string path;
        //public bool Stop;
        private string flag;
        private List<Condition> conditions;
        public List<Action> ActionsStack;
        public Action Current_Action;
        public string Current_Map;
        public string Current_Flag
        {
            get { return flag; }
            internal set { flag = value; }
        }
        public Thread Thread;
        public bool Launched;

        public static readonly IList<String> flags = new ReadOnlyCollection<string>
        (new List<String> {"<Move>","<Fight>","<Gather>","<Dialog>"});

        public static readonly IList<String> Endflags = new ReadOnlyCollection<string>
        (new List<String> { "</Move>", "</Fight>", "</Gather>", "</Dialog>" });

        public static readonly IList<String> Actions = new ReadOnlyCollection<string>
        (new List<String> { "npc(","cell(", "object(", "zaap(", "zaapi(", "use(", "move(" });

        public static readonly IList<Char> operateurs = new ReadOnlyCollection<char>
        (new List<Char> { '<', '>', '=' });
        #endregion

        public string pathBot;

        #region Constructeurs
        public PathManager(AccountUC account, string Path, string name)
        {
            Account = account;
            path = Path;
            Account.PathDownBt.Text = name;
            Thread = new Thread(new ThreadStart(ParsePath));
        }

        #endregion

        #region Public methods
        public void Start()
        {
            Thread = new Thread(new ThreadStart(ParsePath));
            Thread.Start();
            Launched = true;
        }

        public void Stop()
        {
            this.Thread.Interrupt();
            Launched = false;
        }

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

        public void PerformActionsStack()
        {
            if (ActionsStack.Count() == 0)
                return;
            for (int i = 0; i < ActionsStack.Count(); i++)
            {
                Current_Action = ActionsStack[i];
                ActionsStack[i].PerformAction();           
            }
            ActionsStack.Clear();
        }
        #endregion

        #region Private methods

        private void AnalyseLine(string line)
        {
            if (Account.Map.Data == null)
            {
                Account.Log(new ErrorTextInformation("Le bot n'a pas encore reçu les informations de la map, veuillez patienter. Si le problème persiste, rapportez le bug sur le forum : http://forum.bluesheepbot.com"),0);
                return;
            }
            if (line.Contains("#"))
                return;
            if (CheckConditions(true) && flag != "")
            {
                //string[] move = line.Split(' ');
                ParseAction(line);
                PerformFlag(flag);
            }
        }

        private void PerformFlag(string flag)
        {
            switch (flag)
            {
                case "<Move>":
                    //Aucune action spécifique au flag, on éxécute directement les actions
                    if (Account.IsMaster == true && Account.MyGroup != null)
                    {
                        PerformActionsStack();
                        //Account.Path.Stop = true;
                    }
                    else if (Account.IsSlave == false)
                    {
                        PerformActionsStack();
                        //Account.Path.Stop = true;
                    }
                    else
                    {
                        Account.Log(new ErrorTextInformation("Impossible d'enclencher le déplacement. (mûle ?)"),0);
                    }
                    break;
                case "<Fight>":
                    //On lance un combat, les actions seront effectuées après le combat
                    if (Account.IsMaster == true && Account.MyGroup != null && Account.Fight != null)
                    {
                        if (Account.Fight.SearchFight() == false)
                            PerformActionsStack();
                        //Account.Path.Stop = true;
                    }
                    else if (Account.IsSlave == false && Account.Fight != null)
                    {
                        if (Account.Fight.SearchFight() == false)
                            PerformActionsStack();
                       //Account.Path.Stop = true;
                    }
                    else
                    {
                        Account.Log(new ErrorTextInformation("Impossible d'enclencher le déplacement. (mûle ? Aucune IA ?)"),0);
                    }
                    break;
                case "<Gather>":
                    //On récolte la map, les actions seront effectuées après la récolte
                    //Account.Log(new ErrorTextInformation("La récolte n'est pas encore implémentée, veuillez attendre la mise à jour. Tenez vous au courant sur http://forum.bluesheepbot.com "),0);
                    if (Account.PerformGather() == false)
                        PerformActionsStack();
                    //Account.Path.Stop = true;
                    break;
                //case "<Bank>":
                //    //On rajoute la condition pods et on effectue l'action
                //    Condition c = new Condition(ConditionEnum.PodsPercent, (int)Account.NUDPods.Value, '>', Account);
                //    if (c.CheckCondition() == true)
                //        PerformActionsStack();
                //    Account.Path.Stop = true;
                //    break;
            }
        }

        private void ParseCondition(string line)
        {
            line = line.Remove(0, 10);
            line = line.Trim();
            foreach (char op in operateurs)
            {
                if (line.IndexOf(op) != -1)
                {
                    ConditionEnum e = ConditionEnum.Null;
                    string b = line.Substring(0, line.IndexOf(op));
                    switch (b)
                    {
                        case "Aucune":
                            e = ConditionEnum.Null;
                            break;
                        case "LastMap":
                            e = ConditionEnum.LastMapId;
                            break;
                        case "Level":
                            e = ConditionEnum.Level;
                            break;
                        case "Pods":
                            e = ConditionEnum.Pods;
                            break;
                        case "%Pods":
                            e = ConditionEnum.PodsPercent;
                            break;
                    }
                    line = line.Remove(0,line.IndexOf(op) + 1);
                    Condition c = new Condition(e, line, op, Account);
                    conditions.Add(c);
                    return;
                }
            }
        }

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

        private bool CheckConditions(bool analysed)
        {
            foreach (Condition c in conditions)
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

        private void ParsePath()
        {
            if (!File.Exists(path))
                return;
            StreamReader sr = new StreamReader(path);
            string line = "";
            conditions = new List<Condition>();
            ActionsStack = new List<Action>();

            while (sr.Peek() > 0)
            {
                //if (Stop == true)
                //{
                //    sr.Close();
                //    return;
                //}
                line = sr.ReadLine();
                if (line == "" || line == string.Empty || line == null || line.StartsWith("#"))
                    continue;
                if (Account.Map.Data == null)
                {
                    sr.Close();
                    Account.Log(new ErrorTextInformation("Le bot n'a pas encore reçu les informations de la map, veuillez patienter. Si le problème persiste, rapportez le bug sur le forum : http://forum.bluesheepbot.com"), 0);
                    return;
                }
                if (line.Contains("+Condition "))
                {
                    ParseCondition(line);
                    continue;
                }
                if ((line.Contains(Account.Map.X.ToString() + "," + Account.Map.Y.ToString() + ":") || line.Contains(Account.Map.Id.ToString() + ":")) && CheckConditions(false))
                {
                    Current_Map = Account.Map.X.ToString() + "," + Account.Map.Y.ToString();
                    AnalyseLine(line);
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
            sr.Close();
        }
        #endregion


    }
}
