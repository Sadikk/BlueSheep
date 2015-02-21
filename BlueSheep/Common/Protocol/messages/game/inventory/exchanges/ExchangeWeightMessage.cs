









// Generated on 12/11/2014 19:01:53
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeWeightMessage : Message
    {
        public new const uint ID =5793;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int currentWeight;
        public int maxWeight;
        
        public ExchangeWeightMessage()
        {
        }
        
        public ExchangeWeightMessage(int currentWeight, int maxWeight)
        {
            this.currentWeight = currentWeight;
            this.maxWeight = maxWeight;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(currentWeight);
            writer.WriteVarInt(maxWeight);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            currentWeight = reader.ReadVarInt();
            if (currentWeight < 0)
                throw new Exception("Forbidden value on currentWeight = " + currentWeight + ", it doesn't respect the following condition : currentWeight < 0");
            maxWeight = reader.ReadVarInt();
            if (maxWeight < 0)
                throw new Exception("Forbidden value on maxWeight = " + maxWeight + ", it doesn't respect the following condition : maxWeight < 0");
        }
        
    }
    
}