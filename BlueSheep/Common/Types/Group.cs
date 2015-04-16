using BlueSheep.Interface;
using System.Collections.Generic;
namespace BlueSheep.Common.Types
{
    public class Group
    {
        #region Fields
        public List<AccountUC> accounts;
        public string name;
        #endregion

        #region Constructeurs
        public Group(List<AccountUC> list, string namee)
        {
            accounts = list;
            name = namee;
        }
        #endregion

        #region Publics methods
        public AccountUC GetMaster()
        {
            foreach (AccountUC a in accounts)
            {
                if (a.IsMaster == true)
                    return a;
            }
            return null;
        }

        public void MoveGroup(string move)
        {
            foreach (AccountUC a in accounts)
            {
                if (a.IsMaster == true)
                {
                    a.Map.ChangeMap(move);
                    foreach (AccountUC ac in accounts)
                    {
                        if (ac.IsSlave == true)
                            ac.Map.ChangeMap(move);
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
        }

        public void UseItemGroup(int uid)
        {
            foreach (AccountUC a in accounts)
            {
                if (a.IsMaster == true)
                {
                    a.Inventory.UseItem(uid);
                    foreach (AccountUC ac in accounts)
                    {
                        if (ac.IsSlave == true)
                            ac.Inventory.UseItem(uid);
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
        }

        public void UseZaapGroup()
        {
            foreach (AccountUC a in accounts)
            {
                if (a.IsMaster == true)
                {
                    a.Map.useZaap((int)a.Path.Current_Action.m_delta);
                    foreach (AccountUC ac in accounts)
                    {
                        if (ac.IsSlave == true)
                            ac.Map.useZaap((int)a.Path.Current_Action.m_delta);
                        System.Threading.Thread.Sleep(500);
                    }
                    return;
                }
            }
        }

        public void UseZaapiGroup()
        {
            foreach (AccountUC a in accounts)
            {
                if (a.IsMaster == true)
                {
                    a.Map.useZaapi((int)a.Path.Current_Action.m_delta);
                    foreach (AccountUC ac in accounts)
                    {
                        if (ac.IsSlave == true)
                            ac.Map.useZaapi((int)a.Path.Current_Action.m_delta);
                        System.Threading.Thread.Sleep(500);
                    }
                    return;
                }
            }
        }

        public void MoveToCellGroup(int delta)
        {
            foreach (AccountUC a in accounts)
            {
                if (a.IsMaster == true)
                {
                    a.Map.MoveToCell(delta);
                    foreach (AccountUC ac in accounts)
                    {
                        if (ac.IsSlave == true)
                            ac.Map.MoveToCell(delta);
                        System.Threading.Thread.Sleep(500);
                    }
                    return;
                }
            }
        }

        public void MoveToElementGroup(int delta)
        {
            foreach (AccountUC a in accounts)
            {
                if (a.IsMaster == true)
                {
                    a.Map.MoveToSecureElement(delta);
                    foreach (AccountUC ac in accounts)
                    {
                        if (ac.IsSlave == true)
                            ac.Map.MoveToSecureElement(delta);
                        System.Threading.Thread.Sleep(500);
                    }
                    return;
                }
            }
        }

        public void TalkToNpcGroup(int delta)
        {
            foreach (AccountUC a in accounts)
            {
                if (a.IsMaster == true)
                {
                    a.Npc.TalkToNpc(delta);
                    foreach (AccountUC ac in accounts)
                    {
                        if (ac.IsSlave == true)
                            ac.Npc.TalkToNpc(delta);
                        System.Threading.Thread.Sleep(500);
                    }
                    return;
                }
            }
        }

        public void RequestExchangeGroup(string delta)
        {
            foreach (AccountUC a in accounts)
            {
                if (a.IsMaster == true)
                {
                    a.Inventory.RequestExchange(delta);
                    foreach (AccountUC ac in accounts)
                    {
                        if (ac.IsSlave == true)
                            ac.Inventory.RequestExchange(delta);
                        System.Threading.Thread.Sleep(2000);
                    }
                    return;
                }
            }
        }
    #endregion

    }
}