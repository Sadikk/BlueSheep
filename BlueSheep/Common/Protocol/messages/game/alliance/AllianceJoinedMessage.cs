









// Generated on 12/11/2014 19:01:20
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AllianceJoinedMessage : Message
    {
        public new const uint ID =6402;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.AllianceInformations allianceInfo;
        public bool enabled;
        
        public AllianceJoinedMessage()
        {
        }
        
        public AllianceJoinedMessage(Types.AllianceInformations allianceInfo, bool enabled)
        {
            this.allianceInfo = allianceInfo;
            this.enabled = enabled;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            allianceInfo.Serialize(writer);
            writer.WriteBoolean(enabled);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            allianceInfo = new Types.AllianceInformations();
            allianceInfo.Deserialize(reader);
            enabled = reader.ReadBoolean();
        }
        
    }
    
}