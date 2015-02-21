









// Generated on 12/11/2014 19:01:49
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeGoldPaymentForCraftMessage : Message
    {
        public new const uint ID =5833;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool onlySuccess;
        public int goldSum;
        
        public ExchangeGoldPaymentForCraftMessage()
        {
        }
        
        public ExchangeGoldPaymentForCraftMessage(bool onlySuccess, int goldSum)
        {
            this.onlySuccess = onlySuccess;
            this.goldSum = goldSum;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(onlySuccess);
            writer.WriteVarInt(goldSum);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            onlySuccess = reader.ReadBoolean();
            goldSum = reader.ReadVarInt();
            if (goldSum < 0)
                throw new Exception("Forbidden value on goldSum = " + goldSum + ", it doesn't respect the following condition : goldSum < 0");
        }
        
    }
    
}