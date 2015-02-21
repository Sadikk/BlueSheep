









// Generated on 12/11/2014 19:01:22
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class BasicWhoAmIRequestMessage : Message
    {
        public new const uint ID =5664;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool verbose;
        
        public BasicWhoAmIRequestMessage()
        {
        }
        
        public BasicWhoAmIRequestMessage(bool verbose)
        {
            this.verbose = verbose;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(verbose);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            verbose = reader.ReadBoolean();
        }
        
    }
    
}