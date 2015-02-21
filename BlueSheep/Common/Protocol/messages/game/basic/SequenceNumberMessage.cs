









// Generated on 12/11/2014 19:01:22
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class SequenceNumberMessage : Message
    {
        public new const uint ID =6317;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public ushort number;
        
        public SequenceNumberMessage()
        {
        }
        
        public SequenceNumberMessage(ushort number)
        {
            this.number = number;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort(number);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            number = reader.ReadUShort();
            if (number < 0 || number > 65535)
                throw new Exception("Forbidden value on number = " + number + ", it doesn't respect the following condition : number < 0 || number > 65535");
        }
        
    }
    
}