









// Generated on 12/11/2014 19:01:57
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AreaFightModificatorUpdateMessage : Message
    {
        public new const uint ID =6493;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int spellPairId;
        
        public AreaFightModificatorUpdateMessage()
        {
        }
        
        public AreaFightModificatorUpdateMessage(int spellPairId)
        {
            this.spellPairId = spellPairId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(spellPairId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            spellPairId = reader.ReadInt();
        }
        
    }
    
}