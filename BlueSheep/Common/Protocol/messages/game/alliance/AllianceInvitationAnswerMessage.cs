









// Generated on 12/11/2014 19:01:20
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AllianceInvitationAnswerMessage : Message
    {
        public new const uint ID =6401;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool accept;
        
        public AllianceInvitationAnswerMessage()
        {
        }
        
        public AllianceInvitationAnswerMessage(bool accept)
        {
            this.accept = accept;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(accept);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            accept = reader.ReadBoolean();
        }
        
    }
    
}