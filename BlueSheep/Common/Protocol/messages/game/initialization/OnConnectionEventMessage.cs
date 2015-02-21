









// Generated on 12/11/2014 19:01:46
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class OnConnectionEventMessage : Message
    {
        public new const uint ID =5726;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte eventType;
        
        public OnConnectionEventMessage()
        {
        }
        
        public OnConnectionEventMessage(sbyte eventType)
        {
            this.eventType = eventType;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(eventType);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            eventType = reader.ReadSByte();
            if (eventType < 0)
                throw new Exception("Forbidden value on eventType = " + eventType + ", it doesn't respect the following condition : eventType < 0");
        }
        
    }
    
}