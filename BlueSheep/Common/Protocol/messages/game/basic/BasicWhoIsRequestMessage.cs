









// Generated on 12/11/2014 19:01:22
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class BasicWhoIsRequestMessage : Message
    {
        public new const uint ID =181;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool verbose;
        public string search;
        
        public BasicWhoIsRequestMessage()
        {
        }
        
        public BasicWhoIsRequestMessage(bool verbose, string search)
        {
            this.verbose = verbose;
            this.search = search;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(verbose);
            writer.WriteUTF(search);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            verbose = reader.ReadBoolean();
            search = reader.ReadUTF();
        }
        
    }
    
}