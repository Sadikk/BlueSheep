









// Generated on 12/11/2014 19:01:33
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class HouseBuyRequestMessage : Message
    {
        public new const uint ID =5738;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int proposedPrice;
        
        public HouseBuyRequestMessage()
        {
        }
        
        public HouseBuyRequestMessage(int proposedPrice)
        {
            this.proposedPrice = proposedPrice;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(proposedPrice);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            proposedPrice = reader.ReadVarInt();
            if (proposedPrice < 0)
                throw new Exception("Forbidden value on proposedPrice = " + proposedPrice + ", it doesn't respect the following condition : proposedPrice < 0");
        }
        
    }
    
}