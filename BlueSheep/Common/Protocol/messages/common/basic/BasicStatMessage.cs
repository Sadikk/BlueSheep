









// Generated on 12/11/2014 19:01:13
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class BasicStatMessage : Message
    {
        public new const uint ID =6530;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short statId;
        
        public BasicStatMessage()
        {
        }
        
        public BasicStatMessage(short statId)
        {
            this.statId = statId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(statId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            statId = reader.ReadVarShort();
            if (statId < 0)
                throw new Exception("Forbidden value on statId = " + statId + ", it doesn't respect the following condition : statId < 0");
        }
        
    }
    
}