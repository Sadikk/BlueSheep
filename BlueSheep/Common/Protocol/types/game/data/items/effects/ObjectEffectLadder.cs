


















// Generated on 12/11/2014 19:02:09
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ObjectEffectLadder : ObjectEffectCreature
{

public new const short ID = 81;
public override short TypeId
{
    get { return ID; }
}

public int monsterCount;
        

public ObjectEffectLadder()
{
}

public ObjectEffectLadder(short actionId, short monsterFamilyId, int monsterCount)
         : base(actionId, monsterFamilyId)
        {
            this.monsterCount = monsterCount;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarInt(monsterCount);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            monsterCount = reader.ReadVarInt();
            if (monsterCount < 0)
                throw new Exception("Forbidden value on monsterCount = " + monsterCount + ", it doesn't respect the following condition : monsterCount < 0");
            

}


}


}