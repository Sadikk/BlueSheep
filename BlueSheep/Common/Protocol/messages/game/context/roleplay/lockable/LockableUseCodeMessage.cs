









// Generated on 12/11/2014 19:01:35
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class LockableUseCodeMessage : Message
    {
        public new const uint ID =5667;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string code;
        
        public LockableUseCodeMessage()
        {
        }
        
        public LockableUseCodeMessage(string code)
        {
            this.code = code;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(code);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            code = reader.ReadUTF();
        }
        
    }
    
}