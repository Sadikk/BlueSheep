









// Generated on 12/11/2014 19:01:19
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AllianceCreationValidMessage : Message
    {
        public new const uint ID =6393;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string allianceName;
        public string allianceTag;
        public Types.GuildEmblem allianceEmblem;
        
        public AllianceCreationValidMessage()
        {
        }
        
        public AllianceCreationValidMessage(string allianceName, string allianceTag, Types.GuildEmblem allianceEmblem)
        {
            this.allianceName = allianceName;
            this.allianceTag = allianceTag;
            this.allianceEmblem = allianceEmblem;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(allianceName);
            writer.WriteUTF(allianceTag);
            allianceEmblem.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            allianceName = reader.ReadUTF();
            allianceTag = reader.ReadUTF();
            allianceEmblem = new Types.GuildEmblem();
            allianceEmblem.Deserialize(reader);
        }
        
    }
    
}