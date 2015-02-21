









// Generated on 12/11/2014 19:01:42
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class TreasureHuntRequestMessage : Message
    {
        public new const uint ID =6488;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public byte questLevel;
        public sbyte questType;
        
        public TreasureHuntRequestMessage()
        {
        }
        
        public TreasureHuntRequestMessage(byte questLevel, sbyte questType)
        {
            this.questLevel = questLevel;
            this.questType = questType;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteByte(questLevel);
            writer.WriteSByte(questType);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            questLevel = reader.ReadByte();
            if (questLevel < 1 || questLevel > 200)
                throw new Exception("Forbidden value on questLevel = " + questLevel + ", it doesn't respect the following condition : questLevel < 1 || questLevel > 200");
            questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
        }
        
    }
    
}