


















// Generated on 12/11/2014 19:02:08
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class GoldItem : Item
{

public new const short ID = 123;
public override short TypeId
{
    get { return ID; }
}

public int sum;
        

public GoldItem()
{
}

public GoldItem(int sum)
        {
            this.sum = sum;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarInt(sum);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            sum = reader.ReadVarInt();
            if (sum < 0)
                throw new Exception("Forbidden value on sum = " + sum + ", it doesn't respect the following condition : sum < 0");
            

}


}


}