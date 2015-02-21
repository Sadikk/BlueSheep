









// Generated on 12/11/2014 19:01:56
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class SymbioticObjectAssociateRequestMessage : Message
    {
        public new const uint ID =6522;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int symbioteUID;
        public byte symbiotePos;
        public int hostUID;
        public byte hostPos;
        
        public SymbioticObjectAssociateRequestMessage()
        {
        }
        
        public SymbioticObjectAssociateRequestMessage(int symbioteUID, byte symbiotePos, int hostUID, byte hostPos)
        {
            this.symbioteUID = symbioteUID;
            this.symbiotePos = symbiotePos;
            this.hostUID = hostUID;
            this.hostPos = hostPos;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(symbioteUID);
            writer.WriteByte(symbiotePos);
            writer.WriteVarInt(hostUID);
            writer.WriteByte(hostPos);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            symbioteUID = reader.ReadVarInt();
            if (symbioteUID < 0)
                throw new Exception("Forbidden value on symbioteUID = " + symbioteUID + ", it doesn't respect the following condition : symbioteUID < 0");
            symbiotePos = reader.ReadByte();
            if (symbiotePos < 0 || symbiotePos > 255)
                throw new Exception("Forbidden value on symbiotePos = " + symbiotePos + ", it doesn't respect the following condition : symbiotePos < 0 || symbiotePos > 255");
            hostUID = reader.ReadVarInt();
            if (hostUID < 0)
                throw new Exception("Forbidden value on hostUID = " + hostUID + ", it doesn't respect the following condition : hostUID < 0");
            hostPos = reader.ReadByte();
            if (hostPos < 0 || hostPos > 255)
                throw new Exception("Forbidden value on hostPos = " + hostPos + ", it doesn't respect the following condition : hostPos < 0 || hostPos > 255");
        }
        
    }
    
}