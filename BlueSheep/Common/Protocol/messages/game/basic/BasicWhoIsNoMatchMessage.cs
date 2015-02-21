









// Generated on 12/11/2014 19:01:22
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class BasicWhoIsNoMatchMessage : Message
    {
        public new const uint ID =179;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string search;
        
        public BasicWhoIsNoMatchMessage()
        {
        }
        
        public BasicWhoIsNoMatchMessage(string search)
        {
            this.search = search;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(search);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            search = reader.ReadUTF();
        }
        
    }
    
}