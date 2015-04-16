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
        private int _Id = -1;
        private List<int> m_BannedId = new List<int>();

        /// <summary>
        /// Return the name of Current_El.
        /// </summary>
        public string resourceName
        {
            get
            {
                return Current_El.Name;
            }
        }
        public JobUC Current_Job;
        
        /// <summary>
        /// Store the gathering stats.
        /// </summary>
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
        private int _Error;
        #endregion


        public Gather(AccountUC Account)
        {
            account = Account;
        }

        /// <summary>
        /// Perform the gathering of the specified ressources.
        /// </summary>
        /// <param name="ressources">
        /// List of the ressources'id.
        /// </param>
        /// /// <param name="job">
        /// The job used.
        /// </param>
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
                        foreach (var UsableElement in account.MapData.UsableElements)
                        {
                            foreach (InteractiveElement InteractiveElement in account.MapData.InteractiveElements.Keys)
                            {
                                if (UsableElement.Value.Element.Id == InteractiveElement.Id && InteractiveElement.IsUsable)
                                {
                                    if (InteractiveElement.TypeId == RessourceID && account.MapData.NoEntitiesOnCell(UsableElement.Value.CellId))
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
                    foreach (var UsableElement in account.MapData.UsableElements)
                    {
                        foreach (InteractiveElement InteractiveElement in account.MapData.InteractiveElements.Keys)
                        {
                            if (UsableElement.Value.Element.Id == InteractiveElement.Id && InteractiveElement.IsUsable)
                            {
                                if (account.MapData.NoEntitiesOnCell(UsableElement.Value.CellId))
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
                        if (UsableElement.Element.IsUsable == false || m_BannedId.Contains((int)UsableElement.Element.Id))
                            continue;
                        Id = (int)UsableElement.Element.Id;
                        SkillInstanceUid = UsableElement.Skills[0].skillInstanceUid;
                        Current_El = UsableElement.Element;
                        int distance = GetRessourceDistance((int)UsableElement.Element.Id);
                        account.Log(new DebugTextInformation("[Gather] Distance from element " + UsableElement.Element.Id + " = " + distance), 0);
                        if (distance == -1)
                        {
                            continue;
                        }
                        if (account.MapData.CanGatherElement(Id, distance))
                        {
                            account.SetStatus(Status.Gathering);
                            account.Map.UseElement(Id, SkillInstanceUid);
                            return true;
                        }
                        else if (account.Map.MoveToElement((int)UsableElement.Element.Id, account.Inventory.WeaponRange))
                        {
                            return true;
                        }
                        else
                            continue;
                    }
                }
            }
            catch (Exception Ex)
            {
                account.Log(new ErrorTextInformation(Ex.Message), 0);
            }
            Id = -1;
            SkillInstanceUid = -1;
            Current_El = null;
            return false;
        }

        /// <summary>
        /// Increase the error count and continue the path if it overtakes a step.
        /// </summary>
        public bool Error()
        {
            _Error++;
            if (_Error > 1 && account.Path != null)
            {
                account.Path.PerformActionsStack();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Forbid an element id that is unreachable actually.
        /// </summary>
        public void BanElementId(int id)
        {
            m_BannedId.Add(id);
            account.Log(new DebugTextInformation("[Gather] Element id " + id + " banned."), 0);
        }

        /// <summary>
        /// Set the error amount to 0.
        /// </summary>
        public void ClearError()
        {
            _Error = 0;
            m_BannedId.Clear();
        }

        /// <summary>
        /// Sort the distance from a list.
        /// </summary>
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

        /// <summary>
        /// Get the distance between the character and the ressource.
        /// </summary>
        /// <param name="Id">
        /// The id of the ressource.
        /// </param>
        public int GetRessourceDistance(int Id)
        {
            MapPoint CharacterMapPoint = new MapPoint(account.MapData.Character.disposition.cellId);
            StatedElement StatedRessource = account.MapData.StatedElements.FirstOrDefault((se) => se.Id == Id);
            if (StatedRessource != null)
            {
                MapPoint RessourceMapPoint = new MapPoint((int)StatedRessource.CellId);
                return CharacterMapPoint.DistanceTo(RessourceMapPoint);
            }
            return -1;
        }
    }
}
