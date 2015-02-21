









// Generated on 12/11/2014 19:01:45
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildMemberLeavingMessage : Message
    {
        public new const uint ID =5923;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool kicked;
        public int memberId;
        
        public GuildMemberLeavingMessage()
        {
        }
        
        public GuildMemberLeavingMessage(bool kicked, int memberId)
        {
            this.kicked = kicked;
            this.memberId = memberId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(kicked);
            writer.WriteInt(memberId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            kicked = reader.ReadBoolean();
            memberId = reader.ReadInt();
        }
        
    }
    
}