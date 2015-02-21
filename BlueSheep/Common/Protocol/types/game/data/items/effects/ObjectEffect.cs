


















// Generated on 12/11/2014 19:02:09
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ObjectEffect
{

public new const short ID = 76;
public virtual short TypeId
{
    get { return ID; }
}

public short actionId;
        

public ObjectEffect()
{
}

public ObjectEffect(short actionId)
        {
            this.actionId = actionId;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarShort(actionId);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

actionId = reader.ReadVarShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            

}


}


}