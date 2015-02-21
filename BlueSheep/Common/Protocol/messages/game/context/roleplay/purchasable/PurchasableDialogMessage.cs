









// Generated on 12/11/2014 19:01:40
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class PurchasableDialogMessage : Message
    {
        public new const uint ID =5739;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool buyOrSell;
        public int purchasableId;
        public int price;
        
        public PurchasableDialogMessage()
        {
        }
        
        public PurchasableDialogMessage(bool buyOrSell, int purchasableId, int price)
        {
            this.buyOrSell = buyOrSell;
            this.purchasableId = purchasableId;
            this.price = price;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(buyOrSell);
            writer.WriteVarInt(purchasableId);
            writer.WriteVarInt(price);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            buyOrSell = reader.ReadBoolean();
            purchasableId = reader.ReadVarInt();
            if (purchasableId < 0)
                throw new Exception("Forbidden value on purchasableId = " + purchasableId + ", it doesn't respect the following condition : purchasableId < 0");
            price = reader.ReadVarInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
        }
        
    }
    
}