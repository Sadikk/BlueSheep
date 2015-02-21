









// Generated on 12/11/2014 19:01:20
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AllianceInvitedMessage : Message
    {
        public new const uint ID =6397;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int recruterId;
        public string recruterName;
        public Types.BasicNamedAllianceInformations allianceInfo;
        
        public AllianceInvitedMessage()
        {
        }
        
        public AllianceInvitedMessage(int recruterId, string recruterName, Types.BasicNamedAllianceInformations allianceInfo)
        {
            this.recruterId = recruterId;
            this.recruterName = recruterName;
            this.allianceInfo = allianceInfo;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(recruterId);
            writer.WriteUTF(recruterName);
            allianceInfo.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            recruterId = reader.ReadVarInt();
            if (recruterId < 0)
                throw new Exception("Forbidden value on recruterId = " + recruterId + ", it doesn't respect the following condition : recruterId < 0");
            recruterName = reader.ReadUTF();
            allianceInfo = new Types.BasicNamedAllianceInformations();
            allianceInfo.Deserialize(reader);
        }
        
    }
    
}