









// Generated on 12/11/2014 19:01:51
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeReplayMessage : Message
    {
        public new const uint ID =6002;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int count;
        
        public ExchangeReplayMessage()
        {
        }
        
        public ExchangeReplayMessage(int count)
        {
            this.count = count;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(count);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            count = reader.ReadVarInt();
        }
        
    }
    
}