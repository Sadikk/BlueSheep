









// Generated on 12/11/2014 19:01:40
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class QuestStartedMessage : Message
    {
        public new const uint ID =6091;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short questId;
        
        public QuestStartedMessage()
        {
        }
        
        public QuestStartedMessage(short questId)
        {
            this.questId = questId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(questId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            questId = reader.ReadVarShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
        }
        
    }
    
}