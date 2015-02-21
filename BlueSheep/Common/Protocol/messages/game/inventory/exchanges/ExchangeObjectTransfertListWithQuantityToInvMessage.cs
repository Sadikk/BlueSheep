









// Generated on 12/11/2014 19:01:51
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeObjectTransfertListWithQuantityToInvMessage : Message
    {
        public new const uint ID =6470;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int[] ids;
        public int[] qtys;
        
        public ExchangeObjectTransfertListWithQuantityToInvMessage()
        {
        }
        
        public ExchangeObjectTransfertListWithQuantityToInvMessage(int[] ids, int[] qtys)
        {
            this.ids = ids;
            this.qtys = qtys;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)ids.Length);
            foreach (var entry in ids)
            {
                 writer.WriteVarInt(entry);
            }
            writer.WriteUShort((ushort)qtys.Length);
            foreach (var entry in qtys)
            {
                 writer.WriteVarInt(entry);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            ids = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 ids[i] = reader.ReadVarInt();
            }
            limit = reader.ReadUShort();
            qtys = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 qtys[i] = reader.ReadVarInt();
            }
        }
        
    }
    
}