using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Engine.Enums;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Engine.Handlers.Context
{
    class NpcHandler
    {
        [MessageHandler(typeof(ExchangeStartOkNpcShopMessage))]
        public static void GameContextRemoveElementMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            ExchangeStartOkNpcShopMessage msg = (ExchangeStartOkNpcShopMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.SetStatus(Status.Exchanging);
        }

        [MessageHandler(typeof(LeaveDialogMessage))]
        public static void NpcDialogCreationMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            LeaveDialogMessage msg = (LeaveDialogMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.SetStatus(Status.None);
        }

        [MessageHandler(typeof(NpcDialogCreationMessage))]
        public static void LeaveDialogMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            NpcDialogCreationMessage msg = (NpcDialogCreationMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Npc.Id = msg.npcId;
            account.Npc.Entity = account.MapData.Npcs.FirstOrDefault((npc) => npc.contextualId == msg.npcId);
            account.SetStatus(Status.Speaking);
        }

        [MessageHandler(typeof(NpcDialogQuestionMessage))]
        public static void NpcDialogQuestionMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            NpcDialogQuestionMessage msg = (NpcDialogQuestionMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Npc.QuestionId = msg.messageId;
            int mess = (int)GameData.GetDataObject(D2oFileEnum.NpcMessages, account.Npc.QuestionId).Fields["messageId"];
            account.Log(new BotTextInformation("Dialogue : " + BlueSheep.Common.Data.I18N.GetText(mess)), 0);
            if (account.Npc.QuestionId == 318 && (int)msg.visibleReplies[0] == 259)
            {
                //Bank
                account.Npc.SendReply(259);
                return;
            }
            else if (account.Npc.QuestionId == 318)
            {
                account.Log(new ErrorTextInformation("Vous n'êtes pas level 10, vous ne pouvez pas utiliser la banque. Fermeture du dialogue."), 0);
                account.Npc.CloseDialog();
            }
            if (msg.visibleReplies.Length == 0)
                account.Npc.CloseDialog();
            account.Npc.Replies.Clear();
            account.Npc.Replies = msg.visibleReplies.Select<short, BlueSheep.Core.Npc.NpcReply>((id) => new BlueSheep.Core.Npc.NpcReply(account.MapData.Npcs.Find(n => n.contextualId == account.Npc.Id).npcId, id)).ToList();
            if (account.Path != null)
            {
                account.Path.SearchReplies(BlueSheep.Common.Data.I18N.GetText(mess));
            }
        }

    }
}
