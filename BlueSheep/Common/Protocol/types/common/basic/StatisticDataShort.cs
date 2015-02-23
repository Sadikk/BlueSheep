using BlueSheep.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Common.Protocol.Types
{
    public class StatisticDataShort : StatisticData
    {
        public new const short ID = 488;
        public virtual short TypeId
        {
            get { return ID; }
        }

        public short value;


        public StatisticDataShort()
        {
        }

        public StatisticDataShort(short value)
        {
            this.value = value;

        }


        public virtual void Serialize(BigEndianWriter writer)
        {

            writer.WriteShort(this.value);



        }

        public virtual void Deserialize(BigEndianReader reader)
        {
            this.value = reader.ReadShort();
        }
    }
}
