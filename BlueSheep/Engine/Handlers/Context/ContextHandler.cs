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
            MapComplementaryInformationsDataMessageTreatment(message, packetDatas, account);
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
            account.Gather.ClearError();
            account.MapData.Clear();
            account.MapData.ParseLocation(msg.mapId, msg.subAreaId);
            account.MapData.ParseStatedElements(msg.statedElements);
            account.MapData.ParseActors(msg.actors);
            account.MapData.ParseInteractiveElements(msg.interactiveElements);
            account.Enable(true);
            account.MapData.DoAction();
            account.ActualizeMap();
        }

        [MessageHandler(typeof(MapComplementaryInformationsWithCoordsMessage))]
        public static void MapComplementaryInformationsWithCoordsMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            MapComplementaryInformationsDataMessageTreatment(message, packetDatas, account);
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
            if (account.MapID == account.MapData.LastMapId && account.Fight != null)
            {
                account.FightData.winLoseDic["Gagné"]++;
                account.ActualizeFightStats(account.FightData.winLoseDic, account.FightData.xpWon);
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
        public static void TextInformationMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            TextInformationMessage msg = (TextInformationMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            switch (msg.msgId)
            {
                //case 89:
                //    account.Log(new DofAlertTextInformation("Bienvenue sur DOFUS, dans le Monde des Douze !" + System.Environment.NewLine + "Il est interdit de transmettre votre identifiant ou votre mot de passe."), 1);
                //    break;
                //case 153:
                //    account.Log(new DofInfoCanal("Votre adresse ip actuelle est " + msg.parameters[0]), 0);
                //    break;
                //case 171:
                //    account.Log(new ErrorTextInformation(string.Format("Impossible de lancer ce sort, vous avez une portée de {0} à {1}, et vous visez à {2} !", msg.parameters[0], msg.parameters[1], msg.parameters[2])), 4);
                //    break;
                case 34:
                    //account.Log(new ErrorTextInformation(string.Format("Vous avez perdu {0} points d'énergie", msg.parameters[0])), 0);
                    //account.Log(new ErrorTextInformation("Combat perdu"), 0);
                    if (account.Fight != null)
                    {
                        account.FightData.winLoseDic["Perdu"]++;
                        account.ActualizeFightStats(account.FightData.winLoseDic, account.FightData.xpWon);
                    }
                    break;
            }
            //default:


            DataClass data = GameData.GetDataObject(D2oFileEnum.InfoMessages, msg.msgType * 10000 + msg.msgId);
            string text = I18N.GetText((int)data.Fields["textId"]);
            for (int i = 0; i < msg.parameters.Length; i++)
            {
                var parameter = msg.parameters[i];
                text = text.Replace("%" + (i + 1), parameter);
            }
            account.Log(new DofAlertTextInformation(text), 0);
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

        //[MessageHandler(typeof(GameMapMovementConfirmMessage))]
        //public static void GameMapMovementConfirmMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        //{
        //    GameMapMovementConfirmMessage msg = (GameMapMovementConfirmMessage)message;

        //    using (BigEndianReader reader = new BigEndianReader(packetDatas))
        //    {
        //        msg.Deserialize(reader);
        //    }
        //    BlueSheep.Core.Fight.Entity Character = null;
        //    foreach (BlueSheep.Core.Fight.Entity e in account.Map.Entities)
        //    {
        //        if (e.Id == account.CharacterBaseInformations.id)
        //            Character = e;
        //    }
        //    int mapChangeData = ((BlueSheep.Data.D2p.Map)account.Map.Data).Cells[Character.CellId].MapChangeData;
        //    if (mapChangeData != 0)
        //    {
        //        int neighbourId = 0;
        //        if (neighbourId == -2)
        //        {
        //            if ((mapChangeData & 64) > 0)
        //                neighbourId = ((BlueSheep.Data.D2p.Map)account.Map.Data).TopNeighbourId;
        //            if ((mapChangeData & 16) > 0)
        //                neighbourId = ((BlueSheep.Data.D2p.Map)account.Map.Data).LeftNeighbourId;
        //            if ((mapChangeData & 4) > 0)
        //                neighbourId = ((BlueSheep.Data.D2p.Map)account.Map.Data).BottomNeighbourId;
        //            if ((mapChangeData & 1) > 0)
        //                neighbourId = ((BlueSheep.Data.D2p.Map)account.Map.Data).RightNeighbourId;
        //        }
        //        if (neighbourId >= 0)
        //            account.Map.LaunchChangeMap(neighbourId);
        //    }
        //    account.SetStatus(Status.None);

        //}

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
            account.MapData.UpdateEntityCell(msg.actorId, clientMovement.CellEnd.CellId);
            if (account.Map.Moving && msg.actorId == account.MapData.Character.contextualId)
            {
                account.Map.ConfirmMove();
            }
        }

        [MessageHandler(typeof(GameMapNoMovementMessage))]
        public static void GameMapNoMovementMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameMapNoMovementMessage msg = (GameMapNoMovementMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            //if (account.Fight != null && account.FightData.IsFollowingGroup)
            //{
            //    account.Fight.LaunchFight(account.FightData.followingGroup.m_contextualId);
            //    return;
            //}
            //if (account.Path != null)
            //{
            //    account.SetStatus(Status.None);
            //    account.Log(new DebugTextInformation("[Path] NoMovement : Continue the path..."), 0);
            //    account.Path.PerformActionsStack();
            //}
            //account.Map.Moving = false;
            //account.Map.ConfirmMove();
            if (account.Path != null)
                account.Path.PerformActionsStack();
                
            //else if (account.Map.Moving)
            //{
            //    account.SetStatus(Status.None);
            //    if (account.Path != null)
            //        account.Path.ParsePath();
            //}

        }

        [MessageHandler(typeof(PopupWarningMessage))]
        public static void PopupWarningMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            PopupWarningMessage msg = (PopupWarningMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Log(new ErrorTextInformation("[FROM " + msg.author + " ] : " + msg.content),0);
            account.Log(new BotTextInformation("You has been locked for " + msg.lockDuration + ". Stopping BlueSheep actions while blocked..."),0);
            account.Log(new ErrorTextInformation("Y a un popup sur l'écran, surement un modo :s"), 0);
             account.Wait(msg.lockDuration, msg.lockDuration);
            //account.SocketManager.Disconnect("Alerte au modo ! Alerte au modo !");
        }

        [MessageHandler(typeof(SystemMessageDisplayMessage))]
        public static void SystemMessageDisplayMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            SystemMessageDisplayMessage msg = (SystemMessageDisplayMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
           // account.SocketManager.Disconnect("Alerte au modo ! Alerte au modo !");
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
            if (msg.fightMap.mapId == account.MapData.Id && msg.memberName == account.MyGroup.GetMaster().CharacterBaseInformations.name)
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
            account.MapData.UpdateInteractiveElement(msg.interactiveElement);

        }

        [MessageHandler(typeof(StatedElementUpdatedMessage))]
        public static void StatedElementUpdatedMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            StatedElementUpdatedMessage msg = (StatedElementUpdatedMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.MapData.UpdateStatedElement(msg.statedElement);
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
                    SourceFile.WriteLine("Maison abandonnée en : " + "[" + account.MapData.X + ";" + account.MapData.Y + "]");
                    SourceFile.Close();
                }
                account.Log(new BotTextInformation("Maison abandonnée en : " + "[" + account.MapData.X + ";" + account.MapData.Y + "]"),1);
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
            account.MapData.Remove(msg.id);
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

            if (account.FloodUC.StartStopFloodingBox.Checked == true && account.FloodUC.PrivateEnterBox.Checked == true && msg.informations is GameRolePlayCharacterInformations)
            {
                GameRolePlayCharacterInformations infos = (GameRolePlayCharacterInformations)msg.informations;
                account.Flood.SendPrivateTo(infos); 
            }
            if (account.FloodUC.IsMemoryCheck.Checked == true && msg.informations is GameRolePlayCharacterInformations)
            {
                GameRolePlayCharacterInformations infos = (GameRolePlayCharacterInformations)msg.informations;
                account.Flood.SaveNameInMemory(infos);
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
            if (account.Gather.Current_El == null)
                return;
            account.Log(new ActionTextInformation("Ressource récoltée : " + account.Gather.resourceName + " +" + msg.baseQuantity), 3);
            if (account.Gather.Stats.ContainsKey(account.Gather.resourceName))
                account.Gather.Stats[account.Gather.resourceName] += msg.baseQuantity;
            else
                account.Gather.Stats.Add(account.Gather.resourceName, msg.baseQuantity);
            account.Gather.Current_Job.ActualizeStats(account.Gather.Stats);
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
            //if (account.Gather.Error())
            //    return;
            account.Gather.BanElementId(account.Gather.Id);
            //account.Log(new ErrorTextInformation("Erreur lors de l'utilisation de l'element numero " + msg.elemId + ". Pg lelz. Poursuite du trajet."), 0);
            if (account.Path != null)
                account.Path.PerformFlag();
        }

        [MessageHandler(typeof(InteractiveUseEndedMessage))]
        public static void InteractiveUseEndedMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            InteractiveUseEndedMessage msg = (InteractiveUseEndedMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.Gather.Id == -1)
                return;
            account.SetStatus(Status.None);
            account.Gather.Id = -1;
            if (account.Path != null)
                account.Path.PerformFlag();
        }

        [MessageHandler(typeof(ExchangeStartedWithPodsMessage))]
        public static void ExchangeStartedWithPodsMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            ExchangeStartedWithPodsMessage msg = (ExchangeStartedWithPodsMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.GestItemsUC.ListenerBox.Checked)
                return;
            List<int> items = account.GestItemsUC.GetItemsToTransfer();
            account.Inventory.TransferItems(items);
            account.Wait(2000, 3000);
            account.Inventory.ExchangeReady();
        }

        [MessageHandler(typeof(ExchangeRequestedTradeMessage))]
        public static void ExchangeRequestedTradeMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            ExchangeRequestedTradeMessage msg = (ExchangeRequestedTradeMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.GestItemsUC.ListenerBox.Checked)
                account.Inventory.AcceptExchange();
        }

        [MessageHandler(typeof(ExchangeIsReadyMessage))]
        public static void ExchangeIsReadyMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            ExchangeIsReadyMessage msg = (ExchangeIsReadyMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (msg.ready && account.GestItemsUC.ListenerBox.Checked)
                account.Inventory.ExchangeReady();
        }

                        
        #endregion
    }
}
