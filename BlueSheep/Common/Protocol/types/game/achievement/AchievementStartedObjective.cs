


















// Generated on 12/11/2014 19:02:02
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class AchievementStartedObjective : AchievementObjective
{

public new const short ID = 402;
public override short TypeId
{
    get { return ID; }
}

public short value;
        

public AchievementStartedObjective()
{
}

public AchievementStartedObjective(int id, short maxValue, short value)
         : base(id, maxValue)
        {
            this.value = value;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarShort(value);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            value = reader.ReadVarShort();
            if (value < 0)
                throw new Exception("Forbidden value on value = " + value + ", it doesn't respect the following condition : value < 0");
            

}


}


}