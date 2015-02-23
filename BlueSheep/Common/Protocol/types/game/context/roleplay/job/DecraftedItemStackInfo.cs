using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueSheep.Common.IO;

namespace BlueSheep.Common.Protocol.types.game.context.roleplay.job
{
    public class DecraftedItemStackInfo
    {
        public new const short ID = 481;
        public virtual short TypeId
        {
            get { return ID; }
        }
        public int objectUID;
        public float bonusMin;
        public float bonusMax;
        public List<short> runesId;
        public List<int> runesQty;

        public DecraftedItemStackInfo()
        {
            runesId = new List<short>();
            runesQty = new List<int>();
        }
        public DecraftedItemStackInfo(int objectUID, float bonusMin, float bonusMax, List<short> runesId, List<int> runesQty)
        {
            this.objectUID = objectUID;
            this.bonusMin = bonusMin;
            this.bonusMax = bonusMax;
            this.runesId = runesId;
            this.runesQty = runesQty;
        }

        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(this.objectUID);
            writer.WriteFloat(this.bonusMin);
            writer.WriteFloat(this.bonusMax);
            writer.WriteShort((short)this.runesId.Count);
            for(int i=0;i<runesId.Count;i++)
            {
                writer.WriteVarShort(this.runesId[i]);
            }
            writer.WriteShort((short)this.runesQty.Count);
            for(int i=0;i<runesQty.Count;i++)
            {
                writer.WriteVarInt(this.runesQty[i]);
            }
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            this.objectUID = reader.ReadVarInt();
            this.bonusMin = reader.ReadFloat();
            this.bonusMax = reader.ReadFloat();
            var limit = reader.ReadUShort();
            for(int i=0;i<limit;i++)
            {
                this.runesId.Add(reader.ReadVarShort());
            }
            var newLimit = reader.ReadUShort();
            for(int i=0;i<newLimit;i++)
            {
                this.runesQty.Add(reader.ReadVarInt());
            }
        }
    }
}
