


















// Generated on 12/11/2014 19:02:10
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class MapObstacle
{

public new const short ID = 200;
public virtual short TypeId
{
    get { return ID; }
}

public short obstacleCellId;
        public sbyte state;
        

public MapObstacle()
{
}

public MapObstacle(short obstacleCellId, sbyte state)
        {
            this.obstacleCellId = obstacleCellId;
            this.state = state;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarShort(obstacleCellId);
            writer.WriteSByte(state);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

obstacleCellId = reader.ReadVarShort();
            if (obstacleCellId < 0 || obstacleCellId > 559)
                throw new Exception("Forbidden value on obstacleCellId = " + obstacleCellId + ", it doesn't respect the following condition : obstacleCellId < 0 || obstacleCellId > 559");
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
            

}


}


}