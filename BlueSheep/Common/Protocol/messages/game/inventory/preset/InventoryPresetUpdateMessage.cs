









// Generated on 12/11/2014 19:01:56
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class InventoryPresetUpdateMessage : Message
    {
        public new const uint ID =6171;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.Preset preset;
        
        public InventoryPresetUpdateMessage()
        {
        }
        
        public InventoryPresetUpdateMessage(Types.Preset preset)
        {
            this.preset = preset;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            preset.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            preset = new Types.Preset();
            preset.Deserialize(reader);
        }
        
    }
    
}