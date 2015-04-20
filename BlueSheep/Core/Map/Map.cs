using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Core.Fight;
using BlueSheep.Core.Map.Elements;
using BlueSheep.Data.D2p.Elements;
using BlueSheep.Data.Pathfinding;
using BlueSheep.Data.Pathfinding.Positions;
using BlueSheep.Engine.Enums;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlueSheep.Core.Map
{
    public class Map
    {
        #region Fields
        private AccountUC m_Account;
        public bool Moving;
        private int m_time;
        private int m_MapId;
        private int m_elemId = -1;
        #endregion

        #region Constructeurs
        public Map(AccountUC account)
        {
            m_Account = account;
        }
        #endregion

        #region Public methods
        public bool ChangeMap(string direction)
        {
            int neighbourId = -1;
            int num2 = -1;
            switch(direction)
            {
                case "haut":
                case "up":
                    neighbourId = m_Account.MapData.Data.TopNeighbourId;
                num2 = 64;
                    break;
                case "bas":
                case "bottom":
                    neighbourId = m_Account.MapData.Data.BottomNeighbourId;
                num2 = 4;
                    break;
                case "droite":
                case "right":
                    neighbourId = m_Account.MapData.Data.RightNeighbourId;
                num2 = 1;
                    break;
                case "gauche":
                case "left":
                    neighbourId = m_Account.MapData.Data.LeftNeighbourId;
                num2 = 16;
                break;
            }

            if ((num2 != -1) && (neighbourId >= 0))
            {
                int cellId = m_Account.MapData.Character.disposition.cellId;
                if ((m_Account.MapData.Data.Cells[cellId].MapChangeData & num2) > 0)
                {
                    LaunchChangeMap(neighbourId);
                    return true;
                }
                List<int> list = new List<int>();
                int num4 = (m_Account.MapData.Data.Cells.Count - 1);
                int i = 0;
                while (i <= num4)
                {
                    if (((m_Account.MapData.Data.Cells[i].MapChangeData & num2) > 0) && m_Account.MapData.NothingOnCell(i))
                        list.Add(i);
                    i += 1;
                }
                while (list.Count > 0)
                {
                    int randomCellId = list[RandomCell(0, list.Count)];
                    m_MapId = neighbourId;
                    if (MoveToCell(randomCellId))
                    {
                        return true;
                    }
                    list.Remove(randomCellId);
                }
            }
            return false;
        }

        public bool MoveToCell(int cellId)
        {
            if (m_Account.state == Engine.Enums.Status.Fighting)
                return false;
            m_Account.SetStatus(Status.Moving);
            MovementPath path = (new Pathfinder(m_Account.MapData)).FindPath(m_Account.MapData.Character.disposition.cellId, cellId);
            if (path == null)
                return false;
            List<UInt32> serverMovement = MapMovementAdapter.GetServerMovement(path);
            if(serverMovement[serverMovement.Count -1] == m_Account.MapData.Character.disposition.cellId)
            {
                Moving = false;
                ConfirmMove();
                return true;
            }
            int timetowait;
            if (serverMovement.Count() < 3)
                timetowait = serverMovement.Count() * 500;
            else
            {
                timetowait = serverMovement.Count() * 300;
            }
            m_time = timetowait;
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                GameMapMovementRequestMessage msg = new GameMapMovementRequestMessage(serverMovement.Select<uint, short>(ui => (short)ui).ToArray(), m_Account.MapData.Id);
                msg.Serialize(writer);
                writer.Content = m_Account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                m_Account.SocketManager.Send(pack.Writer.Content);
                Moving = true;
                if (m_Account.DebugMode.Checked)
                    m_Account.Log(new DebugTextInformation("[SND] 950 (GameMapMovementRequestMessage)"), 0);
            }
            //m_Account.Wait(timetowait, timetowait + 100);
            //using (BigEndianWriter writer = new BigEndianWriter())
            //{
            //    GameMapMovementConfirmMessage newmsg = new GameMapMovementConfirmMessage();
            //    newmsg.Serialize(writer);
            //    MessagePackaging pack = new MessagePackaging(writer);
            //    pack.Pack((int)newmsg.ProtocolID);
            //    if (m_Account.Fight != null && m_Account.FightData.IsFollowingGroup && m_Account.FightData.followingGroup.m_cellId == cellId)
            //    {
            //        m_Account.SocketManager.Send(pack.Writer.Content);
            //        if (m_Account.DebugMode.Checked)
            //            m_Account.Log(new DebugTextInformation("[SND] 952 (GameMapMovementConfirmMessage)"), 0);
            //        m_Account.Fight.LaunchFight(m_Account.FightData.followingGroup.m_contextualId);
            //        Thread t = new Thread(new ThreadStart(CheckFight));
            //        t.Start();
            //    }
            //    else if (m_Account.Fight != null && m_Account.FightData.IsFollowingGroup)
            //    {
            //        m_Account.Fight.SearchFight();
            //    }
            //    else if (m_Account.Gather.Id != -1)
            //    {
            //        m_Account.SocketManager.Send(pack.Writer.Content);
            //        if (m_Account.DebugMode.Checked)
            //            m_Account.Log(new DebugTextInformation("[SND] 952 (GameMapMovementConfirmMessage)"), 0);
            //        //UseElement(m_Account.Gather.Id, m_Account.Gather.SkillInstanceUid);
            //        int distance = m_Account.Gather.GetRessourceDistance(m_Account.Gather.Id);
            //        m_Account.Log(new DebugTextInformation("[Gather] New distance from element " + m_Account.Gather.Id + " = " + distance), 0);
            //        if (distance <= m_Account.Inventory.WeaponRange)
            //        {
            //            UseElement(m_Account.Gather.Id, m_Account.Gather.SkillInstanceUid);
            //            m_Account.SetStatus(Status.Gathering);
            //        }
            //        else if (m_Account.Path != null)
            //            m_Account.Path.PerformFlag();
            //        else
            //            m_Account.PerformGather();
            //    }
            //    else
            //    {
            //        m_Account.SocketManager.Send(pack.Writer.Content);
            //        if (m_Account.DebugMode.Checked)
            //            m_Account.Log(new DebugTextInformation("[SND] 952 (GameMapMovementConfirmMessage)"), 0);
            //    }
            //    m_Account.SetStatus(Status.None);
            //}
            
            return true;
        }

        public bool MoveToDoor(int cellId)
        {
            return MoveToCellWithDistance(cellId, 1, true);
        }

        public bool MoveToElement(int id, int maxDistance)
        {
            Elements.StatedElement element = m_Account.MapData.StatedElements.Find(s => s.Id == id);
            if (element != null)
                return MoveToCellWithDistance((int)element.CellId, maxDistance, false);
            else
                return false;
        }

        public bool MoveToSecureElement(int id)
        {
            Elements.StatedElement element = m_Account.MapData.StatedElements.Find(s => s.Id == id);
            m_elemId = id;
            if (element != null)
                return MoveToCellWithDistance((int)element.CellId, 1, true);
            else
            {
                return false;
            }
        }

        public void LaunchChangeMap(int mapId)
        {
            if (m_Account.Path != null)
                m_Account.Path.ClearStack();
            m_MapId = -1;
            ChangeMapMessage msg = new ChangeMapMessage(mapId);
            m_Account.SetStatus(Status.Busy);
            m_Account.SocketManager.Send(msg);
            Thread t = new Thread(new ThreadStart(CheckMapChange));
            t.Start();
        }

        public void UseElement(int id, int skillId)
        {
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                InteractiveUseRequestMessage msg = new InteractiveUseRequestMessage(id, skillId);
                msg.Serialize(writer);
                writer.Content = m_Account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                m_Account.SocketManager.Send(pack.Writer.Content);
            }
            if (m_Account.DebugMode.Checked)
                m_Account.Log(new DebugTextInformation("[SND] 5001 (InteractiveUseRequestMessage)"), 0);
        }

        public void UseElement(int id)
        {
            InteractiveElement e = m_Account.MapData.InteractiveElements.Keys.ToList().Find(i => i.Id == id);
            UseElement(id, e.EnabledSkills[0].skillInstanceUid);
            m_elemId = -1;
        }

        public bool MoveToCellWithDistance(int cellId, int maxDistance, bool bool1)
        {
            MovementPath path = null;
            int savDistance = -1;
            MapPoint characterPoint = new MapPoint(m_Account.MapData.Character.disposition.cellId);
            MapPoint targetPoint = new MapPoint(cellId);
            foreach (MapPoint point in m_Account.MapData.GetListPointAtGoodDistance(characterPoint, targetPoint, maxDistance))
            {
                Pathfinder pathFinding = null;
                if ((targetPoint.DistanceToCell(point) > maxDistance) || ((targetPoint.X != point.X) && (targetPoint.Y != point.Y)))
                    continue;
                int distance = characterPoint.DistanceTo(point);
                if ((savDistance != -1) && (distance >= savDistance))
                    continue;
                if (bool1)
                {
                    if (m_Account.MapData.Data.IsWalkable(point.CellId))
                        goto Label_00A8;
                    continue;
                }
                if (!(m_Account.MapData.NothingOnCell(point.CellId)))
                    continue;
            Label_00A8:
                pathFinding = new Pathfinder(m_Account.MapData);
            MovementPath path2 = pathFinding.FindPath(m_Account.MapData.Character.disposition.cellId, point.CellId);
                if (path2 != null)
                {
                    path = path2;
                    savDistance = distance;
                }
            }
            if (path == null)
                return false;
            List<UInt32> serverMovement = MapMovementAdapter.GetServerMovement(path);
            if (serverMovement[serverMovement.Count - 1] == m_Account.MapData.Character.disposition.cellId)
            {
                Moving = false;
                ConfirmMove();
                return true;
            }
            int timetowait;
            if (serverMovement.Count() < 3)
                timetowait = serverMovement.Count() * 514;
            else
            {
                timetowait = serverMovement.Count() * 320;
            }
            m_time = timetowait;
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                GameMapMovementRequestMessage msg = new GameMapMovementRequestMessage(serverMovement.Select<uint, short>(ui => (short)ui).ToArray(), m_Account.MapData.Id);
                msg.Serialize(writer);
                writer.Content = m_Account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                m_Account.SocketManager.Send(pack.Writer.Content);
                m_Account.SetStatus(Status.Moving);
                Moving = true;
                if (m_Account.DebugMode.Checked)
                    m_Account.Log(new DebugTextInformation("[SND] 950 (GameMapMovementRequestMessage)"), 0);
            }
            return true;
        }

        public void ConfirmMove()
        {
            m_Account.Wait(m_time, m_time + 100);
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                if (Moving)
                {
                    m_Account.SocketManager.Send(new GameMapMovementConfirmMessage());
                    Moving = false;
                }
                m_Account.SetStatus(Status.None);
                
                if (m_MapId != -1)
                {
                    LaunchChangeMap(m_MapId);
                    return;
                }
                else if (m_elemId != -1 && m_Account.Gather.Id == -1)
                {
                    UseElement(m_elemId);
                }
                if (m_Account.Fight != null && m_Account.FightData.IsFollowingGroup && m_Account.FightData.followingGroup.m_cellId == m_Account.MapData.Character.disposition.cellId)
                {
                    m_Account.Fight.LaunchFight(m_Account.FightData.followingGroup.m_contextualId);
                }
                else if (m_Account.Fight != null && m_Account.FightData.IsFollowingGroup)
                {
                    m_Account.Fight.SearchFight();
                }
                else if (m_Account.Gather.Id != -1)
                {
                    //if (m_Account.MapData.CanGatherElement(m_Account.Gather.Id, m_Account.Inventory.WeaponRange))
                    int distance = m_Account.Gather.GetRessourceDistance(m_Account.Gather.Id);
                    m_Account.Log(new DebugTextInformation("[Gather] New distance from element " + m_Account.Gather.Id + " = " + distance), 0);
                    m_Account.Log(new DebugTextInformation("[CanGatherElement] " + m_Account.MapData.CanGatherElement(m_Account.Gather.Id, m_Account.Inventory.WeaponRange).ToString()), 0);
                    //if (distance <= m_Account.Inventory.WeaponRange)
                    if (m_Account.MapData.CanGatherElement(m_Account.Gather.Id, m_Account.Inventory.WeaponRange))
                    {
                        m_Account.SetStatus(Status.Gathering);
                        UseElement(m_Account.Gather.Id, m_Account.Gather.SkillInstanceUid);
                    }
                    else if (m_Account.Path != null)
                    {
                        m_Account.Gather.BanElementId(m_Account.Gather.Id);
                        m_Account.Path.PerformFlag();
                    }
                    else
                    {
                        m_Account.SetStatus(Status.None);
                        m_Account.Gather.BanElementId(m_Account.Gather.Id);
                        m_Account.PerformGather();
                    }
                }
                else
                {
                    m_Account.SetStatus(Status.None);
                }
            }
        }

        public void useZaapi(int mapid)
        {

            InteractiveElement e = m_Account.MapData.InteractiveElements.Keys.ToList().Find(i => i.TypeId == 106);
            if (e != null)
            {
                MoveToSecureElement((int)e.Id);
                UseElement((int)e.Id, e.EnabledSkills[0].skillInstanceUid);
                m_Account.Wait(500, 1000);
                TeleportRequestMessage msg = new TeleportRequestMessage(1, mapid);
                m_Account.SocketManager.Send(msg);
            }

        }

        public void useZaap(int mapid)
        {
            InteractiveElement e = m_Account.MapData.InteractiveElements.Keys.ToList().Find(i => i.TypeId == 16);
            if (e != null)
            {
                MoveToSecureElement((int)e.Id);
                UseElement((int)e.Id, e.EnabledSkills[0].skillInstanceUid);
                m_Account.Wait(500, 1000);
                TeleportRequestMessage msg = new TeleportRequestMessage(0, mapid);
                m_Account.SocketManager.Send(msg);
            }

        }

        public void CheckFight()
        {
            double endwait = Environment.TickCount + 4000;
            while (Environment.TickCount < endwait)
            {
                System.Threading.Thread.Sleep(1);
                System.Windows.Forms.Application.DoEvents();
            }
            if (m_Account.state != Status.Fighting && m_Account.Fight != null)
            {
                m_Account.Fight.Error();
                m_Account.Log(new DebugTextInformation("[CheckFight] Try to research another fight"), 0);
                if (m_Account.Fight.SearchFight() == false && m_Account.Path != null)
                    m_Account.Path.PerformFlag();

            }
        }
        #endregion

        #region Private methods      
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

        

        private void CheckMapChange()
        {
            int old = m_Account.MapData.Id;
            m_Account.Log(new DebugTextInformation("[Map] Old = " + old), 0);
            System.Threading.Thread.Sleep(4000);
            m_Account.Log(new DebugTextInformation("[Map] New = " + m_Account.MapData.Id), 0);
            if (old == m_Account.MapData.Id && m_Account.Path != null)
            {
                m_Account.Path.PerformFlag();
            }
        }
        #endregion
    }
}
