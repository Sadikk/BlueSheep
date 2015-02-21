









// Generated on 12/11/2014 19:01:20
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AllianceKickRequestMessage : Message
    {
        public new const uint ID =6400;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int kickedId;
        
        public AllianceKickRequestMessage()
        {
        }
        
        public AllianceKickRequestMessage(int kickedId)
        {
            this.kickedId = kickedId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(kickedId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            kickedId = reader.ReadVarInt();
            if (kickedId < 0)
                throw new Exception("Forbidden value on kickedId = " + kickedId + ", it doesn't respect the following condition : kickedId < 0");
        }
        
    }
    
}