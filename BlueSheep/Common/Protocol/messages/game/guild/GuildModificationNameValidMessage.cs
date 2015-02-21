









// Generated on 12/11/2014 19:01:45
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildModificationNameValidMessage : Message
    {
        public new const uint ID =6327;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string guildName;
        
        public GuildModificationNameValidMessage()
        {
        }
        
        public GuildModificationNameValidMessage(string guildName)
        {
            this.guildName = guildName;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(guildName);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            guildName = reader.ReadUTF();
        }
        
    }
    
}