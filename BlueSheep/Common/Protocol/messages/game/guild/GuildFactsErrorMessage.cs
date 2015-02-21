









// Generated on 12/11/2014 19:01:44
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildFactsErrorMessage : Message
    {
        public new const uint ID =6424;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int guildId;
        
        public GuildFactsErrorMessage()
        {
        }
        
        public GuildFactsErrorMessage(int guildId)
        {
            this.guildId = guildId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(guildId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            guildId = reader.ReadVarInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
        }
        
    }
    
}