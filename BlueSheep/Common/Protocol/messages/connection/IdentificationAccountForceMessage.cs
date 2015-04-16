









// Generated on 12/11/2014 19:01:13
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class IdentificationAccountForceMessage : IdentificationMessage
    {
        public new const uint ID =6119;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string forcedAccountLogin;
        
        public IdentificationAccountForceMessage()
        {
        }
        
        public IdentificationAccountForceMessage(bool autoconnect, bool useCertificate, bool useLoginToken, Types.VersionExtended version, string lang, sbyte[] credentials, short serverId, double sessionOptionalSalt, string forcedAccountLogin, List<short> failedAttempts)
         : base(autoconnect, useCertificate, useLoginToken, version, lang, credentials, serverId, sessionOptionalSalt, failedAttempts)
        {
            this.forcedAccountLogin = forcedAccountLogin;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(forcedAccountLogin);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            forcedAccountLogin = reader.ReadUTF();
        }
        
    }
    
}