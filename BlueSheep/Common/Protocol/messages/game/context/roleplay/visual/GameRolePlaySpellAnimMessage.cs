









// Generated on 12/11/2014 19:01:42
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameRolePlaySpellAnimMessage : Message
    {
        public new const uint ID =6114;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int casterId;
        public short targetCellId;
        public short spellId;
        public sbyte spellLevel;
        
        public GameRolePlaySpellAnimMessage()
        {
        }
        
        public GameRolePlaySpellAnimMessage(int casterId, short targetCellId, short spellId, sbyte spellLevel)
        {
            this.casterId = casterId;
            this.targetCellId = targetCellId;
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(casterId);
            writer.WriteVarShort(targetCellId);
            writer.WriteVarShort(spellId);
            writer.WriteSByte(spellLevel);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            casterId = reader.ReadInt();
            targetCellId = reader.ReadVarShort();
            if (targetCellId < 0 || targetCellId > 559)
                throw new Exception("Forbidden value on targetCellId = " + targetCellId + ", it doesn't respect the following condition : targetCellId < 0 || targetCellId > 559");
            spellId = reader.ReadVarShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            spellLevel = reader.ReadSByte();
            if (spellLevel < 1 || spellLevel > 6)
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
        }
        
    }
    
}