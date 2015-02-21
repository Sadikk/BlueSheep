









// Generated on 12/11/2014 19:01:51
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeOnHumanVendorRequestMessage : Message
    {
        public new const uint ID =5772;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int humanVendorId;
        public short humanVendorCell;
        
        public ExchangeOnHumanVendorRequestMessage()
        {
        }
        
        public ExchangeOnHumanVendorRequestMessage(int humanVendorId, short humanVendorCell)
        {
            this.humanVendorId = humanVendorId;
            this.humanVendorCell = humanVendorCell;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(humanVendorId);
            writer.WriteVarShort(humanVendorCell);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            humanVendorId = reader.ReadVarInt();
            if (humanVendorId < 0)
                throw new Exception("Forbidden value on humanVendorId = " + humanVendorId + ", it doesn't respect the following condition : humanVendorId < 0");
            humanVendorCell = reader.ReadVarShort();
            if (humanVendorCell < 0 || humanVendorCell > 559)
                throw new Exception("Forbidden value on humanVendorCell = " + humanVendorCell + ", it doesn't respect the following condition : humanVendorCell < 0 || humanVendorCell > 559");
        }
        
    }
    
}