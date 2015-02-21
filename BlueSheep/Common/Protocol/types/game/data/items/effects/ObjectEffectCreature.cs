


















// Generated on 12/11/2014 19:02:09
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ObjectEffectCreature : ObjectEffect
{

public new const short ID = 71;
public override short TypeId
{
    get { return ID; }
}

public short monsterFamilyId;
        

public ObjectEffectCreature()
{
}

public ObjectEffectCreature(short actionId, short monsterFamilyId)
         : base(actionId)
        {
            this.monsterFamilyId = monsterFamilyId;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarShort(monsterFamilyId);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            monsterFamilyId = reader.ReadVarShort();
            if (monsterFamilyId < 0)
                throw new Exception("Forbidden value on monsterFamilyId = " + monsterFamilyId + ", it doesn't respect the following condition : monsterFamilyId < 0");
            

}


}


}