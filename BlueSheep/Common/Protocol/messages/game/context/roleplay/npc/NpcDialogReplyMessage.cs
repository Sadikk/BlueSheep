









// Generated on 12/11/2014 19:01:35
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class NpcDialogReplyMessage : Message
    {
        public new const uint ID =5616;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short replyId;
        
        public NpcDialogReplyMessage()
        {
        }
        
        public NpcDialogReplyMessage(short replyId)
        {
            this.replyId = replyId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(replyId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            replyId = reader.ReadVarShort();
            if (replyId < 0)
                throw new Exception("Forbidden value on replyId = " + replyId + ", it doesn't respect the following condition : replyId < 0");
        }
        
    }
    
}