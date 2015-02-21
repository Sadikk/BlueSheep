









// Generated on 12/11/2014 19:01:41
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class SpellForgetUIMessage : Message
    {
        public new const uint ID =5565;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool open;
        
        public SpellForgetUIMessage()
        {
        }
        
        public SpellForgetUIMessage(bool open)
        {
            this.open = open;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(open);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            open = reader.ReadBoolean();
        }
        
    }
    
}