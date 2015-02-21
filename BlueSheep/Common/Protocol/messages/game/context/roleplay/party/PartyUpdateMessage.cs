









// Generated on 12/11/2014 19:01:40
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class PartyUpdateMessage : AbstractPartyEventMessage
    {
        public new const uint ID =5575;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.PartyMemberInformations memberInformations;
        
        public PartyUpdateMessage()
        {
        }
        
        public PartyUpdateMessage(int partyId, Types.PartyMemberInformations memberInformations)
         : base(partyId)
        {
            this.memberInformations = memberInformations;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(memberInformations.TypeId);
            memberInformations.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            memberInformations = Types.ProtocolTypeManager.GetInstance<Types.PartyMemberInformations>(reader.ReadShort());
            memberInformations.Deserialize(reader);
        }
        
    }
    
}