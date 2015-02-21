









// Generated on 12/11/2014 19:01:55
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ObtainedItemMessage : Message
    {
        public new const uint ID =6519;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short genericId;
        public int baseQuantity;
        
        public ObtainedItemMessage()
        {
        }
        
        public ObtainedItemMessage(short genericId, int baseQuantity)
        {
            this.genericId = genericId;
            this.baseQuantity = baseQuantity;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(genericId);
            writer.WriteVarInt(baseQuantity);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            genericId = reader.ReadVarShort();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            baseQuantity = reader.ReadVarInt();
            if (baseQuantity < 0)
                throw new Exception("Forbidden value on baseQuantity = " + baseQuantity + ", it doesn't respect the following condition : baseQuantity < 0");
        }
        
    }
    
}