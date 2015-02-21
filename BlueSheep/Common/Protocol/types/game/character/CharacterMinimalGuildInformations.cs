


















// Generated on 12/11/2014 19:02:02
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class CharacterMinimalGuildInformations : CharacterMinimalPlusLookInformations
{

public new const short ID = 445;
public override short TypeId
{
    get { return ID; }
}

public Types.BasicGuildInformations guild;
        

public CharacterMinimalGuildInformations()
{
}

public CharacterMinimalGuildInformations(int id, byte level, string name, Types.EntityLook entityLook, Types.BasicGuildInformations guild)
         : base(id, level, name, entityLook)
        {
            this.guild = guild;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            guild.Serialize(writer);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            guild = new Types.BasicGuildInformations();
            guild.Deserialize(reader);
            

}


}


}