









// Generated on 12/11/2014 19:01:41
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class TreasureHuntAvailableRetryCountUpdateMessage : Message
    {
        public new const uint ID =6491;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte questType;
        public int availableRetryCount;
        
        public TreasureHuntAvailableRetryCountUpdateMessage()
        {
        }
        
        public TreasureHuntAvailableRetryCountUpdateMessage(sbyte questType, int availableRetryCount)
        {
            this.questType = questType;
            this.availableRetryCount = availableRetryCount;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(questType);
            writer.WriteInt(availableRetryCount);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
            availableRetryCount = reader.ReadInt();
        }
        
    }
    
}