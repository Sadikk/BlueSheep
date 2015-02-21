









// Generated on 12/11/2014 19:01:21
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class CompassResetMessage : Message
    {
        public new const uint ID =5584;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte type;
        
        public CompassResetMessage()
        {
        }
        
        public CompassResetMessage(sbyte type)
        {
            this.type = type;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(type);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
    }
    
}