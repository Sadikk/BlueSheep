









// Generated on 12/11/2014 19:01:41
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class TreasureHuntFlagRequestAnswerMessage : Message
    {
        public new const uint ID =6507;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte questType;
        public sbyte result;
        public sbyte index;
        
        public TreasureHuntFlagRequestAnswerMessage()
        {
        }
        
        public TreasureHuntFlagRequestAnswerMessage(sbyte questType, sbyte result, sbyte index)
        {
            this.questType = questType;
            this.result = result;
            this.index = index;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(questType);
            writer.WriteSByte(result);
            writer.WriteSByte(index);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
            result = reader.ReadSByte();
            if (result < 0)
                throw new Exception("Forbidden value on result = " + result + ", it doesn't respect the following condition : result < 0");
            index = reader.ReadSByte();
            if (index < 0)
                throw new Exception("Forbidden value on index = " + index + ", it doesn't respect the following condition : index < 0");
        }
        
    }
    
}