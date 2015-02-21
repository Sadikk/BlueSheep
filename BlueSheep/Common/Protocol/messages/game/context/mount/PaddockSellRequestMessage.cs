









// Generated on 12/11/2014 19:01:30
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class PaddockSellRequestMessage : Message
    {
        public new const uint ID =5953;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int price;
        
        public PaddockSellRequestMessage()
        {
        }
        
        public PaddockSellRequestMessage(int price)
        {
            this.price = price;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(price);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            price = reader.ReadVarInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
        }
        
    }
    
}