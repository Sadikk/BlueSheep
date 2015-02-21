


















// Generated on 12/11/2014 19:02:05
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class GameFightFighterMonsterLightInformations : GameFightFighterLightInformations
{

public new const short ID = 455;
public override short TypeId
{
    get { return ID; }
}

public short creatureGenericId;
        

public GameFightFighterMonsterLightInformations()
{
}

public GameFightFighterMonsterLightInformations(int id, sbyte wave, short level, sbyte breed, short creatureGenericId)
         : base(id, wave, level, breed)
        {
            this.creatureGenericId = creatureGenericId;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarShort(creatureGenericId);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            creatureGenericId = reader.ReadVarShort();
            if (creatureGenericId < 0)
                throw new Exception("Forbidden value on creatureGenericId = " + creatureGenericId + ", it doesn't respect the following condition : creatureGenericId < 0");
            

}


}


}