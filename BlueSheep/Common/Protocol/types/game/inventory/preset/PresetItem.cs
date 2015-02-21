


















// Generated on 12/11/2014 19:02:10
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class PresetItem
{

public new const short ID = 354;
public virtual short TypeId
{
    get { return ID; }
}

public byte position;
        public short objGid;
        public int objUid;
        

public PresetItem()
{
}

public PresetItem(byte position, short objGid, int objUid)
        {
            this.position = position;
            this.objGid = objGid;
            this.objUid = objUid;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteByte(position);
            writer.WriteVarShort(objGid);
            writer.WriteVarInt(objUid);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

position = reader.ReadByte();
            if (position < 0 || position > 255)
                throw new Exception("Forbidden value on position = " + position + ", it doesn't respect the following condition : position < 0 || position > 255");
            objGid = reader.ReadVarShort();
            if (objGid < 0)
                throw new Exception("Forbidden value on objGid = " + objGid + ", it doesn't respect the following condition : objGid < 0");
            objUid = reader.ReadVarInt();
            if (objUid < 0)
                throw new Exception("Forbidden value on objUid = " + objUid + ", it doesn't respect the following condition : objUid < 0");
            

}


}


}