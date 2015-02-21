









// Generated on 12/11/2014 19:01:48
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeBidHouseSearchMessage : Message
    {
        public new const uint ID =5806;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int type;
        public short genId;
        
        public ExchangeBidHouseSearchMessage()
        {
        }
        
        public ExchangeBidHouseSearchMessage(int type, short genId)
        {
            this.type = type;
            this.genId = genId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(type);
            writer.WriteVarShort(genId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            type = reader.ReadVarInt();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            genId = reader.ReadVarShort();
            if (genId < 0)
                throw new Exception("Forbidden value on genId = " + genId + ", it doesn't respect the following condition : genId < 0");
        }
        
    }
    
}