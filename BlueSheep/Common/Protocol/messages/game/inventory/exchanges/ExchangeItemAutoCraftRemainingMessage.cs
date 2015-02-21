









// Generated on 12/11/2014 19:01:49
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeItemAutoCraftRemainingMessage : Message
    {
        public new const uint ID =6015;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int count;
        
        public ExchangeItemAutoCraftRemainingMessage()
        {
        }
        
        public ExchangeItemAutoCraftRemainingMessage(int count)
        {
            this.count = count;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(count);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            count = reader.ReadVarInt();
            if (count < 0)
                throw new Exception("Forbidden value on count = " + count + ", it doesn't respect the following condition : count < 0");
        }
        
    }
    
}