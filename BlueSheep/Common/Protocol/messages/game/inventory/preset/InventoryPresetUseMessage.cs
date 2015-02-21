









// Generated on 12/11/2014 19:01:56
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class InventoryPresetUseMessage : Message
    {
        public new const uint ID =6167;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte presetId;
        
        public InventoryPresetUseMessage()
        {
        }
        
        public InventoryPresetUseMessage(sbyte presetId)
        {
            this.presetId = presetId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(presetId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
        }
        
    }
    
}