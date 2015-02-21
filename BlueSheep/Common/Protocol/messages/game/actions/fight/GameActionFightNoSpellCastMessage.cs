









// Generated on 12/11/2014 19:01:18
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameActionFightNoSpellCastMessage : Message
    {
        public new const uint ID =6132;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int spellLevelId;
        
        public GameActionFightNoSpellCastMessage()
        {
        }
        
        public GameActionFightNoSpellCastMessage(int spellLevelId)
        {
            this.spellLevelId = spellLevelId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(spellLevelId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            spellLevelId = reader.ReadVarInt();
            if (spellLevelId < 0)
                throw new Exception("Forbidden value on spellLevelId = " + spellLevelId + ", it doesn't respect the following condition : spellLevelId < 0");
        }
        
    }
    
}