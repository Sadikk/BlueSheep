









// Generated on 12/11/2014 19:01:36
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class PaddockSellBuyDialogMessage : Message
    {
        public new const uint ID =6018;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool bsell;
        public int ownerId;
        public int price;
        
        public PaddockSellBuyDialogMessage()
        {
        }
        
        public PaddockSellBuyDialogMessage(bool bsell, int ownerId, int price)
        {
            this.bsell = bsell;
            this.ownerId = ownerId;
            this.price = price;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(bsell);
            writer.WriteVarInt(ownerId);
            writer.WriteVarInt(price);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            bsell = reader.ReadBoolean();
            ownerId = reader.ReadVarInt();
            if (ownerId < 0)
                throw new Exception("Forbidden value on ownerId = " + ownerId + ", it doesn't respect the following condition : ownerId < 0");
            price = reader.ReadVarInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
        }
        
    }
    
}