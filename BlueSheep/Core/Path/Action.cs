using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Core.Path
{
    public class Action
    {
        #region Fields
        string m_action;
        public object m_delta;
        AccountUC account;
        #endregion

        #region Constructors
        public Action(string action, object delta, AccountUC Account)
        {
            if (((string)delta).Contains(')'))
            {
                delta = ((string)delta).Substring(0, ((string)delta).Length - 1);
            }
            m_action = action;
            m_delta = delta;
            account = Account;
        }
        #endregion

        #region Public Methods
        public void PerformAction()
        {
            if (account.Path == null)
                return;
            while (account.Busy == true)
                account.Wait(1, 5);
            switch (m_action)
            {
                case "move(":
                    m_delta = RandomDir((string)m_delta);
                    if (account.IsMaster == true && account.MyGroup != null)
                    {
                        account.MyGroup.MoveGroup((string)m_delta);
                        account.Wait(2000, 3000);
                    }
                    else if (account.IsSlave == false)
                    {
                        account.Map.ChangeMap((string)m_delta);
                        account.Wait(2000, 3000);
                    }
                    else
                    {
                        account.Log(new ErrorTextInformation("Impossible d'enclencher le déplacement. (mûle ?)"),0);
                    }
                    break;
                case "object(":
                    if (account.IsMaster == true && account.MyGroup != null)
                    {
                        account.MyGroup.UseItemGroup(account.Inventory.GetItemFromGID(Convert.ToInt32(m_delta)).UID);
                        account.Wait(2000, 3000);
                    }
                    else if (account.IsSlave == false)
                    {
                        account.Inventory.UseItem(account.Inventory.GetItemFromGID(Convert.ToInt32(m_delta)).UID);
                    }
                    else
                    {
                        account.Log(new ErrorTextInformation("Impossible d'enclencher le déplacement. (mûle ? plus d'objet ?)"), 0);
                    }
                    break;
                case "cell(":
                    if (account.IsMaster == true && account.MyGroup != null)
                    {
                        account.MyGroup.MoveToCellGroup(Convert.ToInt32(m_delta));
                        account.Wait(2000, 3000);
                    }
                    else if (account.IsSlave == false)
                    {
                        account.Map.MoveToCell(Convert.ToInt32(m_delta));
                    }
                    else
                    {
                        account.Log(new ErrorTextInformation("Impossible d'enclencher le déplacement. (mûle ? plus d'objet ?)"), 0);
                    }       
                    account.Log(new BotTextInformation("Trajet : Déplacement sur la cellule " + Convert.ToString(m_delta)),5);
                    break;
                case "npc(":
                    if (account.IsMaster == true && account.MyGroup != null)
                    {
                        account.MyGroup.TalkToNpcGroup(Convert.ToInt32(m_delta));
                        account.Wait(2000, 3000);
                    }
                    else if (account.IsSlave == false)
                    {
                        account.Npc.TalkToNpc(Convert.ToInt32(m_delta));
                    }
                    else
                    {
                        account.Log(new ErrorTextInformation("Impossible d'enclencher le dialogue. (mûle ?)"), 0);
                    } 
                    break;
                    
                    break;
                case "use(":
                    account.Map.MoveToSecureElement(Convert.ToInt32(m_delta));
                    break;
                case "zaap(":
                    if (account.IsMaster == true && account.MyGroup != null)
                    {
                        account.MyGroup.UseZaapGroup();
                        account.Wait(2000, 3000);
                    }
                    else if (account.IsSlave == false)
                    {
                        account.Map.useZaap(Convert.ToInt32(account.Path.Current_Action.m_delta));
                    }
                    else
                    {
                        account.Log(new ErrorTextInformation("Impossible d'enclencher le déplacement. (mûle ? plus d'objet ?)"), 0);
                    }                         
                    break;
                case "zaapi(":
                    if (account.IsMaster == true && account.MyGroup != null)
                    {
                        account.MyGroup.UseZaapiGroup();
                        account.Wait(2000, 3000);
                    }
                    else if (account.IsSlave == false)
                    {
                        account.Map.useZaapi(Convert.ToInt32(account.Path.Current_Action.m_delta));
                    }
                    else
                    {
                        account.Log(new ErrorTextInformation("Impossible d'enclencher le déplacement. (mûle ? plus d'objet ?)"), 0);
                    } 
                    break;
                case "exchange(":
                    if (account.IsMaster == true && account.MyGroup != null)
                    {
                        account.MyGroup.RequestExchangeGroup((string)m_delta);
                        account.Wait(2000, 3000);
                    }
                    else if (account.IsSlave == false)
                    {
                        account.Inventory.RequestExchange((string)m_delta);
                    }
                    else
                    {
                        account.Log(new ErrorTextInformation("Impossible d'enclencher le dialogue. (mûle ?)"), 0);
                    } 
                    break;
            }
            account.WatchDog.Update();
        }
        #endregion

        #region Private Methods
        private string RandomDir(string delta)
        {
            string[] t = delta.Split(',');
            Random r = new Random();
            int pos = r.Next(0, t.Length);
            return t[pos];
        }
        #endregion
    }
}
