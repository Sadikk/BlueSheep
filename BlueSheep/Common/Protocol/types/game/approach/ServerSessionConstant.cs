


















// Generated on 12/11/2014 19:02:02
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ServerSessionConstant
{

public new const short ID = 430;
public virtual short TypeId
{
    get { return ID; }
}

public short id;
        

public ServerSessionConstant()
{
}

public ServerSessionConstant(short id)
        {
            this.id = id;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarShort(id);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

id = reader.ReadVarShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            

}


}


}