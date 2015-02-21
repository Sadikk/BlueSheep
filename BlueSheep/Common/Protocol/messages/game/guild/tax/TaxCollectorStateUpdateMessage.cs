









// Generated on 12/11/2014 19:01:46
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class TaxCollectorStateUpdateMessage : Message
    {
        public new const uint ID =6455;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int uniqueId;
        public sbyte state;
        
        public TaxCollectorStateUpdateMessage()
        {
        }
        
        public TaxCollectorStateUpdateMessage(int uniqueId, sbyte state)
        {
            this.uniqueId = uniqueId;
            this.state = state;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(uniqueId);
            writer.WriteSByte(state);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            uniqueId = reader.ReadInt();
            state = reader.ReadSByte();
        }
        
    }
    
}