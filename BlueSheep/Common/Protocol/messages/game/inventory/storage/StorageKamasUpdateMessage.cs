









// Generated on 12/11/2014 19:01:57
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class StorageKamasUpdateMessage : Message
    {
        public new const uint ID =5645;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int kamasTotal;
        
        public StorageKamasUpdateMessage()
        {
        }
        
        public StorageKamasUpdateMessage(int kamasTotal)
        {
            this.kamasTotal = kamasTotal;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(kamasTotal);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            kamasTotal = reader.ReadInt();
        }
        
    }
    
}