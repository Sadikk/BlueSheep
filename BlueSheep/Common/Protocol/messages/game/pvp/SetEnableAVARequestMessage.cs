









// Generated on 12/11/2014 19:01:58
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class SetEnableAVARequestMessage : Message
    {
        public new const uint ID =6443;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool enable;
        
        public SetEnableAVARequestMessage()
        {
        }
        
        public SetEnableAVARequestMessage(bool enable)
        {
            this.enable = enable;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(enable);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            enable = reader.ReadBoolean();
        }
        
    }
    
}