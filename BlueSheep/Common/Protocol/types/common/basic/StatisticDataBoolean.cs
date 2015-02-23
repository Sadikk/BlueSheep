using BlueSheep.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Common.Protocol.Types
{
    public class StatisticDataBoolean : StatisticData
    {
        public new const short ID = 482;
        public virtual short TypeId
        {
            get { return ID; }
        }

        public bool value;


        public StatisticDataBoolean()
        {
        }

        public StatisticDataBoolean(bool value)
        {
            this.value = value;

        }


        public virtual void Serialize(BigEndianWriter writer)
        {

            writer.WriteBoolean(this.value);



        }

        public virtual void Deserialize(BigEndianReader reader)
        {
            this.value = reader.ReadBoolean();
        }
    }
}
