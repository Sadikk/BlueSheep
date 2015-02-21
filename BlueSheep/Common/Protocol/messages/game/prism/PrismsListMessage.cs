









// Generated on 12/11/2014 19:01:58
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class PrismsListMessage : Message
    {
        public new const uint ID =6440;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.PrismSubareaEmptyInfo[] prisms;
        
        public PrismsListMessage()
        {
        }
        
        public PrismsListMessage(Types.PrismSubareaEmptyInfo[] prisms)
        {
            this.prisms = prisms;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)prisms.Length);
            foreach (var entry in prisms)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            prisms = new Types.PrismSubareaEmptyInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 prisms[i] = Types.ProtocolTypeManager.GetInstance<Types.PrismSubareaEmptyInfo>(reader.ReadShort());
                 prisms[i].Deserialize(reader);
            }
        }
        
    }
    
}