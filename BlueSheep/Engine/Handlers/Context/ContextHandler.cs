using BlueSheep.Common.Data;
using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.Types;
using BlueSheep.Data.D2p;
using BlueSheep.Data.D2p.Elements;
using BlueSheep.Data.Pathfinding;
using BlueSheep.Data.Pathfinding.Positions;
using BlueSheep.Engine.Enums;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using BlueSheep.Interface.Text.Chat;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlueSheep.Engine.Handlers.Context
{
    class ContextHandler
    {
        #region Public methods
        [MessageHandler(typeof(MapComplementaryInformationsDataInHouseMessage))]
        public static void MapComplementaryInformationsDataInHouseMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            MapComplementaryInformationsDataInHouseMessage msg = (MapComplementaryInformationsDataInHouseMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.Map.Data != null)
                account.Map.LastMapId = account.Map.Id;
            foreach (InteractiveElement interactiveElement in msg.interactiveElements)
            {
                if (interactiveElement.elementTypeId == 85)
                    account.Safe = interactiveElement;
            }
            account.Map.SubAreaId = msg.subAreaId;
            account.Map.Data = MapsManager.FromId(msg.mapId);
            DataClass subArea = GameData.GetDataObject(D2oFileEnum.SubAreas, (int)msg.subAreaId);
            string mapName = I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Areas, (int)subArea.Fields["areaId"]).Fields["nameId"]);
            string subAreaName = I18N.GetText((int)subArea.Fields["nameId"]);
            account.ModifBar(5, 0, 0, "[" + account.Map.X + ";" + account.Map.Y + "]" + " " + mapName + " (" + subAreaName + ")");
            account.Map.Entities.Clear();
            account.Map.List.Clear();
            account.Map.Players = new Dictionary<int, Common.Protocol.Types.GameRolePlayCharacterInformations>();
            foreach (GameRolePlayActorInformations actor in msg.actors)
            {
                account.Map.Entities.Add(new BlueSheep.Core.Fight.Entity(actor.contextualId, actor.disposition.cellId));
                if (actor is GameRolePlayGroupMonsterInformations)
                {
                    GameRolePlayGroupMonsterInformations a = (GameRolePlayGroupMonsterInformations)actor;
                    account.Map.List.Add(new MonsterGroup(a.staticInfos, a.disposition.cellId, a.contextualId));
                }
                else if (actor is GameRolePlayCharacterInformations)
                {
                    GameRolePlayCharacterInformations a = (GameRolePlayCharacterInformations)actor;
                    account.Map.Players.Add(a.contextualId, a);
                }

            }
            account.Map.StatedElements.Clear();
            foreach (var statedElementDofus in msg.statedElements)
            {
                if (!(account.Map.StatedElements.ContainsKey(statedElementDofus.elementId)))
                    account.Map.StatedElements.Add(statedElementDofus.elementId, new BlueSheep.Core.Map.Elements.StatedElement((uint)statedElementDofus.elementCellId, (uint)statedElementDofus.elementId, (uint)statedElementDofus.elementState));
            }
            account.Map.InteractiveElements.Clear();
            account.Map.Doors.Clear();
            account.Enable(true);
            foreach (var element in msg.interactiveElements)
            {
                account.Map.InteractiveElements.Add(element.elementId, new BlueSheep.Core.Map.Elements.InteractiveElement((uint)element.elementId, element.elementTypeId, new List<InteractiveElementSkill>(element.enabledSkills), new List<InteractiveElementSkill>(element.disabledSkills)));
                InteractiveElement interactiveElement = element;
                List<int> listDoorSkillId = new List<int>(new[] { 184, 183, 187, 198, 114 });
                List<int> listDoorTypeId = new List<int>(new[] { -1, 128, 168, 16 });
                if (listDoorTypeId.Contains(interactiveElement.elementTypeId) && (interactiveElement.enabledSkills.Length > 0) && (listDoorSkillId.Contains(interactiveElement.enabledSkills[0].skillId)))
                {
                    foreach (var layer in ((BlueSheep.Data.D2p.Map)account.Map.Data).Layers)
                    {
                        foreach (var cell in layer.Cells)
                        {
                            foreach (var layerElement in cell.Elements)
                            {
                                if (layerElement is GraphicalElement)
                                {
                                    GraphicalElement graphicalElement = (GraphicalElement)layerElement;
                                    if ((graphicalElement.Identifier == interactiveElement.elementId) && !(account.Map.Doors.ContainsKey(cell.CellId)))
                                        account.Map.Doors.Add(cell.CellId, new BlueSheep.Core.Map.Elements.InteractiveElement((uint)element.elementId, element.elementTypeId, new List<InteractiveElementSkill>(element.enabledSkills), new List<InteractiveElementSkill>(element.disabledSkills)));
                                }
                            }
                        }
                    }
                }

            }
            account.Npc.Npcs.Clear();
            foreach (GameRolePlayActorInformations a in msg.actors)
            {
                if (a is GameRolePlayNpcInformations)
                    account.Npc.Npcs.Add(a.contextualId, ((GameRolePlayNpcInformations)a).npcId);
            }
            if (account.Path != null)
            {
                if (account.Path.Current_Flag == "<Fight>" && account.state != Enums.Status.Fighting && account.Path.Current_Map == account.Map.X.ToString() + "," + account.Map.Y.ToString())
                {
                    if (account.Fight.SearchFight() == false)
                    {
                        account.Path.PerformActionsStack();
                    }
                }
                else if (account.Path != null & account.state != Enums.Status.Fighting && account.Path.Current_Map == account.Map.X.ToString() + "," + account.Map.Y.ToString())
                    account.Path.PerformActionsStack();
                else if (account.Path != null & account.Path.Current_Map != account.Map.X.ToString() + "," + account.Map.Y.ToString() || account.Map.Id != account.Map.LastMapId)
                {
                    account.Path.Stop = false;
                    account.Path.ParsePath();
                }
            }
            if (account.petsList.Count != 0 && account.checkBoxBegin.Checked == true)
            {
                account.StartFeeding();
            }
            else if (account.checkBoxBegin.Checked == true)
            {
                account.Log(new ErrorTextInformation("Aucun familier dans l'inventaire."),0);
                account.checkBoxBegin.Checked = false;
            }
            account.ActualizeMap();
        }

        [MessageHandler(typeof(MapComplementaryInformationsDataMessage))]
        public static void MapComplementaryInformationsDataMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            MapComplementaryInformationsDataMessage msg = (MapComplementaryInformationsDataMessage)message;
            account.HeroicUC.AnalysePacket(message, packetDatas);
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.petsList.Count != 0 && account.checkBoxBegin.Checked == true)
            {
                account.StartFeeding();
            }
            else if (account.checkBoxBegin.Checked == true)
            {
                account.Log(new ErrorTextInformation("Aucun familier dans l'inventaire."),0);
                account.checkBoxBegin.Checked = false;
            }
            if (account.Map.Data != null)
                account.Map.LastMapId = account.Map.Id;
            account.Map.SubAreaId = msg.subAreaId;
            account.Map.Data = MapsManager.FromId(msg.mapId);
            account.Enable(true);
            DataClass subArea = GameData.GetDataObject(D2oFileEnum.SubAreas, (int)msg.subAreaId);
            string mapName = I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Areas, (int)subArea.Fields["areaId"]).Fields["nameId"]);
            string subAreaName = I18N.GetText((int)subArea.Fields["nameId"]);
            account.ModifBar(5, 0, 0, "[" + account.Map.X + ";" + account.Map.Y + "]" + " " + mapName + " (" + subAreaName + ")");
            account.Map.Entities.Clear();
            account.Map.List.Clear();
            account.Map.Players = new Dictionary<int, Common.Protocol.Types.GameRolePlayCharacterInformations>();
            foreach (GameRolePlayActorInformations actor in msg.actors)
            {
                account.Map.Entities.Add(new BlueSheep.Core.Fight.Entity(actor.contextualId, actor.disposition.cellId));
                if (actor is GameRolePlayGroupMonsterInformations)
                {
                    GameRolePlayGroupMonsterInformations a = (GameRolePlayGroupMonsterInformations)actor;
                    account.Map.List.Add(new MonsterGroup(a.staticInfos, a.disposition.cellId, a.contextualId));
                }
                else if (actor is GameRolePlayCharacterInformations)
                {
                    GameRolePlayCharacterInformations a = (GameRolePlayCharacterInformations)actor;
                    account.Map.Players.Add(a.contextualId, a);
                }

            }
            account.Map.StatedElements.Clear();
            foreach (var statedElementDofus in msg.statedElements)
            {
                if (!(account.Map.StatedElements.ContainsKey(statedElementDofus.elementId)))
                    account.Map.StatedElements.Add(statedElementDofus.elementId, new BlueSheep.Core.Map.Elements.StatedElement((uint)statedElementDofus.elementCellId, (uint)statedElementDofus.elementId, (uint)statedElementDofus.elementState));
            }
            account.Map.InteractiveElements.Clear();
            account.Map.Doors.Clear();
            foreach (var element in msg.interactiveElements)
            {
                account.Map.InteractiveElements.Add(element.elementId, new BlueSheep.Core.Map.Elements.InteractiveElement((uint)element.elementId, element.elementTypeId, new List<InteractiveElementSkill>(element.enabledSkills), new List<InteractiveElementSkill>(element.disabledSkills)));
                InteractiveElement interactiveElement = element;
                List<int> listDoorSkillId = new List<int>(new[] { 184, 183, 187, 198, 114 });
                List<int> listDoorTypeId = new List<int>(new[] { -1, 128, 168, 16 });
                if (listDoorTypeId.Contains(interactiveElement.elementTypeId) && (interactiveElement.enabledSkills.Length > 0) && (listDoorSkillId.Contains(interactiveElement.enabledSkills[0].skillId)))
                {
                    foreach (var layer in ((BlueSheep.Data.D2p.Map)account.Map.Data).Layers)
                    {
                        foreach (var cell in layer.Cells)
                        {
                            foreach (var layerElement in cell.Elements)
                            {
                                if (layerElement is GraphicalElement)
                                {
                                    GraphicalElement graphicalElement = (GraphicalElement)layerElement;
                                    if ((graphicalElement.Identifier == interactiveElement.elementId) && !(account.Map.Doors.ContainsKey(cell.CellId)))
                                        account.Map.Doors.Add(cell.CellId, new BlueSheep.Core.Map.Elements.InteractiveElement((uint)element.elementId, element.elementTypeId, new List<InteractiveElementSkill>(element.enabledSkills), new List<InteractiveElementSkill>(element.disabledSkills)));
                                }
                            }
                        }
                    }
                }
            }
            account.Npc.Npcs.Clear();
            foreach (GameRolePlayActorInformations a in msg.actors)
            {
                if (a is GameRolePlayNpcInformations)
                    account.Npc.Npcs.Add(a.contextualId, ((GameRolePlayNpcInformations)a).npcId);
            }
            if (account.Path != null)
            {
                if (account.Path.Current_Flag == "<Fight>" && account.state != Enums.Status.Fighting && account.Path.Current_Map == account.Map.X.ToString() + "," + account.Map.Y.ToString())
                {
                    if (account.Fight.SearchFight() == false)
                    {
                        account.Path.PerformActionsStack();
                    }
                }
                else if (account.state != Enums.Status.Fighting && account.Path.Current_Map == account.Map.X.ToString() + "," + account.Map.Y.ToString() && account.Map.LastMapId == account.Map.Id)
                    account.Path.PerformActionsStack();
                else if ((account.Path.Current_Map != account.Map.X.ToString() + "," + account.Map.Y.ToString()) || account.Map.Id != account.Map.LastMapId)
                {
                    account.Path.Stop = false;
                    account.Path.ParsePath();
                }
            }
            account.ActualizeMap();
            account.Map.LastMapId = account.Map.Id;
        }

        [MessageHandler(typeof(MapComplementaryInformationsWithCoordsMessage))]
        public static void MapComplementaryInformationsWithCoordsMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            MapComplementaryInformationsWithCoordsMessage msg = (MapComplementaryInformationsWithCoordsMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Map.SubAreaId = msg.subAreaId;
            account.Map.Data = MapsManager.FromId(msg.mapId);
            DataClass subArea = GameData.GetDataObject(D2oFileEnum.SubAreas, (int)msg.subAreaId);
            string mapName = I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Areas, (int)subArea.Fields["areaId"]).Fields["nameId"]);
            string subAreaName = I18N.GetText((int)subArea.Fields["nameId"]);
            account.ModifBar(5, 0, 0, "[" + msg.worldX + ";" + msg.worldY + "]" + " " + mapName + " (" + subAreaName + ")");
            account.Map.Entities.Clear();
            account.Map.List.Clear();
            account.Enable(true);
            foreach (GameRolePlayActorInformations actor in msg.actors)
            {
                account.Map.Entities.Add(new BlueSheep.Core.Fight.Entity(actor.contextualId, actor.disposition.cellId));
                if (actor is GameRolePlayGroupMonsterInformations)
                {
                    GameRolePlayGroupMonsterInformations a = (GameRolePlayGroupMonsterInformations)actor;
                    account.Map.List.Add(new MonsterGroup(a.staticInfos, a.disposition.cellId, a.contextualId));
                }

            }
            account.Map.StatedElements.Clear();
            foreach (var statedElementDofus in msg.statedElements)
            {
                if (!(account.Map.StatedElements.ContainsKey(statedElementDofus.elementId)))
                    account.Map.StatedElements.Add(statedElementDofus.elementId, new BlueSheep.Core.Map.Elements.StatedElement((uint)statedElementDofus.elementCellId, (uint)statedElementDofus.elementId, (uint)statedElementDofus.elementState));
            }
            account.Map.InteractiveElements.Clear();
            account.Map.Doors.Clear();
            foreach (var element in msg.interactiveElements)
            {
                account.Map.InteractiveElements.Add(element.elementId, new BlueSheep.Core.Map.Elements.InteractiveElement((uint)element.elementId, element.elementTypeId, new List<InteractiveElementSkill>(element.enabledSkills), new List<InteractiveElementSkill>(element.disabledSkills)));
                InteractiveElement interactiveElement = element;
                List<int> listDoorSkillId = new List<int>(new[] { 184, 183, 187, 198, 114 });
                List<int> listDoorTypeId = new List<int>(new[] { -1, 128, 168, 16 });
                if (listDoorTypeId.Contains(interactiveElement.elementTypeId) && (interactiveElement.enabledSkills.Length > 0) && (listDoorSkillId.Contains(interactiveElement.enabledSkills[0].skillId)))
                {
                    foreach (var layer in ((BlueSheep.Data.D2p.Map)account.Map.Data).Layers)
                    {
                        foreach (var cell in layer.Cells)
                        {
                            foreach (var layerElement in cell.Elements)
                            {
                                if (layerElement is GraphicalElement)
                                {
                                    GraphicalElement graphicalElement = (GraphicalElement)layerElement;
                                    if ((graphicalElement.Identifier == interactiveElement.elementId) && !(account.Map.Doors.ContainsKey(cell.CellId)))
                                        account.Map.Doors.Add(cell.CellId, new BlueSheep.Core.Map.Elements.InteractiveElement((uint)element.elementId, element.elementTypeId, new List<InteractiveElementSkill>(element.enabledSkills), new List<InteractiveElementSkill>(element.disabledSkills)));
                                }
                            }
                        }
                    }
                }
            }
            account.Npc.Npcs.Clear();
            foreach (GameRolePlayActorInformations a in msg.actors)
            {
                if (a is GameRolePlayNpcInformations)
                    account.Npc.Npcs.Add(a.contextualId, ((GameRolePlayNpcInformations)a).npcId);
            }
            if (account.Path != null)
            {
                if (account.Path.Current_Flag == "<Fight>" && account.state != Enums.Status.Fighting && account.Path.Current_Map == account.Map.X.ToString() + "," + account.Map.Y.ToString())
                {
                    if (account.Fight.SearchFight() == false)
                    {
                        account.Path.PerformActionsStack();
                    }
                }
                else if (account.Path != null & account.state != Enums.Status.Fighting && account.Path.Current_Map == account.Map.X.ToString() + "," + account.Map.Y.ToString())
                    account.Path.PerformActionsStack();
                else if (account.Path != null & account.Path.Current_Map != account.Map.X.ToString() + "," + account.Map.Y.ToString() || account.Map.Id != account.Map.LastMapId)
                {
                    account.Path.Stop = false;
                    account.Path.ParsePath();
                }
            }
        }

        [MessageHandler(typeof(CurrentMapMessage))]
        public static void CurrentMapMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            CurrentMapMessage currentMapMessage = (CurrentMapMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                currentMapMessage.Deserialize(reader);
            }

            account.MapID = currentMapMessage.mapId;
            if (account.MapID == account.Map.LastMapId && account.Fight != null)
            {
                account.Fight.winLoseDic["Gagné"]++;
                account.ActualizeFightStats(account.Fight.winLoseDic, account.Fight.xpWon);
            }
            if (!account.IsMITM)
            {
                MapInformationsRequestMessage mapInformationsRequestMessage
                = new MapInformationsRequestMessage(account.MapID);
                account.SocketManager.Send(mapInformationsRequestMessage);
            }
            
            
        }

        [MessageHandler(typeof(GameContextCreateMessage))]
        public static void GameContextCreateMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            //QuestListRequestMessage questListRequestMessage = new QuestListRequestMessage();

            //using (BigEndianWriter writer = new BigEndianWriter())
            //{
            //    questListRequestMessage.Serialize(writer);

            //    MessagePackaging messagePackaging = new MessagePackaging(writer);

            //    messagePackaging.Pack((int)questListRequestMessage.ProtocolID);

            //    account.SocketManager.Send(messagePackaging.Writer.Content);
            //}
        }

        [MessageHandler(typeof(QuestListMessage))]
        public static void QuestListMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            if (!account.IsMITM)
            {
                MapInformationsRequestMessage mapInformationsRequestMessage
                    = new MapInformationsRequestMessage(account.MapID);

                account.SocketManager.Send(mapInformationsRequestMessage);
            }
            
        }

        [MessageHandler(typeof(TextInformationMessage))]
        public static void TextInformationTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            TextInformationMessage msg = (TextInformationMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            switch (msg.msgId)
            {
                case 89:
                    account.Log(new DofAlertTextInformation("Bienvenue sur DOFUS, dans le Monde des Douze !" + System.Environment.NewLine + "Il est interdit de transmettre votre identifiant ou votre mot de passe."),1);
                    break;
                case 153:
                    account.Log(new DofInfoCanal("Votre adresse ip actuelle est " + msg.parameters[0]),0);
                    break;
                case 171:
                    account.Log(new ErrorTextInformation(string.Format("Impossible de lancer ce sort, vous avez une portée de {0} à {1}, et vous visez à {2} !", msg.parameters[0], msg.parameters[1],msg.parameters[2])),4);
                    break;
                case 34:
                    account.Log(new ErrorTextInformation(string.Format("Vous avez perdu {0} points d'énergie", msg.parameters[0])),0);
                    account.Log(new ErrorTextInformation("Combat perdu"), 0);
                    if (account.Fight != null)
                    {
                        account.Fight.winLoseDic["Perdu"]++;
                        account.ActualizeFightStats(account.Fight.winLoseDic, account.Fight.xpWon);
                    }
                    break;
                //default:
                //    DataClass data = GameData.GetDataObjects(D2oFileEnum.InfoMessages)[msg.msgId];
                //    account.Log(new DofInfoCanal(I18N.GetText((int)data.Fields["textId"])));
                //    break;
            }
                        //string textId = I18N.GetText(msg.msgType * 10000 + msg.msgId);
                        //if (msg.parameters != null)
                        //{
                        //    if (msg.parameters[0] != null && msg.parameters[0].IndexOf("~") != -1)
                        //    {
                        //        params = msg.parameters[0].split("~");
                        //    }
                        //    else
                        //    {
                        //        params = msg.parameters;
                        //    }
                        //}
        }

        [MessageHandler(typeof(ChatServerMessage))]
        public static void ChatServerMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            ChatServerMessage msg = (ChatServerMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            switch ((int)msg.channel)
            {
                case 0:
                    account.Log(new GeneralTextInformation(msg.senderName + ": " + msg.content),1);
                    break;
                case 1:
                    //account.Log(new DofAlertTextInformation("Bienvenue sur DOFUS, dans le Monde des Douze !" + System.Environment.NewLine + "Il est interdit de transmettre votre identifiant ou votre mot de passe."));
                    break;
                case 2:
                    account.Log(new GuildTextInformation(msg.senderName + ": " + msg.content),1);
                    break;
                case 3:
                    account.Log(new AllianceTextInformation(msg.senderName + ": " + msg.content),1);
                    break;
                case 5:
                    account.Log(new CommerceTextInformation(msg.senderName + ": " + msg.content),1);
                    break;
                case 6:
                    account.Log(new RecrutementTextInformation(msg.senderName + ": " + msg.content),1);
                    break;
                case 9:
                    account.Log(new PrivateTextInformation("de " + msg.senderName + " : " + msg.content),1);
                    break;
            }
        }

        [MessageHandler(typeof(GameMapMovementConfirmMessage))]
        public static void GameMapMovementConfirmMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameMapMovementConfirmMessage msg = (GameMapMovementConfirmMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
           BlueSheep.Core.Fight.Entity Character = null;
            foreach (BlueSheep.Core.Fight.Entity e in account.Map.Entities)
            {
                if (e.Id == account.CharacterBaseInformations.id)
                    Character = e;
            }
            int mapChangeData = ((BlueSheep.Data.D2p.Map)account.Map.Data).Cells[Character.CellId].MapChangeData;
                if (mapChangeData != 0)
                {
                    int neighbourId = 0;
                    if (neighbourId == -2)
                    {
                        if ((mapChangeData & 64) > 0)
                            neighbourId = ((BlueSheep.Data.D2p.Map)account.Map.Data).TopNeighbourId;
                        if ((mapChangeData & 16) > 0)
                            neighbourId = ((BlueSheep.Data.D2p.Map)account.Map.Data).LeftNeighbourId;
                        if ((mapChangeData & 4) > 0)
                            neighbourId = ((BlueSheep.Data.D2p.Map)account.Map.Data).BottomNeighbourId;
                        if ((mapChangeData & 1) > 0)
                            neighbourId = ((BlueSheep.Data.D2p.Map)account.Map.Data).RightNeighbourId;
                    }
                    if (neighbourId >= 0)
                        account.Map.LaunchChangeMap(neighbourId);
                }
                account.SetStatus(Status.None);

        }

        [MessageHandler(typeof(GameMapMovementMessage))]
        public static void GameMapMovementMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameMapMovementMessage msg = (GameMapMovementMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
                List<uint> keys = new List<uint>();
                foreach (short s in msg.keyMovements)
                {
                    keys.Add((uint)s);
                }
                MovementPath clientMovement = MapMovementAdapter.GetClientMovement(keys);
                BlueSheep.Core.Fight.Entity entity = null;
                foreach (BlueSheep.Core.Fight.Entity en in account.Map.Entities)
                {
                    if (en.Id == msg.actorId)
                        entity = en;
                }
                if (entity != null)
                    (account.Map.Entities[account.Map.Entities.IndexOf(entity)]).CellId = clientMovement.CellEnd.CellId;
                
                //GameRolePlayGroupMonsterInformations e = null;
                foreach (MonsterGroup en in account.Map.List)
                {
                        if (en.m_contextualId == msg.actorId)
                        {
                            en.m_cellId = clientMovement.CellEnd.CellId;
                        }
                    
                }
                //if (e != null)
                //{
                //    account.Map.List.RemoveAt(i);
                //    account.Map.List.Add(new GameRolePlayGroupMonsterInformations(msg.actorId, e.look, new EntityDispositionInformations((short)clientMovement.CellEnd.CellId, e.disposition.direction), e.keyRingBonus, e.hasHardcoreDrop, e.hasAVARewardToken, e.staticInfos, e.ageBonus, e.lootShare, e.alignmentSide));
                //}
                //if (msg.actorId == account.Map.Character.Id)
                //{
                //    account.Wait(100, 200);
                //    using (BigEndianWriter writer = new BigEndianWriter())
                //    {
                //        GameMapMovementConfirmMessage newmsg = new GameMapMovementConfirmMessage();
                //        newmsg.Serialize(writer);
                //        MessagePackaging pack = new MessagePackaging(writer);
                //        pack.Pack((int)newmsg.ProtocolID);
                //        account.SocketManager.Send(pack.Writer.Content);
                //    }
                //}
                //    if (account.Fight.flag != 0)
                //    {
                //        account.Fight.LaunchFight(account.Fight.flag);
                //    }
                //}
                
            
        }

        [MessageHandler(typeof(GameMapNoMovementMessage))]
        public static void GameMapNoMovementMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameMapNoMovementMessage msg = (GameMapNoMovementMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }

            if (account.Fight != null && account.Fight.IsFollowingGroup())
                account.Fight.LaunchFight(account.Fight.followinggroup.m_contextualId);

        }

        [MessageHandler(typeof(PopupWarningMessage))]
        public static void PopupWarningMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            PopupWarningMessage msg = (PopupWarningMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Log(new ErrorTextInformation("Y a un popup sur l'écran, surement un modo :s"), 0);
            account.SocketManager.DisconnectFromGUI();

        }

        [MessageHandler(typeof(PartyInvitationMessage))]
        public static void PartyInvitationMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            PartyInvitationMessage msg = (PartyInvitationMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.MyGroup != null && msg.fromName == account.MyGroup.GetMaster().CharacterBaseInformations.name)
            {

                    PartyAcceptInvitationMessage msg2 = new PartyAcceptInvitationMessage(msg.partyId);
                    account.SocketManager.Send(msg2);
                    account.Log(new BotTextInformation("J'ai rejoint le groupe :3"),3);
                
            }
            else
            {
                    PartyRefuseInvitationMessage msg2 = new PartyRefuseInvitationMessage(msg.partyId);
                    account.SocketManager.Send(msg2);
                
            }
        }

        [MessageHandler(typeof(PartyMemberInFightMessage))]
        public static void PartyMemberInFightMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            PartyMemberInFightMessage msg = (PartyMemberInFightMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (msg.fightMap.mapId == account.Map.Id && msg.memberName == account.MyGroup.GetMaster().CharacterBaseInformations.name)
            {
                account.Wait(500, 1500);
                using (BigEndianWriter writer = new BigEndianWriter())
                {
                    GameFightJoinRequestMessage msg2 = new GameFightJoinRequestMessage(msg.memberId, msg.fightId);
                    account.SocketManager.Send(msg2);
                }
            }

        }

        [MessageHandler(typeof(InteractiveElementUpdatedMessage))]
        public static void InteractiveElementUpdatedMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            InteractiveElementUpdatedMessage msg = (InteractiveElementUpdatedMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.House != null)
            {
                InteractiveElement e = msg.interactiveElement;
                account.House.ElementIdd = e.elementId;
                InteractiveElementSkill[] EnabledSkills = e.enabledSkills;
                account.House.SkillInstanceID = EnabledSkills[1].skillInstanceUid;
                account.House.UseHouse();
            }
            if (account.Map.InteractiveElements.Count > 0)
            {
                account.Map.InteractiveElements.Remove(msg.interactiveElement.elementId);
                account.Map.InteractiveElements.Add(msg.interactiveElement.elementId, new Core.Map.Elements.InteractiveElement((uint)msg.interactiveElement.elementId, msg.interactiveElement.elementTypeId,new List<InteractiveElementSkill>(msg.interactiveElement.enabledSkills), new List<InteractiveElementSkill>(msg.interactiveElement.disabledSkills)));
            }

        }

        [MessageHandler(typeof(StatedElementUpdatedMessage))]
        public static void StatedElementUpdatedMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            StatedElementUpdatedMessage msg = (StatedElementUpdatedMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.Map.StatedElements.Count > 0)
                account.Map.StatedElements[msg.statedElement.elementId].State = (uint)msg.statedElement.elementState;
        }
        
        [MessageHandler(typeof(PurchasableDialogMessage))]
        public static void PurchasableDialogMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            PurchasableDialogMessage msg = (PurchasableDialogMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.House != null)
            {
                account.House.priceHouse = msg.price;
                if (account.House.priceHouse < Convert.ToInt32(account.MaxPrice.Value))
                {
                    account.House.Buy();
                }
                else
                {
                    account.Log(new ErrorTextInformation("Prix trop élevé..."),2);
                }

            }

        }

        [MessageHandler(typeof(HousePropertiesMessage))]
        public static void HousePropertiesMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            HousePropertiesMessage msg = (HousePropertiesMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (msg.properties.ownerName == "?")
            {
                if (account.SearcherLogBox.Text.Length > 0)
                {
                    StreamWriter SourceFile = new StreamWriter(account.SearcherLogBox.Text, true);
                    SourceFile.WriteLine("Maison abandonnée en : " + "[" + account.Map.X + ";" + account.Map.Y + "]");
                    SourceFile.Close();
                }
                account.Log(new BotTextInformation("Maison abandonnée en : " + "[" + account.Map.X + ";" + account.Map.Y + "]"),1);
            }
        }

        [MessageHandler(typeof(GameContextRemoveElementMessage))]
        public static void GameContextRemoveElementMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameContextRemoveElementMessage msg = (GameContextRemoveElementMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Map.Entities.RemoveAll(f => f.Id == msg.id);
            account.Map.List.RemoveAll(f => f.m_contextualId == msg.id);
        }

        [MessageHandler(typeof(GameRolePlayShowActorMessage))]
        public static void GameRolePlayShowActorMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameRolePlayShowActorMessage msg = (GameRolePlayShowActorMessage)message;
            account.HeroicUC.AnalysePacket(msg, packetDatas);
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            GameRolePlayHumanoidInformations info = (GameRolePlayHumanoidInformations)msg.informations;
            if (account.StartStopFloodingBox.Checked == true && account.PrivateEnterBox.Checked == true && msg.informations is GameRolePlayHumanoidInformations)
            {
                account.Flood.SendPrivateTo(info.name); 
            }
            if(account.IsMemoryCheck.Checked == true && msg.informations is GameRolePlayHumanoidInformations)
            {
                account.Flood.SaveNameInMemory(info.name);
            }
        }

        [MessageHandler(typeof(ExchangeStartedWithStorageMessage))]
        public static void ExchangeStartedWithStorageMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            ExchangeStartedWithStorageMessage msg = (ExchangeStartedWithStorageMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.Path != null && account.Inventory != null)
            {
                List<int> items = account.GestItemsUC.GetItemsToTransfer();
                account.Inventory.TransferItems(items);
                account.Inventory.GetItems(account.GestItemsUC.GetItemsToGetFromBank());


            }
        }

        [MessageHandler(typeof(DisplayNumericalValuePaddockMessage))]
        public static void DisplayNumericalValueMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            DisplayNumericalValuePaddockMessage msg = (DisplayNumericalValuePaddockMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            
            
        }

        [MessageHandler(typeof(ObtainedItemMessage))]
        public static void ObtainedItemMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            ObtainedItemMessage msg = (ObtainedItemMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
                if (account.Gather.resourceName == "Unknown")
                    return;
                account.SetStatus(Status.None);
                account.Log(new ActionTextInformation("Ressource récoltée : " + account.Gather.resourceName + " +" + msg.baseQuantity), 3);
                if (account.Gather.Stats.ContainsKey(account.Gather.resourceName))
                    account.Gather.Stats[account.Gather.resourceName] += msg.baseQuantity;
                else
                    account.Gather.Stats.Add(account.Gather.resourceName, msg.baseQuantity);
                account.Gather.Current_Job.ActualizeStats(account.Gather.Stats);
                if (account.PerformGather() == false && account.Path != null)
                    account.Path.PerformActionsStack();
            
        }

        ////////////////////////////////// PACKET DELETED ///////////////////////////////////////////////

        //[MessageHandler(typeof(DisplayNumericalValueWithAgeBonusMessage))]
        //public static void DisplayNumericalValueWithAgeBonusTreatment(Message message, byte[] packetDatas, AccountUC account)
        //{
        //    DisplayNumericalValueWithAgeBonusMessage msg = (DisplayNumericalValueWithAgeBonusMessage)message;

        //    using (BigEndianReader reader = new BigEndianReader(packetDatas))
        //    {
        //        msg.Deserialize(reader);
        //    }
        //    account.ModifBar(6, 0, 0, "Connecté");
        //    if ((int)msg.type == 1 && msg.entityId == account.CharacterBaseInformations.id)
        //    {
        //        if (account.Gather.resourceName == "Unknown")
        //            return;
        //        account.Log(new ActionTextInformation("Ressource récoltée : " + account.Gather.resourceName + " +" + msg.value + msg.valueOfBonus), 3);
        //        if (account.Gather.Stats.ContainsKey(account.Gather.resourceName))
        //            account.Gather.Stats[account.Gather.resourceName] += msg.value + msg.valueOfBonus;
        //        else
        //            account.Gather.Stats.Add(account.Gather.resourceName, msg.value + msg.valueOfBonus);
        //        account.Gather.Current_Job.ActualizeStats(account.Gather.Stats);
        //        if (account.PerformGather() == false && account.Path != null)
        //            account.Path.PerformActionsStack();
        //    }
        //}

        [MessageHandler(typeof(InteractiveUseErrorMessage))]
        public static void InteractiveUseErrorMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            InteractiveUseErrorMessage msg = (InteractiveUseErrorMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Log(new ErrorTextInformation("Erreur lors de l'utilisation de l'element numero " + msg.elemId + ". Si vous connaissez la raison, rapportez la sur le forum. Merci ! Poursuite du trajet..."), 0);
            if (account.Gather.Id != -1 && account.PerformGather() == false)
                if (account.Path != null)
                    account.Path.PerformActionsStack();
            else if (account.Path != null)
                account.Path.PerformActionsStack();
        }
        #endregion
    }
}
