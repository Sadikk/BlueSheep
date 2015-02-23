


















// Generated on 12/11/2014 19:02:10
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class HouseInformations
{

public new const short ID = 111;
public virtual short TypeId
{
    get { return ID; }
}

public int houseId;
        public int[] doorsOnMap;
        public string ownerName;
        public short modelId;
          public bool isOnSale;
      
      public bool isSaleLocked;
        

public HouseInformations()
{
}

public HouseInformations(int houseId, int[] doorsOnMap, string ownerName, short modelId)
        {
            this.houseId = houseId;
            this.doorsOnMap = doorsOnMap;
            this.ownerName = ownerName;
            this.modelId = modelId;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

//writer.WriteInt(houseId);
    writer.WriteVarInt(houseId);
    writer.WriteUShort((ushort)doorsOnMap.Length);
            foreach (var entry in doorsOnMap)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUTF(ownerName);
            writer.WriteVarShort(modelId);
            

}

public virtual void Deserialize(BigEndianReader reader)
{
             byte b = reader.ReadByte();
         this.isOnSale = BooleanByteWrapper.GetFlag(b,0);
         this.isSaleLocked = BooleanByteWrapper.GetFlag(b,1);
       //  this.houseId = reader.ReadInt();
         this.houseId = reader.ReadVarInt();     
    if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            var limit = reader.ReadUShort();
            doorsOnMap = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 doorsOnMap[i] = reader.ReadInt();
            }
            ownerName = reader.ReadUTF();
            modelId = reader.ReadVarShort();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
            

}


}


}