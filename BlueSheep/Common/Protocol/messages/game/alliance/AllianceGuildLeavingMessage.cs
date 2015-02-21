









// Generated on 12/11/2014 19:01:20
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AllianceGuildLeavingMessage : Message
    {
        public new const uint ID =6399;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool kicked;
        public int guildId;
        
        public AllianceGuildLeavingMessage()
        {
        }
        
        public AllianceGuildLeavingMessage(bool kicked, int guildId)
        {
            this.kicked = kicked;
            this.guildId = guildId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(kicked);
            writer.WriteVarInt(guildId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            kicked = reader.ReadBoolean();
            guildId = reader.ReadVarInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
        }
        
    }
    
}