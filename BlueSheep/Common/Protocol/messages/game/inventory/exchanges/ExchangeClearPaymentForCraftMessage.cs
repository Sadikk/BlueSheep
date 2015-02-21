









// Generated on 12/11/2014 19:01:48
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeClearPaymentForCraftMessage : Message
    {
        public new const uint ID =6145;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte paymentType;
        
        public ExchangeClearPaymentForCraftMessage()
        {
        }
        
        public ExchangeClearPaymentForCraftMessage(sbyte paymentType)
        {
            this.paymentType = paymentType;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(paymentType);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            paymentType = reader.ReadSByte();
            if (paymentType < 0)
                throw new Exception("Forbidden value on paymentType = " + paymentType + ", it doesn't respect the following condition : paymentType < 0");
        }
        
    }
    
}