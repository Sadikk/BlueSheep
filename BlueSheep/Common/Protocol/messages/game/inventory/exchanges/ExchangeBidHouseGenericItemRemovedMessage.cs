









// Generated on 12/11/2014 19:01:48
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeBidHouseGenericItemRemovedMessage : Message
    {
        public new const uint ID =5948;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short objGenericId;
        
        public ExchangeBidHouseGenericItemRemovedMessage()
        {
        }
        
        public ExchangeBidHouseGenericItemRemovedMessage(short objGenericId)
        {
            this.objGenericId = objGenericId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(objGenericId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            objGenericId = reader.ReadVarShort();
            if (objGenericId < 0)
                throw new Exception("Forbidden value on objGenericId = " + objGenericId + ", it doesn't respect the following condition : objGenericId < 0");
        }
        
    }
    
}