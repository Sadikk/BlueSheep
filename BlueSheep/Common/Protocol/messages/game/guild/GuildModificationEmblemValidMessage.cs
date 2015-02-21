









// Generated on 12/11/2014 19:01:45
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildModificationEmblemValidMessage : Message
    {
        public new const uint ID =6328;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.GuildEmblem guildEmblem;
        
        public GuildModificationEmblemValidMessage()
        {
        }
        
        public GuildModificationEmblemValidMessage(Types.GuildEmblem guildEmblem)
        {
            this.guildEmblem = guildEmblem;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            guildEmblem.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            guildEmblem = new Types.GuildEmblem();
            guildEmblem.Deserialize(reader);
        }
        
    }
    
}