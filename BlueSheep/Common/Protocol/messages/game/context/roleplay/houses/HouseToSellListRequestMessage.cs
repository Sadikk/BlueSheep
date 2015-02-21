









// Generated on 12/11/2014 19:01:34
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class HouseToSellListRequestMessage : Message
    {
        public new const uint ID =6139;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short pageIndex;
        
        public HouseToSellListRequestMessage()
        {
        }
        
        public HouseToSellListRequestMessage(short pageIndex)
        {
            this.pageIndex = pageIndex;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(pageIndex);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            pageIndex = reader.ReadVarShort();
            if (pageIndex < 0)
                throw new Exception("Forbidden value on pageIndex = " + pageIndex + ", it doesn't respect the following condition : pageIndex < 0");
        }
        
    }
    
}