









// Generated on 12/11/2014 19:01:56
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class SymbioticObjectAssociatedMessage : Message
    {
        public new const uint ID =6527;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int hostUID;
        
        public SymbioticObjectAssociatedMessage()
        {
        }
        
        public SymbioticObjectAssociatedMessage(int hostUID)
        {
            this.hostUID = hostUID;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(hostUID);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            hostUID = reader.ReadVarInt();
            if (hostUID < 0)
                throw new Exception("Forbidden value on hostUID = " + hostUID + ", it doesn't respect the following condition : hostUID < 0");
        }
        
    }
    
}