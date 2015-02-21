









// Generated on 12/11/2014 19:01:22
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class CurrentServerStatusUpdateMessage : Message
    {
        public new const uint ID =6525;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte status;
        
        public CurrentServerStatusUpdateMessage()
        {
        }
        
        public CurrentServerStatusUpdateMessage(sbyte status)
        {
            this.status = status;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(status);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            status = reader.ReadSByte();
            if (status < 0)
                throw new Exception("Forbidden value on status = " + status + ", it doesn't respect the following condition : status < 0");
        }
        
    }
    
}