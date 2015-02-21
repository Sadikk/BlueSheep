









// Generated on 12/11/2014 19:01:46
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildFightLeaveRequestMessage : Message
    {
        public new const uint ID =5715;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int taxCollectorId;
        public int characterId;
        
        public GuildFightLeaveRequestMessage()
        {
        }
        
        public GuildFightLeaveRequestMessage(int taxCollectorId, int characterId)
        {
            this.taxCollectorId = taxCollectorId;
            this.characterId = characterId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(taxCollectorId);
            writer.WriteVarInt(characterId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            taxCollectorId = reader.ReadInt();
            if (taxCollectorId < 0)
                throw new Exception("Forbidden value on taxCollectorId = " + taxCollectorId + ", it doesn't respect the following condition : taxCollectorId < 0");
            characterId = reader.ReadVarInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
        }
        
    }
    
}