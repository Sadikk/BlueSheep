









// Generated on 12/11/2014 19:02:01
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class KrosmasterTransferRequestMessage : Message
    {
        public new const uint ID =6349;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string uid;
        
        public KrosmasterTransferRequestMessage()
        {
        }
        
        public KrosmasterTransferRequestMessage(string uid)
        {
            this.uid = uid;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(uid);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            uid = reader.ReadUTF();
        }
        
    }
    
}