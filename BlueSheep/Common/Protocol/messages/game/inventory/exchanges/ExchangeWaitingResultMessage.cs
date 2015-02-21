









// Generated on 12/11/2014 19:01:53
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeWaitingResultMessage : Message
    {
        public new const uint ID =5786;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool bwait;
        
        public ExchangeWaitingResultMessage()
        {
        }
        
        public ExchangeWaitingResultMessage(bool bwait)
        {
            this.bwait = bwait;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(bwait);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            bwait = reader.ReadBoolean();
        }
        
    }
    
}