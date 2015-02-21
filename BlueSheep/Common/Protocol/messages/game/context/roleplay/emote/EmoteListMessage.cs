









// Generated on 12/11/2014 19:01:32
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class EmoteListMessage : Message
    {
        public new const uint ID =5689;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public byte[] emoteIds;
        
        public EmoteListMessage()
        {
        }
        
        public EmoteListMessage(byte[] emoteIds)
        {
            this.emoteIds = emoteIds;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)emoteIds.Length);
            foreach (var entry in emoteIds)
            {
                 writer.WriteByte(entry);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            emoteIds = new byte[limit];
            for (int i = 0; i < limit; i++)
            {
                 emoteIds[i] = reader.ReadByte();
            }
        }
        
    }
    
}