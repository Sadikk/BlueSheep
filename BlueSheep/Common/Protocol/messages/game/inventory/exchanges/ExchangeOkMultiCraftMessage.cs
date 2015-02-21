









// Generated on 12/11/2014 19:01:51
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeOkMultiCraftMessage : Message
    {
        public new const uint ID =5768;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int initiatorId;
        public int otherId;
        public sbyte role;
        
        public ExchangeOkMultiCraftMessage()
        {
        }
        
        public ExchangeOkMultiCraftMessage(int initiatorId, int otherId, sbyte role)
        {
            this.initiatorId = initiatorId;
            this.otherId = otherId;
            this.role = role;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(initiatorId);
            writer.WriteVarInt(otherId);
            writer.WriteSByte(role);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            initiatorId = reader.ReadVarInt();
            if (initiatorId < 0)
                throw new Exception("Forbidden value on initiatorId = " + initiatorId + ", it doesn't respect the following condition : initiatorId < 0");
            otherId = reader.ReadVarInt();
            if (otherId < 0)
                throw new Exception("Forbidden value on otherId = " + otherId + ", it doesn't respect the following condition : otherId < 0");
            role = reader.ReadSByte();
        }
        
    }
    
}