









// Generated on 12/11/2014 19:01:34
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class HouseSellRequestMessage : Message
    {
        public new const uint ID =5697;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int amount;
        
        public HouseSellRequestMessage()
        {
        }
        
        public HouseSellRequestMessage(int amount)
        {
            this.amount = amount;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(amount);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            amount = reader.ReadVarInt();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
        }
        
    }
    
}