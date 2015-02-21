









// Generated on 12/11/2014 19:01:59
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class SubscriptionZoneMessage : Message
    {
        public new const uint ID =5573;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool active;
        
        public SubscriptionZoneMessage()
        {
        }
        
        public SubscriptionZoneMessage(bool active)
        {
            this.active = active;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(active);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            active = reader.ReadBoolean();
        }
        
    }
    
}