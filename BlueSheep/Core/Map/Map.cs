using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Core.Fight;
using BlueSheep.Core.Map.Elements;
using BlueSheep.Data.D2p.Elements;
using BlueSheep.Data.Pathfinding;
using BlueSheep.Data.Pathfinding.Positions;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Core.Map
{
    public class Map : BlueSheep.Data.D2p.Map
    {
        #region Fields
        public List<BlueSheep.Common.Types.MonsterGroup> List;
        public Dictionary<int, BlueSheep.Common.Protocol.Types.GameRolePlayCharacterInformations> Players;
        #endregion

        #region Properties
        public AccountUC m_Account { get; set; }
        public bool AutoTimeout { get; private set; }

        public Entity Character
        {
            get { return Entities.FirstOrDefault(p => p.Id == m_Account.CharacterBaseInformations.id); }
        }

        public BlueSheep.Data.D2p.Map Data { get; set; }
        public Dictionary<int, InteractiveElement> Doors { get; private set; }
        public List<Entity> Entities { get; private set; }
        public int LastMapId;

        public int Id
        {
            get { return (Data.Id); }
        }

        private string position;

        public string Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }

        private string zone;

        public string Zone
        {
            get { return zone; }
            set
            {
                zone = value;
            }
        }


        public Dictionary<int, InteractiveElement> InteractiveElements { get; private set; }
        public int MapChange { get; private set; }
        public Dictionary<int, StatedElement> StatedElements { get; private set; }
        public int SubAreaId { get; set; }
        public int TimeoutMax { get; set; }
        public int TimeoutMin { get; set; }
        public Dictionary<int, UsableElement> UsableElements
        {
            get
            {
                    Dictionary<int, UsableElement> usableElements = new Dictionary<int, UsableElement>();
                    foreach (var element in InteractiveElements)
                    {
                        Elements.InteractiveElement interactiveElement = (Elements.InteractiveElement)element.Value;
                        if (interactiveElement.EnabledSkills.Count >= 1)
                        {
                            foreach (var layer in ((BlueSheep.Data.D2p.Map)Data).Layers)
                            {
                                foreach (var cell in layer.Cells)
                                {
                                    foreach (var layerElement in cell.Elements)
                                    {
                                        if (layerElement is GraphicalElement)
                                        {
                                            GraphicalElement graphicalElement = (GraphicalElement)layerElement;
                                            if (graphicalElement.Identifier == interactiveElement.Id)
                                            {
                                                usableElements.Add((int)interactiveElement.Id, new Elements.UsableElement(cell.CellId, interactiveElement, interactiveElement.EnabledSkills.ToList()));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return usableElements;
                
            }
        }

#endregion

        #region Constructeurs
        public Map(AccountUC account)
        {
            m_Account = account;
            Entities = new List<Core.Fight.Entity>();
            InteractiveElements = new Dictionary<int, InteractiveElement>();
            StatedElements = new Dictionary<int, StatedElement>();
            Doors = new Dictionary<int, InteractiveElement>();
            List = new List<BlueSheep.Common.Types.MonsterGroup>();
        }

        #endregion

        #region Public methods
        public int WorldId
        {
            get 
            {
                DataClass mapp = GameData.GetDataObject(D2oFileEnum.MapPositions, Id);
                return (int)mapp.Fields["worldMap"];
            }
        }

        public int X
        {
            get
            {
                DataClass mapp = GameData.GetDataObject(D2oFileEnum.MapPositions, Id);
                return (int)mapp.Fields["posX"];
            }
        }

        public int Y
        {
            get
            {
                DataClass mapp = GameData.GetDataObject(D2oFileEnum.MapPositions, Id);
                return (int)mapp.Fields["posY"];
            }
        }

        public bool CanGatherElement(int id, int distance)
        {
            if (distance <= 1 && distance >= 0)
                return true;
            if (m_Account.Inventory.HasFishingRod)
            {
                MapPoint characterPoint = new MapPoint(Character.CellId);
                //Elements.StatedElement element = (Elements.StatedElement)StatedElements.GetValueOrNull(id);
                Elements.StatedElement element = null;
                if (StatedElements.ContainsKey(id))
                {
                    element = StatedElements[id];
                }
            
                if (element != null)
                {
                    //StatedElement selectedStatedElement = findValue(StatedElements, id);
                    //if (selectedStatedElement != null)
                    //    element = (Elements.StatedElement)selectedStatedElement;
                    MapPoint elementPoint = new MapPoint((int)element.CellId);
                    List<MapPoint> goodPointsList = GetListPointAtGoodDistance(characterPoint, elementPoint, 1); //m_Account.Inventory.WeaponRange);
                    var selectedPoint = goodPointsList.FirstOrDefault((point) => point.CellId == characterPoint.CellId);
                    if (selectedPoint != null)
                        return true;
                }
            }
            return false;
        }

        public bool ChangeMap(string direction)
        {
            int neighbourId = -1;
            int num2 = -1;
            switch(direction)
            {
                case "up":
                neighbourId = ((BlueSheep.Data.D2p.Map)Data).TopNeighbourId;
                num2 = 64;
                    break;
                case "bottom":
                neighbourId = ((BlueSheep.Data.D2p.Map)Data).BottomNeighbourId;
                num2 = 4;
                    break;
                case "right":
                neighbourId = ((BlueSheep.Data.D2p.Map)Data).RightNeighbourId;
                num2 = 1;
                    break;
                case "left":
                neighbourId = ((BlueSheep.Data.D2p.Map)Data).LeftNeighbourId;
                num2 = 16;
                break;
            }

            if ((num2 != -1) && (neighbourId >= 0))
            {
                int cellId = Character.CellId;
                if ((((BlueSheep.Data.D2p.Map)Data).Cells[cellId].MapChangeData & num2) > 0)
                {
                    LastMapId = this.Id;
                    LaunchChangeMap(neighbourId);
                    return true;
                }
                List<int> list = new List<int>();
                int num4 = (((BlueSheep.Data.D2p.Map)Data).Cells.Count - 1);
                int i = 0;
                while (i <= num4)
                {
                    if (((((BlueSheep.Data.D2p.Map)Data).Cells[i].MapChangeData & num2) > 0) && NothingOnCell(i))
                        list.Add(i);
                    i += 1;
                }
                while (list.Count > 0)
                {
                    int randomCellId = list[RandomCell(0, list.Count)];
                    if (MoveToCell(randomCellId))
                    {
                        MapChange = neighbourId;
                        LastMapId = this.Id;
                        LaunchChangeMap(neighbourId);
                        return true;
                    }
                    list.Remove(randomCellId);
                }
            }
            return false;
        }

        public bool MoveToCell(int cellId)
        {
            if (m_Account.StatusLb.Text == "Combat" || m_Account.StatusLb.Text == "Fighting")
                return false;
            m_Account.ModifBar(6, 0, 0, "Déplacement");
            MovementPath path = (new Pathfinder(m_Account.Map.Data, m_Account.Map)).FindPath(Character.CellId, cellId);
            if (path == null)
                return false;
            List<UInt32> serverMovement = MapMovementAdapter.GetServerMovement(path);
            int timetowait;
            if (serverMovement.Count() < 3)
                timetowait = serverMovement.Count() * 500;
            else
            {
                timetowait = serverMovement.Count() * 300;
            }
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                GameMapMovementRequestMessage msg = new GameMapMovementRequestMessage(serverMovement.Select<uint, short>(ui => (short)ui).ToArray(), Id);
                msg.Serialize(writer);
                writer.Content = m_Account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                m_Account.SocketManager.Send(pack.Writer.Content);
            }
            m_Account.Wait(timetowait, timetowait + 100);
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                GameMapMovementConfirmMessage newmsg = new GameMapMovementConfirmMessage();
                newmsg.Serialize(writer);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)newmsg.ProtocolID);
                if (m_Account.Fight != null && m_Account.Fight.IsFollowingGroup())
                {
                    m_Account.SocketManager.Send(pack.Writer.Content);
                    m_Account.Fight.LaunchFight(m_Account.Fight.followinggroup.m_contextualId);
                    //m_Account.Wait(3000, 4000);
                    //if (m_Account.StatusLb.Text != "Combat")
                    //{
                    //    m_Account.Fight.SearchFight();
                    //}
                }
                else
                {
                    m_Account.SocketManager.Send(pack.Writer.Content);
                }
                if (m_Account.Gather != null)
                    m_Account.Gather.Moved = true;
                m_Account.ModifBar(6, 0, 0, "Connecté");
            }
            
            return true;
        }

        public bool MoveToDoor(int cellId)
        {
            return MoveToCellWithDistance(cellId, 1, true);
        }

        public bool MoveToElement(int id, int maxDistance)
        {
            Elements.StatedElement element = null;
            if (StatedElements.ContainsKey(id))
            {
                element = StatedElements[id];
            }
            if (element != null)
                return MoveToCellWithDistance((int)element.CellId, maxDistance, false);
            else
                return false;
        }

        public bool MoveToSecureElement(int id)
        {
            Elements.StatedElement element = null;
            if (StatedElements.ContainsKey(id))
            {
                element = StatedElements[id];
            }
            if (element != null)
                return MoveToCellWithDistance((int)element.CellId, 1, true);
            else
            {
                //m_Account.Log(new ErrorTextInformation("Element introuvable"), 0);
                return false;
            }
        }

        public bool NoEntitiesOnCell(int cellId)
        {
                Entity selectedEntity = Entities.FirstOrDefault(e => e.CellId == cellId);
                if (selectedEntity != null)
                    return false;
                else
                    return true;
        }

        public bool NothingOnCell(int cellId)
        {
            if (Data.IsWalkable(cellId))
                return NoEntitiesOnCell(cellId);
            return false;
        }

        public void UseElement(int id, int skillId)
        {
            //if (AutoTimeout)
            //    m_Account.Game.Character.State.SetTimeout(StateEnum.Gathering, false, TimeoutMin, TimeoutMax);
            //m_Account.Network.SendToServer(new InteractiveUseRequestMessage(id, skillId));
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                InteractiveUseRequestMessage msg = new InteractiveUseRequestMessage(id, skillId);
                msg.Serialize(writer);
                writer.Content = m_Account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                m_Account.SocketManager.Send(pack.Writer.Content);
            }
        }

        public void UseElement(int id)
        {
            InteractiveElement e = InteractiveElements[id];
            UseElement(id, e.EnabledSkills[0].skillInstanceUid);
        }

        public bool MoveToCellWithDistance(int cellId, int maxDistance, bool bool1)
        {
            m_Account.ModifBar(6, 0, 0, "Déplacement");
            MovementPath path = null;
            int savDistance = -1;
            MapPoint characterPoint = new MapPoint(Character.CellId);
            MapPoint targetPoint = new MapPoint(cellId);
            foreach (MapPoint point in GetListPointAtGoodDistance(characterPoint, targetPoint, maxDistance))
            {
                Pathfinder pathFinding = null;
                if ((targetPoint.DistanceToCell(point) > maxDistance) || ((targetPoint.X != point.X) && (targetPoint.Y != point.Y)))
                    continue;
                int distance = characterPoint.DistanceTo(point);
                if ((savDistance != -1) && (distance >= savDistance))
                    continue;
                if (bool1)
                {
                    if (Data.IsWalkable(point.CellId))
                        goto Label_00A8;
                    continue;
                }
                if (!(NothingOnCell(point.CellId)))
                    continue;
            Label_00A8:
                pathFinding = new Pathfinder(m_Account.Map.Data, m_Account.Map);
                MovementPath path2 = pathFinding.FindPath(Character.CellId, point.CellId);
                if (path2 != null)
                {
                    path = path2;
                    savDistance = distance;
                }
            }
            if (path == null)
                return false;
            //if (AutoTimeout)
            //    m_Account.Game.Character.State.SetTimeout(StateEnum.Moving, false, TimeoutMin, TimeoutMax);
            List<UInt32> serverMovement = MapMovementAdapter.GetServerMovement(path);
            int timetowait;
            if (serverMovement.Count() < 3)
                timetowait = serverMovement.Count() * 500;
            else
            {
                timetowait = serverMovement.Count() * 300;
            }
            //m_Account.Network.SendToServer(new GameMapMovementRequestMessage(serverMovement.Select<uint, short>(ui => (short)ui).ToArray(), Id));
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                GameMapMovementRequestMessage msg = new GameMapMovementRequestMessage(serverMovement.Select<uint, short>(ui => (short)ui).ToArray(), Id);
                msg.Serialize(writer);
                writer.Content = m_Account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                m_Account.SocketManager.Send(pack.Writer.Content);
            }
            m_Account.Wait(timetowait, timetowait + 100);
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                GameMapMovementConfirmMessage newmsg = new GameMapMovementConfirmMessage();
                newmsg.Serialize(writer);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)newmsg.ProtocolID);
                if (m_Account.Fight != null && m_Account.Fight.IsFollowingGroup())
                {
                    m_Account.SocketManager.Send(pack.Writer.Content);
                    m_Account.Fight.LaunchFight(m_Account.Fight.followinggroup.m_contextualId);
                    //m_Account.Wait(3000, 4000);
                    //if (m_Account.StatusLb.Text != "Combat")
                    //{
                    //    m_Account.Fight.SearchFight();
                    //}
                }
                else if (m_Account.Gather.Id != -1)
                {
                    m_Account.SocketManager.Send(pack.Writer.Content);
                    UseElement(m_Account.Gather.Id, m_Account.Gather.SkillInstanceUid);
                }
                else
                {
                    m_Account.SocketManager.Send(pack.Writer.Content);
                }
                m_Account.ModifBar(6, 0, 0, "Connecté");
            }
            return true;
        }

        public void useZaapi(int mapid)
        {
            foreach (InteractiveElement e in InteractiveElements.Values)
            {
                if (e.TypeId == 106)
                {
                    MoveToSecureElement((int)e.Id);
                    UseElement((int)e.Id,e.EnabledSkills[0].skillInstanceUid);
                    m_Account.Wait(500, 1000);
                    using (BigEndianWriter writer = new BigEndianWriter())
                    {
                        TeleportRequestMessage msg = new TeleportRequestMessage(1,mapid);
                        msg.Serialize(writer);
                        MessagePackaging pack = new MessagePackaging(writer);
                        pack.Pack((int)msg.ProtocolID);
                        m_Account.SocketManager.Send(pack.Writer.Content);
                    }
                }
            }
        }

        public void useZaap(int mapid)
        {
            foreach (InteractiveElement e in InteractiveElements.Values)
            {
                if (e.TypeId == 16)
                {
                    MoveToSecureElement((int)e.Id);
                    UseElement((int)e.Id, e.EnabledSkills[0].skillInstanceUid);
                    m_Account.Wait(500, 1000);
                    TeleportRequestMessage msg = new TeleportRequestMessage(0, mapid);
                    m_Account.SocketManager.Send(msg);
                }
            }
        }

        public int GetCellFromContextId(int contextId)
        {
            foreach (Entity e in Entities)
            {
                if (e.Id == contextId)
                    return e.CellId;
            }
            foreach (StatedElement s in StatedElements.Values)
            {
                if (s.Id == contextId)
                    return (int)s.CellId;
            }
            foreach (InteractiveElement d in Doors.Values)
            {
                if (d.Id == contextId)
                    return Doors.Keys.ToList()[Doors.Values.ToList().IndexOf(d)];
            }
            return 0;
        }
        #endregion

        #region Private methods
        private List<MapPoint> GetListPointAtGoodDistance(MapPoint characterPoint, MapPoint elementPoint, int weaponRange)
        {
            List<MapPoint> list = new List<MapPoint>();
            int num = -1;
            int direction = 1;
            while (true)
            {
                int i = 0;
                while (i < weaponRange)
                {
                    i += 1;
                    MapPoint nearestCellInDirection = elementPoint.GetNearestCellInDirection(direction, i);
                    if (nearestCellInDirection.IsInMap() && m_Account.Map.Data.IsWalkable(nearestCellInDirection.CellId))
                    {
                        int num4 = characterPoint.DistanceToCell(nearestCellInDirection);
                        if ((num == -1) || (num >= num4))
                        {
                            if (num4 < num)
                                list.Clear();
                            num = num4;
                            list.Add(nearestCellInDirection);
                        }
                        break;
                    }
                }
                direction = (direction + 2);
                if (direction > 7)
                    return list;
            }
        }
        private int RandomCell(int min, int max)
        {
            Random random = new Random();
            if (min > max)
            {
                int num = max;
                max = min;
                min = num;
            }
            return random.Next(min, max);
        }

        public void LaunchChangeMap(int mapId)
        {
            //if (AutoTimeout)
            //    m_Account.Game.Character.State.SetTimeout(StateEnum.Context, false, TimeoutMin, TimeoutMax);
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                ChangeMapMessage msg = new ChangeMapMessage(mapId);
                msg.Serialize(writer);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                m_Account.SocketManager.Send(pack.Writer.Content);
            }
        }
        #endregion
    }
}
