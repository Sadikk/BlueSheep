using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Common.Types;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Core.Npc
{
    public class Npc
    {
        #region Fields
        public AccountUC account { get; set; }
        public BlueSheep.Core.Fight.Entity Entity { get; set; }
        public int Id { get; set; }
        public Dictionary<int, int> Npcs { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText
        {
            get { return BlueSheep.Common.Data.I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.NpcMessages, QuestionId).Fields["messageId"]); }
        }

        public List<NpcReply> Replies { get; set; }
        #endregion

        #region Constructors
        public Npc(AccountUC Account)
        {
            account = Account;
            Entity = null;
            Npcs = new Dictionary<int, int>();
            Replies = new List<NpcReply>();
        }
        #endregion

        #region Public Methods
        public void CloseDialog()
        {
            Replies.Clear();
            QuestionId = 0;
            Id = 0;
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                LeaveDialogRequestMessage msg = new LeaveDialogRequestMessage();
                msg.Serialize(writer);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
            }
        }

        public void SendReply(int replyId)
        {
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                NpcDialogReplyMessage msg = new NpcDialogReplyMessage((short)replyId);
                msg.Serialize(writer);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
            }
        }

        public string GetNpcName(int npcId)
        {
            return BlueSheep.Common.Data.I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.NpcMessages, npcId).Fields["nameId"]);
        }

        public void TalkToNpc(int npcId)
        {
            npcId = FindContextIdFromNpcId(npcId);
            if (npcId == 0)
                return;
            account.Busy = true;
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                NpcGenericActionRequestMessage msg = new NpcGenericActionRequestMessage(npcId, 3, account.Map.Id);
                msg.Serialize(writer);
                writer.Content = account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
            }
        }

        public int FindContextIdFromNpcId(int npcid)
        {
            if (npcid == 0)
                return Npcs.ToList()[0].Key;
            foreach (KeyValuePair<int, int> p in Npcs)
            {
                if (p.Value == npcid)
                    return p.Key;
            }
            return 0;
        }

#endregion
    
    }


}
