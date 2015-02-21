









// Generated on 12/11/2014 19:01:48
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeBidHousePriceMessage : Message
    {
        public new const uint ID =5805;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short genId;
        
        public ExchangeBidHousePriceMessage()
        {
        }
        
        public ExchangeBidHousePriceMessage(short genId)
        {
            this.genId = genId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(genId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            genId = reader.ReadVarShort();
            if (genId < 0)
                throw new Exception("Forbidden value on genId = " + genId + ", it doesn't respect the following condition : genId < 0");
        }
        
    }
    
}