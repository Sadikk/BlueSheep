









// Generated on 12/11/2014 19:02:00
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class TrustStatusMessage : Message
    {
        public new const uint ID =6267;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool trusted;
        
        public TrustStatusMessage()
        {
        }
        
        public TrustStatusMessage(bool trusted)
        {
            this.trusted = trusted;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(trusted);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            trusted = reader.ReadBoolean();
        }
        
    }
    
}