









// Generated on 12/11/2014 19:02:01
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class DownloadPartMessage : Message
    {
        public new const uint ID =1503;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string id;
        
        public DownloadPartMessage()
        {
        }
        
        public DownloadPartMessage(string id)
        {
            this.id = id;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(id);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadUTF();
        }
        
    }
    
}