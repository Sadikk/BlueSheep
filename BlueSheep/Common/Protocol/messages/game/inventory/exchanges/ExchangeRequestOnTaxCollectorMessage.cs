









// Generated on 12/11/2014 19:01:51
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeRequestOnTaxCollectorMessage : Message
    {
        public new const uint ID =5779;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int taxCollectorId;
        
        public ExchangeRequestOnTaxCollectorMessage()
        {
        }
        
        public ExchangeRequestOnTaxCollectorMessage(int taxCollectorId)
        {
            this.taxCollectorId = taxCollectorId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(taxCollectorId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            taxCollectorId = reader.ReadInt();
        }
        
    }
    
}