









// Generated on 12/11/2014 19:01:45
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildMemberOnlineStatusMessage : Message
    {
        public new const uint ID =6061;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int memberId;
        public bool online;
        
        public GuildMemberOnlineStatusMessage()
        {
        }
        
        public GuildMemberOnlineStatusMessage(int memberId, bool online)
        {
            this.memberId = memberId;
            this.online = online;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(memberId);
            writer.WriteBoolean(online);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            memberId = reader.ReadVarInt();
            if (memberId < 0)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
            online = reader.ReadBoolean();
        }
        
    }
    
}