









// Generated on 12/11/2014 19:01:51
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeReadyMessage : Message
    {
        public new const uint ID =5511;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool ready;
        public short step;
        
        public ExchangeReadyMessage()
        {
        }
        
        public ExchangeReadyMessage(bool ready, short step)
        {
            this.ready = ready;
            this.step = step;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(ready);
            writer.WriteVarShort(step);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            ready = reader.ReadBoolean();
            step = reader.ReadVarShort();
            if (step < 0)
                throw new Exception("Forbidden value on step = " + step + ", it doesn't respect the following condition : step < 0");
        }
        
    }
    
}