


















// Generated on 12/11/2014 19:02:04
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class FightTeamMemberInformations
{

public new const short ID = 44;
public virtual short TypeId
{
    get { return ID; }
}

public int id;
        

public FightTeamMemberInformations()
{
}

public FightTeamMemberInformations(int id)
        {
            this.id = id;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteInt(id);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

id = reader.ReadInt();
            

}


}


}