using BlueSheep.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Common.Protocol.Types
{
    public class StatisticDataInt : StatisticData
    {
        public new const short ID = 485;
        public virtual short TypeId
        {
            get { return ID; }
        }

        public int value;


        public StatisticDataInt()
        {
        }

        public StatisticDataInt(int value)
        {
            this.value = value;

        }


        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(this.value);
        }

        public virtual void Deserialize(BigEndianReader reader)
        {
            this.value = reader.ReadInt();
        }
    }
}
