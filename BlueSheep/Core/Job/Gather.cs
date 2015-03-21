using BlueSheep.Common.Data;
using BlueSheep.Core.Map.Elements;
using BlueSheep.Data.Pathfinding.Positions;
using BlueSheep.Engine.Enums;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Core
{
    public class Gather
    {
        #region Fields
        private AccountUC account = null;
        public bool IsFishing { get; set; }
        public bool Moved { get; set; }
        private int _Id = -1;
        public string resourceName
        {
            get
            {
                return Current_El.Name;
            }
        }
        public JobUC Current_Job;
        public Dictionary<string, int> Stats = new Dictionary<string, int>();
        public InteractiveElement Current_El;
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }
        private int _SkillInstanceUid = -1;
        public int SkillInstanceUid
        {
            get
            {
                return _SkillInstanceUid;
            }
            set
            {
                _SkillInstanceUid = value;
            }
        }
        #endregion


        public Gather(AccountUC Account)
        {
            account = Account;
        }

        public bool GoGather(List<int> ressources, JobUC job)
        {
            List<int> ListeRessourcesID = ressources;
            List<int> ListDistance = new List<int>();
            List<UsableElement> ListUsableElement = new List<UsableElement>();
            Current_Job = job;
            try
            {
                if (ListeRessourcesID.Count > 0)
                {
                    foreach (var RessourceID in ListeRessourcesID)
                    {
                        foreach (var UsableElement in account.Map.UsableElements)
                        {
                            foreach (InteractiveElement InteractiveElement in account.Map.InteractiveElements.Values)
                            {
                                if (UsableElement.Value.Element.Id == InteractiveElement.Id && InteractiveElement.IsUsable)
                                {
                                    if (InteractiveElement.TypeId == RessourceID && account.Map.NoEntitiesOnCell(UsableElement.Value.CellId))
                                    {
                                        ListUsableElement.Add(UsableElement.Value);
                                        ListDistance.Add(GetRessourceDistance((int)UsableElement.Value.Element.Id));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var UsableElement in account.Map.UsableElements)
                    {
                        foreach (InteractiveElement InteractiveElement in account.Map.InteractiveElements.Values)
                        {
                            if (UsableElement.Value.Element.Id == InteractiveElement.Id && InteractiveElement.IsUsable)
                            {
                                if (account.Map.NoEntitiesOnCell(UsableElement.Value.CellId))
                                {
                                    ListUsableElement.Add(UsableElement.Value);
                                    ListDistance.Add(GetRessourceDistance((int)UsableElement.Value.Element.Id));
                                }
                            }
                        }
                    }
                }

                if (ListDistance.Count > 0)
                {
                    foreach (UsableElement UsableElement in TrierDistanceElement(ListDistance, ListUsableElement))
                    {
                        if (UsableElement.Element.IsUsable == false)
                            continue;
                        Id = (int)UsableElement.Element.Id;
                        SkillInstanceUid = UsableElement.Skills[0].skillInstanceUid;
                        Current_El = UsableElement.Element;
                        //resourceName = UsableElement.Element.TypeId;
                        int distance = GetRessourceDistance((int)UsableElement.Element.Id);
                        if (distance == 1 || (IsFishing && account.Inventory.WeaponRange >= distance))
                        {
                            if (Moved)
                            {
                                account.Map.UseElement(Id, SkillInstanceUid);
                            }
                            else
                            {
                                account.Map.UseElement((int)UsableElement.Element.Id, UsableElement.Skills[0].skillInstanceUid);
                            }
                            Moved = false;
                            IsFishing = false;
                            account.SetStatus(Status.Gathering);
                            return true;
                        }
                        else if ((account.Inventory.HasFishingRod == false && account.Map.MoveToElement((int)UsableElement.Element.Id, 1)) || (account.Inventory.HasFishingRod == true && account.Map.MoveToElement((int)UsableElement.Element.Id, account.Inventory.WeaponRange)))
                        {
                            account.SetStatus(Status.Gathering);
                            Id = (int)UsableElement.Element.Id;
                            SkillInstanceUid = UsableElement.Skills[0].skillInstanceUid;
                            if (account.Inventory.HasFishingRod)
                                IsFishing = true;
                            else
                                IsFishing = false;
                            return true;                    
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                account.Log(new ErrorTextInformation(Ex.Message), 0);
            }
            Id = -1;
            SkillInstanceUid = -1;
            IsFishing = false;
            Current_El = null;
            return false;
        }

        public List<UsableElement> TrierDistanceElement(List<int> ListDistance, List<UsableElement> ListUsableElement)
        {
            int ListLength = 0;
            bool InOrder = false;
            object TimeToAccess = null;

            ListLength = ListDistance.Count;
            while (!InOrder)
            {
                InOrder = true;
                for (var i = 0; i <= ListLength - 2; i++)
                {
                    if (ListDistance[i] > ListDistance[i + 1])
                    {
                        TimeToAccess = ListDistance[i];
                        ListDistance[i] = ListDistance[i + 1];
                        ListDistance[i + 1] = Convert.ToInt32(TimeToAccess);
                        TimeToAccess = ListUsableElement[i];
                        ListUsableElement[i] = ListUsableElement[i + 1];
                        ListUsableElement[i + 1] = (UsableElement)TimeToAccess;
                        InOrder = false;
                    }
                }
                ListLength = ListLength - 1;
            }

            return ListUsableElement;
        }


        private int GetRessourceDistance(int Id)
        {
            MapPoint CharacterMapPoint = new MapPoint(account.Map.Character.CellId);
            StatedElement StatedRessource = account.Map.StatedElements.FirstOrDefault((se) => se.Value.Id == Id).Value;
            if (StatedRessource != null)
            {
                MapPoint RessourceMapPoint = new MapPoint((int)StatedRessource.CellId);
                return CharacterMapPoint.DistanceTo(RessourceMapPoint);
            }
            return -1;
        }
    }
}
