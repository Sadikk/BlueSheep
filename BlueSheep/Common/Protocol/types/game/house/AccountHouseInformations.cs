


















// Generated on 12/11/2014 19:02:10
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class AccountHouseInformations
{

public new const short ID = 390;
public virtual short TypeId
{
    get { return ID; }
}

public int houseId;
        public short modelId;
        public short worldX;
        public short worldY;
        public int mapId;
        public short subAreaId;
        

public AccountHouseInformations()
{
}

public AccountHouseInformations(int houseId, short modelId, short worldX, short worldY, int mapId, short subAreaId)
        {
            this.houseId = houseId;
            this.modelId = modelId;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

//writer.WriteInt(houseId);
    writer.WriteVarInt(houseId);
    writer.WriteVarShort(modelId);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteVarShort(subAreaId);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

//houseId = reader.ReadInt();
    houseId = reader.ReadVarInt();
    if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            modelId = reader.ReadVarShort();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            mapId = reader.ReadInt();
            subAreaId = reader.ReadVarShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            

}


}


}