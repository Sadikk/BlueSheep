


















// Generated on 12/11/2014 19:02:02
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ServerSessionConstantLong : ServerSessionConstant
{

public new const short ID = 429;
public override short TypeId
{
    get { return ID; }
}

public double value;
        

public ServerSessionConstantLong()
{
}

public ServerSessionConstantLong(short id, double value)
         : base(id)
        {
            this.value = value;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteDouble(value);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            value = reader.ReadDouble();
            if (value < -9.007199254740992E15 || value > 9.007199254740992E15)
                throw new Exception("Forbidden value on value = " + value + ", it doesn't respect the following condition : value < -9.007199254740992E15 || value > 9.007199254740992E15");
            

}


}


}