









// Generated on 12/11/2014 19:01:48
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeBidHouseBuyResultMessage : Message
    {
        public new const uint ID =6272;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int uid;
        public bool bought;
        
        public ExchangeBidHouseBuyResultMessage()
        {
        }
        
        public ExchangeBidHouseBuyResultMessage(int uid, bool bought)
        {
            this.uid = uid;
            this.bought = bought;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(uid);
            writer.WriteBoolean(bought);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            uid = reader.ReadVarInt();
            if (uid < 0)
                throw new Exception("Forbidden value on uid = " + uid + ", it doesn't respect the following condition : uid < 0");
            bought = reader.ReadBoolean();
        }
        
    }
    
}