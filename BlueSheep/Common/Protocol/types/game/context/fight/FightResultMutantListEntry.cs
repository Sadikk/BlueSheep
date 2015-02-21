


















// Generated on 12/11/2014 19:02:04
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class FightResultMutantListEntry : FightResultFighterListEntry
{

public new const short ID = 216;
public override short TypeId
{
    get { return ID; }
}

public short level;
        

public FightResultMutantListEntry()
{
}

public FightResultMutantListEntry(short outcome, sbyte wave, Types.FightLoot rewards, int id, bool alive, short level)
         : base(outcome, wave, rewards, id, alive)
        {
            this.level = level;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarShort(level);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            level = reader.ReadVarShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            

}


}


}