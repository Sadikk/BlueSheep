


















// Generated on 12/11/2014 19:02:07
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class NamedPartyTeamWithOutcome
{

public new const short ID = 470;
public virtual short TypeId
{
    get { return ID; }
}

public Types.NamedPartyTeam team;
        public short outcome;
        

public NamedPartyTeamWithOutcome()
{
}

public NamedPartyTeamWithOutcome(Types.NamedPartyTeam team, short outcome)
        {
            this.team = team;
            this.outcome = outcome;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

team.Serialize(writer);
            writer.WriteVarShort(outcome);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

team = new Types.NamedPartyTeam();
            team.Deserialize(reader);
            outcome = reader.ReadVarShort();
            if (outcome < 0)
                throw new Exception("Forbidden value on outcome = " + outcome + ", it doesn't respect the following condition : outcome < 0");
            

}


}


}