









// Generated on 12/11/2014 19:01:54
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class InventoryWeightMessage : Message
    {
        public new const uint ID =3009;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int weight;
        public int weightMax;
        
        public InventoryWeightMessage()
        {
        }
        
        public InventoryWeightMessage(int weight, int weightMax)
        {
            this.weight = weight;
            this.weightMax = weightMax;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(weight);
            writer.WriteVarInt(weightMax);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            weight = reader.ReadVarInt();
            if (weight < 0)
                throw new Exception("Forbidden value on weight = " + weight + ", it doesn't respect the following condition : weight < 0");
            weightMax = reader.ReadVarInt();
            if (weightMax < 0)
                throw new Exception("Forbidden value on weightMax = " + weightMax + ", it doesn't respect the following condition : weightMax < 0");
        }
        
    }
    
}