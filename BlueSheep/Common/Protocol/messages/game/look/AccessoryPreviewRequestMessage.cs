









// Generated on 12/11/2014 19:01:57
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AccessoryPreviewRequestMessage : Message
    {
        public new const uint ID =6518;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short[] genericId;
        
        public AccessoryPreviewRequestMessage()
        {
        }
        
        public AccessoryPreviewRequestMessage(short[] genericId)
        {
            this.genericId = genericId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)genericId.Length);
            foreach (var entry in genericId)
            {
                 writer.WriteVarShort(entry);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            genericId = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 genericId[i] = reader.ReadVarShort();
            }
        }
        
    }
    
}