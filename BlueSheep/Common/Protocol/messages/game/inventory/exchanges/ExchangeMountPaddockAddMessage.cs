









// Generated on 12/11/2014 19:01:49
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeMountPaddockAddMessage : Message
    {
        public new const uint ID =6049;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.MountClientData mountDescription;
        
        public ExchangeMountPaddockAddMessage()
        {
        }
        
        public ExchangeMountPaddockAddMessage(Types.MountClientData mountDescription)
        {
            this.mountDescription = mountDescription;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            mountDescription.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            mountDescription = new Types.MountClientData();
            mountDescription.Deserialize(reader);
        }
        
    }
    
}