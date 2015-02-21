


















// Generated on 12/11/2014 19:02:09
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ObjectEffectMinMax : ObjectEffect
{

public new const short ID = 82;
public override short TypeId
{
    get { return ID; }
}

public int min;
        public int max;
        

public ObjectEffectMinMax()
{
}

public ObjectEffectMinMax(short actionId, int min, int max)
         : base(actionId)
        {
            this.min = min;
            this.max = max;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarInt(min);
            writer.WriteVarInt(max);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            min = reader.ReadVarInt();
            if (min < 0)
                throw new Exception("Forbidden value on min = " + min + ", it doesn't respect the following condition : min < 0");
            max = reader.ReadVarInt();
            if (max < 0)
                throw new Exception("Forbidden value on max = " + max + ", it doesn't respect the following condition : max < 0");
            

}


}


}