









// Generated on 12/11/2014 19:01:36
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AbstractPartyMessage : Message
    {
        public new const uint ID =6274;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int partyId;
        
        public AbstractPartyMessage()
        {
        }
        
        public AbstractPartyMessage(int partyId)
        {
            this.partyId = partyId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(partyId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            partyId = reader.ReadVarInt();
            if (partyId < 0)
                throw new Exception("Forbidden value on partyId = " + partyId + ", it doesn't respect the following condition : partyId < 0");
        }
        
    }
    
}