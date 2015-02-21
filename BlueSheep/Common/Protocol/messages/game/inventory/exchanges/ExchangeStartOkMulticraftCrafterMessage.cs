









// Generated on 12/11/2014 19:01:53
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeStartOkMulticraftCrafterMessage : Message
    {
        public new const uint ID =5818;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte maxCase;
        public int skillId;
        
        public ExchangeStartOkMulticraftCrafterMessage()
        {
        }
        
        public ExchangeStartOkMulticraftCrafterMessage(sbyte maxCase, int skillId)
        {
            this.maxCase = maxCase;
            this.skillId = skillId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(maxCase);
            writer.WriteVarInt(skillId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            maxCase = reader.ReadSByte();
            if (maxCase < 0)
                throw new Exception("Forbidden value on maxCase = " + maxCase + ", it doesn't respect the following condition : maxCase < 0");
            skillId = reader.ReadVarInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
        }
        
    }
    
}