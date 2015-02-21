









// Generated on 12/11/2014 19:01:20
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AllianceFactsRequestMessage : Message
    {
        public new const uint ID =6409;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int allianceId;
        
        public AllianceFactsRequestMessage()
        {
        }
        
        public AllianceFactsRequestMessage(int allianceId)
        {
            this.allianceId = allianceId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(allianceId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            allianceId = reader.ReadVarInt();
            if (allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + allianceId + ", it doesn't respect the following condition : allianceId < 0");
        }
        
    }
    
}