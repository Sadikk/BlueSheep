









// Generated on 12/11/2014 19:02:00
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class CheckFileRequestMessage : Message
    {
        public new const uint ID =6154;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string filename;
        public sbyte type;
        
        public CheckFileRequestMessage()
        {
        }
        
        public CheckFileRequestMessage(string filename, sbyte type)
        {
            this.filename = filename;
            this.type = type;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(filename);
            writer.WriteSByte(type);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            filename = reader.ReadUTF();
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
    }
    
}