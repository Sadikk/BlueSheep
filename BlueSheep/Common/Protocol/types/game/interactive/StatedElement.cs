


















// Generated on 12/11/2014 19:02:10
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class StatedElement
{

public new const short ID = 108;
public virtual short TypeId
{
    get { return ID; }
}

public int elementId;
        public short elementCellId;
        public int elementState;
        

public StatedElement()
{
}

public StatedElement(int elementId, short elementCellId, int elementState)
        {
            this.elementId = elementId;
            this.elementCellId = elementCellId;
            this.elementState = elementState;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteInt(elementId);
            writer.WriteVarShort(elementCellId);
            writer.WriteVarInt(elementState);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

elementId = reader.ReadInt();
            if (elementId < 0)
                throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
            elementCellId = reader.ReadVarShort();
            if (elementCellId < 0 || elementCellId > 559)
                throw new Exception("Forbidden value on elementCellId = " + elementCellId + ", it doesn't respect the following condition : elementCellId < 0 || elementCellId > 559");
            elementState = reader.ReadVarInt();
            if (elementState < 0)
                throw new Exception("Forbidden value on elementState = " + elementState + ", it doesn't respect the following condition : elementState < 0");
            

}


}


}