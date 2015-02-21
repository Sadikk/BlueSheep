









// Generated on 12/11/2014 19:01:45
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildSpellUpgradeRequestMessage : Message
    {
        public new const uint ID =5699;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int spellId;
        
        public GuildSpellUpgradeRequestMessage()
        {
        }
        
        public GuildSpellUpgradeRequestMessage(int spellId)
        {
            this.spellId = spellId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(spellId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            spellId = reader.ReadInt();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
        }
        
    }
    
}