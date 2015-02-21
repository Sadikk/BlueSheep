









// Generated on 12/11/2014 19:01:46
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class TaxCollectorMovementRemoveMessage : Message
    {
        public new const uint ID =5915;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int collectorId;
        
        public TaxCollectorMovementRemoveMessage()
        {
        }
        
        public TaxCollectorMovementRemoveMessage(int collectorId)
        {
            this.collectorId = collectorId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(collectorId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            collectorId = reader.ReadInt();
        }
        
    }
    
}