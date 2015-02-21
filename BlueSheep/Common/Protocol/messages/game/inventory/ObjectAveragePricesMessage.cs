









// Generated on 12/11/2014 19:01:47
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ObjectAveragePricesMessage : Message
    {
        public new const uint ID =6335;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short[] ids;
        public int[] avgPrices;
        
        public ObjectAveragePricesMessage()
        {
        }
        
        public ObjectAveragePricesMessage(short[] ids, int[] avgPrices)
        {
            this.ids = ids;
            this.avgPrices = avgPrices;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)ids.Length);
            foreach (var entry in ids)
            {
                 writer.WriteVarShort(entry);
            }
            writer.WriteUShort((ushort)avgPrices.Length);
            foreach (var entry in avgPrices)
            {
                 writer.WriteVarInt(entry);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            ids = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 ids[i] = reader.ReadVarShort();
            }
            limit = reader.ReadUShort();
            avgPrices = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 avgPrices[i] = reader.ReadVarInt();
            }
        }
        
    }
    
}