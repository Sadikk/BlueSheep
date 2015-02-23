using BlueSheep.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Common.Protocol.Types
{
    public class StatisticDataByte : StatisticData
    {
        public new const short ID = 486;
        public virtual short TypeId
        {
            get { return ID; }
        }

        public byte value;


        public StatisticDataByte()
        {
        }

        public StatisticDataByte(byte value)
        {
            this.value = value;

        }


        public virtual void Serialize(BigEndianWriter writer)
        {

            writer.WriteByte(this.value);



        }

        public virtual void Deserialize(BigEndianReader reader)
        {
            this.value = reader.ReadByte();
        }
    }
}
