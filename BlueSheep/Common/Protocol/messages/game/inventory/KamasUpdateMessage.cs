









// Generated on 12/11/2014 19:01:47
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class KamasUpdateMessage : Message
    {
        public new const uint ID =5537;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int kamasTotal;
        
        public KamasUpdateMessage()
        {
        }
        
        public KamasUpdateMessage(int kamasTotal)
        {
            this.kamasTotal = kamasTotal;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(kamasTotal);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            kamasTotal = reader.ReadVarInt();
        }
        
    }
    
}