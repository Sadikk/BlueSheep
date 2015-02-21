









// Generated on 12/11/2014 19:01:55
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ObjectFoundWhileRecoltingMessage : Message
    {
        public new const uint ID =6017;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short genericId;
        public int quantity;
        public int resourceGenericId;
        
        public ObjectFoundWhileRecoltingMessage()
        {
        }
        
        public ObjectFoundWhileRecoltingMessage(short genericId, int quantity, int resourceGenericId)
        {
            this.genericId = genericId;
            this.quantity = quantity;
            this.resourceGenericId = resourceGenericId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(genericId);
            writer.WriteVarInt(quantity);
            writer.WriteVarInt(resourceGenericId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            genericId = reader.ReadVarShort();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            quantity = reader.ReadVarInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
            resourceGenericId = reader.ReadVarInt();
            if (resourceGenericId < 0)
                throw new Exception("Forbidden value on resourceGenericId = " + resourceGenericId + ", it doesn't respect the following condition : resourceGenericId < 0");
        }
        
    }
    
}