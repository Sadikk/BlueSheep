









// Generated on 12/11/2014 19:01:34
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class HouseGuildNoneMessage : Message
    {
        public new const uint ID =5701;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short houseId;
        
        public HouseGuildNoneMessage()
        {
        }
        
        public HouseGuildNoneMessage(short houseId)
        {
            this.houseId = houseId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(houseId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            houseId = reader.ReadVarShort();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
        }
        
    }
    
}