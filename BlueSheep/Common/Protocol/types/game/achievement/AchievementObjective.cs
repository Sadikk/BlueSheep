


















// Generated on 12/11/2014 19:02:02
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class AchievementObjective
{

public new const short ID = 404;
public virtual short TypeId
{
    get { return ID; }
}

public int id;
        public short maxValue;
        

public AchievementObjective()
{
}

public AchievementObjective(int id, short maxValue)
        {
            this.id = id;
            this.maxValue = maxValue;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarInt(id);
            writer.WriteVarShort(maxValue);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

id = reader.ReadVarInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            maxValue = reader.ReadVarShort();
            if (maxValue < 0)
                throw new Exception("Forbidden value on maxValue = " + maxValue + ", it doesn't respect the following condition : maxValue < 0");
            

}


}


}