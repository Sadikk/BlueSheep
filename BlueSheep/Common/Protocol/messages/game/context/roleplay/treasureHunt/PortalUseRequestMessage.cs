









// Generated on 12/11/2014 19:01:41
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class PortalUseRequestMessage : Message
    {
        public new const uint ID =6492;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int portalId;
        
        public PortalUseRequestMessage()
        {
        }
        
        public PortalUseRequestMessage(int portalId)
        {
            this.portalId = portalId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(portalId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            portalId = reader.ReadVarInt();
            if (portalId < 0)
                throw new Exception("Forbidden value on portalId = " + portalId + ", it doesn't respect the following condition : portalId < 0");
        }
        
    }
    
}