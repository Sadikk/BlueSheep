









// Generated on 12/11/2014 19:01:49
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeItemGoldAddAsPaymentMessage : Message
    {
        public new const uint ID =5770;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte paymentType;
        public int quantity;
        
        public ExchangeItemGoldAddAsPaymentMessage()
        {
        }
        
        public ExchangeItemGoldAddAsPaymentMessage(sbyte paymentType, int quantity)
        {
            this.paymentType = paymentType;
            this.quantity = quantity;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(paymentType);
            writer.WriteVarInt(quantity);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            paymentType = reader.ReadSByte();
            if (paymentType < 0)
                throw new Exception("Forbidden value on paymentType = " + paymentType + ", it doesn't respect the following condition : paymentType < 0");
            quantity = reader.ReadVarInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}