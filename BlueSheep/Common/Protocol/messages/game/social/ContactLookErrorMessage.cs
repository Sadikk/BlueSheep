









// Generated on 12/11/2014 19:01:59
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ContactLookErrorMessage : Message
    {
        public new const uint ID =6045;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int requestId;
        
        public ContactLookErrorMessage()
        {
        }
        
        public ContactLookErrorMessage(int requestId)
        {
            this.requestId = requestId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(requestId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            requestId = reader.ReadVarInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
        }
        
    }
    
}