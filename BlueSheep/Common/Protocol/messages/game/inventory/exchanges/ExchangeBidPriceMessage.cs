









// Generated on 12/11/2014 19:01:48
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeBidPriceMessage : Message
    {
        public new const uint ID =5755;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short genericId;
        public int averagePrice;
        
        public ExchangeBidPriceMessage()
        {
        }
        
        public ExchangeBidPriceMessage(short genericId, int averagePrice)
        {
            this.genericId = genericId;
            this.averagePrice = averagePrice;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(genericId);
            writer.WriteVarInt(averagePrice);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            genericId = reader.ReadVarShort();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            averagePrice = reader.ReadVarInt();
        }
        
    }
    
}