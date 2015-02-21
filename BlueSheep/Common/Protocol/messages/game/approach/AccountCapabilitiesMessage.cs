









// Generated on 12/11/2014 19:01:21
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AccountCapabilitiesMessage : Message
    {
        public new const uint ID =6216;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int accountId;
        public bool tutorialAvailable;
        public ushort breedsVisible;
        public ushort breedsAvailable;
        public sbyte status;
        
        public AccountCapabilitiesMessage()
        {
        }
        
        public AccountCapabilitiesMessage(int accountId, bool tutorialAvailable, ushort breedsVisible, ushort breedsAvailable, sbyte status)
        {
            this.accountId = accountId;
            this.tutorialAvailable = tutorialAvailable;
            this.breedsVisible = breedsVisible;
            this.breedsAvailable = breedsAvailable;
            this.status = status;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(accountId);
            writer.WriteBoolean(tutorialAvailable);
            writer.WriteUShort(breedsVisible);
            writer.WriteUShort(breedsAvailable);
            writer.WriteSByte(status);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            tutorialAvailable = reader.ReadBoolean();
            breedsVisible = reader.ReadUShort();
            if (breedsVisible < 0 || breedsVisible > 65535)
                throw new Exception("Forbidden value on breedsVisible = " + breedsVisible + ", it doesn't respect the following condition : breedsVisible < 0 || breedsVisible > 65535");
            breedsAvailable = reader.ReadUShort();
            if (breedsAvailable < 0 || breedsAvailable > 65535)
                throw new Exception("Forbidden value on breedsAvailable = " + breedsAvailable + ", it doesn't respect the following condition : breedsAvailable < 0 || breedsAvailable > 65535");
            status = reader.ReadSByte();
        }
        
    }
    
}