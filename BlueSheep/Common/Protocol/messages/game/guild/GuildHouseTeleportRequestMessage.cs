









// Generated on 12/11/2014 19:01:44
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildHouseTeleportRequestMessage : Message
    {
        public new const uint ID =5712;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int houseId;
        
        public GuildHouseTeleportRequestMessage()
        {
        }
        
        public GuildHouseTeleportRequestMessage(int houseId)
        {
            this.houseId = houseId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(houseId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            houseId = reader.ReadVarInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
        }
        
    }
    
}