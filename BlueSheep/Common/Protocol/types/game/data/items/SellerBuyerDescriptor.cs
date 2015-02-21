


















// Generated on 12/11/2014 19:02:09
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class SellerBuyerDescriptor
{

public new const short ID = 121;
public virtual short TypeId
{
    get { return ID; }
}

public int[] quantities;
        public int[] types;
        public float taxPercentage;
        public float taxModificationPercentage;
        public byte maxItemLevel;
        public int maxItemPerAccount;
        public int npcContextualId;
        public short unsoldDelay;
        

public SellerBuyerDescriptor()
{
}

public SellerBuyerDescriptor(int[] quantities, int[] types, float taxPercentage, float taxModificationPercentage, byte maxItemLevel, int maxItemPerAccount, int npcContextualId, short unsoldDelay)
        {
            this.quantities = quantities;
            this.types = types;
            this.taxPercentage = taxPercentage;
            this.taxModificationPercentage = taxModificationPercentage;
            this.maxItemLevel = maxItemLevel;
            this.maxItemPerAccount = maxItemPerAccount;
            this.npcContextualId = npcContextualId;
            this.unsoldDelay = unsoldDelay;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteUShort((ushort)quantities.Length);
            foreach (var entry in quantities)
            {
                 writer.WriteVarInt(entry);
            }
            writer.WriteUShort((ushort)types.Length);
            foreach (var entry in types)
            {
                 writer.WriteVarInt(entry);
            }
            writer.WriteFloat(taxPercentage);
            writer.WriteFloat(taxModificationPercentage);
            writer.WriteByte(maxItemLevel);
            writer.WriteVarInt(maxItemPerAccount);
            writer.WriteInt(npcContextualId);
            writer.WriteVarShort(unsoldDelay);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

var limit = reader.ReadUShort();
            quantities = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 quantities[i] = reader.ReadVarInt();
            }
            limit = reader.ReadUShort();
            types = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 types[i] = reader.ReadVarInt();
            }
            taxPercentage = reader.ReadFloat();
            taxModificationPercentage = reader.ReadFloat();
            maxItemLevel = reader.ReadByte();
            if (maxItemLevel < 0 || maxItemLevel > 255)
                throw new Exception("Forbidden value on maxItemLevel = " + maxItemLevel + ", it doesn't respect the following condition : maxItemLevel < 0 || maxItemLevel > 255");
            maxItemPerAccount = reader.ReadVarInt();
            if (maxItemPerAccount < 0)
                throw new Exception("Forbidden value on maxItemPerAccount = " + maxItemPerAccount + ", it doesn't respect the following condition : maxItemPerAccount < 0");
            npcContextualId = reader.ReadInt();
            unsoldDelay = reader.ReadVarShort();
            if (unsoldDelay < 0)
                throw new Exception("Forbidden value on unsoldDelay = " + unsoldDelay + ", it doesn't respect the following condition : unsoldDelay < 0");
            

}


}


}