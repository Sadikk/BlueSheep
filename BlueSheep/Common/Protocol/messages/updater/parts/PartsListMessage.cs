









// Generated on 12/11/2014 19:02:01
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class PartsListMessage : Message
    {
        public new const uint ID =1502;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.ContentPart[] parts;
        
        public PartsListMessage()
        {
        }
        
        public PartsListMessage(Types.ContentPart[] parts)
        {
            this.parts = parts;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)parts.Length);
            foreach (var entry in parts)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            parts = new Types.ContentPart[limit];
            for (int i = 0; i < limit; i++)
            {
                 parts[i] = new Types.ContentPart();
                 parts[i].Deserialize(reader);
            }
        }
        
    }
    
}