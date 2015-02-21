









// Generated on 12/11/2014 19:01:15
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class DebugInClientMessage : Message
    {
        public new const uint ID =6028;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte level;
        public string message;
        
        public DebugInClientMessage()
        {
        }
        
        public DebugInClientMessage(sbyte level, string message)
        {
            this.level = level;
            this.message = message;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(level);
            writer.WriteUTF(message);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            level = reader.ReadSByte();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            message = reader.ReadUTF();
        }
        
    }
    
}