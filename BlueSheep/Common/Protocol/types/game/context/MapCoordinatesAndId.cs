


















// Generated on 12/11/2014 19:02:04
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class MapCoordinatesAndId : MapCoordinates
{

public new const short ID = 392;
public override short TypeId
{
    get { return ID; }
}

public int mapId;
        

public MapCoordinatesAndId()
{
}

public MapCoordinatesAndId(short worldX, short worldY, int mapId)
         : base(worldX, worldY)
        {
            this.mapId = mapId;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteInt(mapId);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            mapId = reader.ReadInt();
            

}


}


}