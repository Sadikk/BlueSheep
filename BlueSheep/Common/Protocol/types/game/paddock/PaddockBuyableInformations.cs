


















// Generated on 12/11/2014 19:02:11
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class PaddockBuyableInformations : PaddockInformations
{

public new const short ID = 130;
public override short TypeId
{
    get { return ID; }
}

public int price;
        public bool locked;
        

public PaddockBuyableInformations()
{
}

public PaddockBuyableInformations(short maxOutdoorMount, short maxItems, int price, bool locked)
         : base(maxOutdoorMount, maxItems)
        {
            this.price = price;
            this.locked = locked;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarInt(price);
            writer.WriteBoolean(locked);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            price = reader.ReadVarInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
            locked = reader.ReadBoolean();
            

}


}


}