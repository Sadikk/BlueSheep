


















// Generated on 12/11/2014 19:02:11
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class PaddockInformations
{

public new const short ID = 132;
public virtual short TypeId
{
    get { return ID; }
}

public short maxOutdoorMount;
        public short maxItems;
        

public PaddockInformations()
{
}

public PaddockInformations(short maxOutdoorMount, short maxItems)
        {
            this.maxOutdoorMount = maxOutdoorMount;
            this.maxItems = maxItems;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarShort(maxOutdoorMount);
            writer.WriteVarShort(maxItems);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

maxOutdoorMount = reader.ReadVarShort();
            if (maxOutdoorMount < 0)
                throw new Exception("Forbidden value on maxOutdoorMount = " + maxOutdoorMount + ", it doesn't respect the following condition : maxOutdoorMount < 0");
            maxItems = reader.ReadVarShort();
            if (maxItems < 0)
                throw new Exception("Forbidden value on maxItems = " + maxItems + ", it doesn't respect the following condition : maxItems < 0");
            

}


}


}