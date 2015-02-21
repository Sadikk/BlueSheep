









// Generated on 12/11/2014 19:01:48
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeBidHouseInListRemovedMessage : Message
    {
        public new const uint ID =5950;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int itemUID;
        
        public ExchangeBidHouseInListRemovedMessage()
        {
        }
        
        public ExchangeBidHouseInListRemovedMessage(int itemUID)
        {
            this.itemUID = itemUID;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(itemUID);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            itemUID = reader.ReadInt();
        }
        
    }
    
}