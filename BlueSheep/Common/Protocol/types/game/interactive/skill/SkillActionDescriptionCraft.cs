


















// Generated on 12/11/2014 19:02:10
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class SkillActionDescriptionCraft : SkillActionDescription
{

public new const short ID = 100;
public override short TypeId
{
    get { return ID; }
}

public sbyte maxSlots;
        public sbyte probability;
        

public SkillActionDescriptionCraft()
{
}

public SkillActionDescriptionCraft(short skillId, sbyte maxSlots, sbyte probability)
         : base(skillId)
        {
            this.maxSlots = maxSlots;
            this.probability = probability;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteSByte(maxSlots);
            writer.WriteSByte(probability);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            maxSlots = reader.ReadSByte();
            if (maxSlots < 0)
                throw new Exception("Forbidden value on maxSlots = " + maxSlots + ", it doesn't respect the following condition : maxSlots < 0");
            probability = reader.ReadSByte();
            if (probability < 0)
                throw new Exception("Forbidden value on probability = " + probability + ", it doesn't respect the following condition : probability < 0");
            

}


}


}