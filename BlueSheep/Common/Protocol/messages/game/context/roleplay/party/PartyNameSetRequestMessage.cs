









// Generated on 12/11/2014 19:01:39
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class PartyNameSetRequestMessage : AbstractPartyMessage
    {
        public new const uint ID =6503;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string partyName;
        
        public PartyNameSetRequestMessage()
        {
        }
        
        public PartyNameSetRequestMessage(int partyId, string partyName)
         : base(partyId)
        {
            this.partyName = partyName;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(partyName);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            partyName = reader.ReadUTF();
        }
        
    }
    
}