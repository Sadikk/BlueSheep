









// Generated on 12/11/2014 19:01:53
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeMultiCraftCrafterCanUseHisRessourcesMessage : Message
    {
        public new const uint ID =6020;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool allowed;
        
        public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage()
        {
        }
        
        public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(bool allowed)
        {
            this.allowed = allowed;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(allowed);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            allowed = reader.ReadBoolean();
        }
        
    }
    
}