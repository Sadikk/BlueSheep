









// Generated on 12/11/2014 19:01:13
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class BasicPongMessage : Message
    {
        public new const uint ID =183;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool quiet;
        
        public BasicPongMessage()
        {
        }
        
        public BasicPongMessage(bool quiet)
        {
            this.quiet = quiet;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(quiet);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            quiet = reader.ReadBoolean();
        }
        
    }
    
}