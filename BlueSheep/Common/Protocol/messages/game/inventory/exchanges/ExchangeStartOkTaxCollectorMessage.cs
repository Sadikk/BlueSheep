









// Generated on 12/11/2014 19:01:53
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeStartOkTaxCollectorMessage : Message
    {
        public new const uint ID =5780;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int collectorId;
        public Types.ObjectItem[] objectsInfos;
        public int goldInfo;
        
        public ExchangeStartOkTaxCollectorMessage()
        {
        }
        
        public ExchangeStartOkTaxCollectorMessage(int collectorId, Types.ObjectItem[] objectsInfos, int goldInfo)
        {
            this.collectorId = collectorId;
            this.objectsInfos = objectsInfos;
            this.goldInfo = goldInfo;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(collectorId);
            writer.WriteUShort((ushort)objectsInfos.Length);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
            writer.WriteVarInt(goldInfo);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            collectorId = reader.ReadInt();
            var limit = reader.ReadUShort();
            objectsInfos = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos[i] = new Types.ObjectItem();
                 objectsInfos[i].Deserialize(reader);
            }
            goldInfo = reader.ReadVarInt();
            if (goldInfo < 0)
                throw new Exception("Forbidden value on goldInfo = " + goldInfo + ", it doesn't respect the following condition : goldInfo < 0");
        }
        
    }
    
}