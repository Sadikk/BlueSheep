









// Generated on 12/11/2014 19:01:28
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightSpectateMessage : Message
    {
        public new const uint ID =6069;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.FightDispellableEffectExtendedInformations[] effects;
        public Types.GameActionMark[] marks;
        public short gameTurn;
        public int fightStart;
        
        public GameFightSpectateMessage()
        {
        }
        
        public GameFightSpectateMessage(Types.FightDispellableEffectExtendedInformations[] effects, Types.GameActionMark[] marks, short gameTurn, int fightStart)
        {
            this.effects = effects;
            this.marks = marks;
            this.gameTurn = gameTurn;
            this.fightStart = fightStart;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)effects.Length);
            foreach (var entry in effects)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)marks.Length);
            foreach (var entry in marks)
            {
                 entry.Serialize(writer);
            }
            writer.WriteVarShort(gameTurn);
            writer.WriteInt(fightStart);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            effects = new Types.FightDispellableEffectExtendedInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 effects[i] = new Types.FightDispellableEffectExtendedInformations();
                 effects[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            marks = new Types.GameActionMark[limit];
            for (int i = 0; i < limit; i++)
            {
                 marks[i] = new Types.GameActionMark();
                 marks[i].Deserialize(reader);
            }
            gameTurn = reader.ReadVarShort();
            if (gameTurn < 0)
                throw new Exception("Forbidden value on gameTurn = " + gameTurn + ", it doesn't respect the following condition : gameTurn < 0");
            fightStart = reader.ReadInt();
            if (fightStart < 0)
                throw new Exception("Forbidden value on fightStart = " + fightStart + ", it doesn't respect the following condition : fightStart < 0");
        }
        
    }
    
}