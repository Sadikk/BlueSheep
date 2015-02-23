using BlueSheep.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Common.Protocol.Types
{
    public class StatisticData
    {
        public new const short ID = 484;
        public virtual short TypeId
        {
            get { return ID; }
        }

        public uint actionId;


        public StatisticData()
        {
        }

        public StatisticData(uint value)
        {
            this.actionId = value;

        }


        public virtual void Serialize(BigEndianWriter writer)
        {

            writer.WriteVarShort((int)this.actionId);



        }

        public virtual void Deserialize(BigEndianReader reader)
        {
            this.actionId = (uint)reader.ReadVarShort();
        }
    }
}
