


















// Generated on 12/11/2014 19:02:10
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class SkillActionDescriptionCollect : SkillActionDescriptionTimed
{

public new const short ID = 99;
public override short TypeId
{
    get { return ID; }
}

public short min;
        public short max;
        

public SkillActionDescriptionCollect()
{
}

public SkillActionDescriptionCollect(short skillId, byte time, short min, short max)
         : base(skillId, time)
        {
            this.min = min;
            this.max = max;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarShort(min);
            writer.WriteVarShort(max);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            min = reader.ReadVarShort();
            if (min < 0)
                throw new Exception("Forbidden value on min = " + min + ", it doesn't respect the following condition : min < 0");
            max = reader.ReadVarShort();
            if (max < 0)
                throw new Exception("Forbidden value on max = " + max + ", it doesn't respect the following condition : max < 0");
            

}


}


}