









// Generated on 12/11/2014 19:01:21
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AlmanachCalendarDateMessage : Message
    {
        public new const uint ID =6341;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int date;
        
        public AlmanachCalendarDateMessage()
        {
        }
        
        public AlmanachCalendarDateMessage(int date)
        {
            this.date = date;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(date);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            date = reader.ReadInt();
        }
        
    }
    
}