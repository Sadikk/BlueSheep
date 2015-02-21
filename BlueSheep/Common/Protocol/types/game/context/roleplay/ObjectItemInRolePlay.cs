


















// Generated on 12/11/2014 19:02:07
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ObjectItemInRolePlay
{

public new const short ID = 198;
public virtual short TypeId
{
    get { return ID; }
}

public short cellId;
        public short objectGID;
        

public ObjectItemInRolePlay()
{
}

public ObjectItemInRolePlay(short cellId, short objectGID)
        {
            this.cellId = cellId;
            this.objectGID = objectGID;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarShort(cellId);
            writer.WriteVarShort(objectGID);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

cellId = reader.ReadVarShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
            objectGID = reader.ReadVarShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            

}


}


}