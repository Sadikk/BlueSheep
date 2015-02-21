









// Generated on 12/11/2014 19:01:48
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeBidHouseTypeMessage : Message
    {
        public new const uint ID =5803;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int type;
        
        public ExchangeBidHouseTypeMessage()
        {
        }
        
        public ExchangeBidHouseTypeMessage(int type)
        {
            this.type = type;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(type);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            type = reader.ReadVarInt();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
    }
    
}