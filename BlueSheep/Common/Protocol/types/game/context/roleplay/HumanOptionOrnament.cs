


















// Generated on 12/11/2014 19:02:07
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class HumanOptionOrnament : HumanOption
{

public new const short ID = 411;
public override short TypeId
{
    get { return ID; }
}

public short ornamentId;
        

public HumanOptionOrnament()
{
}

public HumanOptionOrnament(short ornamentId)
        {
            this.ornamentId = ornamentId;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarShort(ornamentId);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            ornamentId = reader.ReadVarShort();
            if (ornamentId < 0)
                throw new Exception("Forbidden value on ornamentId = " + ornamentId + ", it doesn't respect the following condition : ornamentId < 0");
            

}


}


}