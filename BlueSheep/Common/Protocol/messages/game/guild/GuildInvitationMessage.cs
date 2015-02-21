









// Generated on 12/11/2014 19:01:44
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildInvitationMessage : Message
    {
        public new const uint ID =5551;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int targetId;
        
        public GuildInvitationMessage()
        {
        }
        
        public GuildInvitationMessage(int targetId)
        {
            this.targetId = targetId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(targetId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            targetId = reader.ReadVarInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
        }
        
    }
    
}