









// Generated on 12/11/2014 19:01:14
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class IdentificationMessage : Message
    {
        public new const uint ID =4;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool autoconnect;
        public bool useCertificate;
        public bool useLoginToken;
        public Types.VersionExtended version;
        public string lang;
        public sbyte[] credentials;
        public short serverId;
        public double sessionOptionalSalt;
        public List<short> failedAttempts;
        
        public IdentificationMessage()
        {
        }
        
        public IdentificationMessage(bool autoconnect, bool useCertificate, bool useLoginToken, Types.VersionExtended version, string lang, sbyte[] credentials, short serverId, double sessionOptionalSalt, List<short> failedAttempts)
        {
            this.autoconnect = autoconnect;
            this.useCertificate = useCertificate;
            this.useLoginToken = useLoginToken;
            this.version = version;
            this.lang = lang;
            this.credentials = credentials;
            this.serverId = serverId;
            this.sessionOptionalSalt = sessionOptionalSalt;
            this.failedAttempts = failedAttempts;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, autoconnect);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, useCertificate);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, useLoginToken);
            writer.WriteByte(flag1);
            version.Serialize(writer);
            writer.WriteUTF(lang);
            writer.WriteVarInt(credentials.Length);
            foreach (var entry in credentials)
            {
                 writer.WriteSByte(entry);
            }
            writer.WriteShort(serverId);
            writer.WriteDouble(sessionOptionalSalt);
            writer.WriteShort((short)this.failedAttempts.Count);
            foreach (short s in failedAttempts)
                writer.WriteShort(s);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            autoconnect = BooleanByteWrapper.GetFlag(flag1, 0);
            useCertificate = BooleanByteWrapper.GetFlag(flag1, 1);
            useLoginToken = BooleanByteWrapper.GetFlag(flag1, 2);
            version = new Types.VersionExtended();
            version.Deserialize(reader);
            lang = reader.ReadUTF();
            var limit = reader.ReadVarInt();
            credentials = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 credentials[i] = reader.ReadSByte();
            }
            serverId = reader.ReadShort();
            sessionOptionalSalt = reader.ReadDouble();
            if (sessionOptionalSalt < -9.007199254740992E15 || sessionOptionalSalt > 9.007199254740992E15)
                throw new Exception("Forbidden value on sessionOptionalSalt = " + sessionOptionalSalt + ", it doesn't respect the following condition : sessionOptionalSalt < -9.007199254740992E15 || sessionOptionalSalt > 9.007199254740992E15");
            ushort length = reader.ReadUShort();
            failedAttempts = new List<short>();
            for (int i = 0; i < length; i++)
            {
                failedAttempts.Add(reader.ReadVarShort());
            }

        }
        
    }
    
}