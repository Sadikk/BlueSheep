









// Generated on 12/11/2014 19:01:54
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class InventoryContentMessage : Message
    {
        public new const uint ID =3016;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.ObjectItem[] objects;
        public int kamas;
        
        public InventoryContentMessage()
        {
        }
        
        public InventoryContentMessage(Types.ObjectItem[] objects, int kamas)
        {
            this.objects = objects;
            this.kamas = kamas;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)objects.Length);
            foreach (var entry in objects)
            {
                 entry.Serialize(writer);
            }
            writer.WriteVarInt(kamas);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            objects = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objects[i] = new Types.ObjectItem();
                 objects[i].Deserialize(reader);
            }
            kamas = reader.ReadVarInt();
            if (kamas < 0)
                throw new Exception("Forbidden value on kamas = " + kamas + ", it doesn't respect the following condition : kamas < 0");
        }
        
    }
    
}