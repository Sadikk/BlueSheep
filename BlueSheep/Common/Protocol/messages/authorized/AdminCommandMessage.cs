









// Generated on 12/11/2014 19:01:13
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AdminCommandMessage : Message
    {
        public new const uint ID =76;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string content;
        
        public AdminCommandMessage()
        {
        }
        
        public AdminCommandMessage(string content)
        {
            this.content = content;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(content);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            content = reader.ReadUTF();
        }
        
    }
    
}