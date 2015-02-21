









// Generated on 12/11/2014 19:01:49
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeItemObjectAddAsPaymentMessage : Message
    {
        public new const uint ID =5766;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte paymentType;
        public bool bAdd;
        public int objectToMoveId;
        public int quantity;
        
        public ExchangeItemObjectAddAsPaymentMessage()
        {
        }
        
        public ExchangeItemObjectAddAsPaymentMessage(sbyte paymentType, bool bAdd, int objectToMoveId, int quantity)
        {
            this.paymentType = paymentType;
            this.bAdd = bAdd;
            this.objectToMoveId = objectToMoveId;
            this.quantity = quantity;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(paymentType);
            writer.WriteBoolean(bAdd);
            writer.WriteVarInt(objectToMoveId);
            writer.WriteVarInt(quantity);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            paymentType = reader.ReadSByte();
            if (paymentType < 0)
                throw new Exception("Forbidden value on paymentType = " + paymentType + ", it doesn't respect the following condition : paymentType < 0");
            bAdd = reader.ReadBoolean();
            objectToMoveId = reader.ReadVarInt();
            if (objectToMoveId < 0)
                throw new Exception("Forbidden value on objectToMoveId = " + objectToMoveId + ", it doesn't respect the following condition : objectToMoveId < 0");
            quantity = reader.ReadVarInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}