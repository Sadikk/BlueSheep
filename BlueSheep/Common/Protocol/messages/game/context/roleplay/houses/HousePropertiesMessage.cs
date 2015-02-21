









// Generated on 12/11/2014 19:01:33
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class HousePropertiesMessage : Message
    {
        public new const uint ID =5734;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.HouseInformations properties;
        
        public HousePropertiesMessage()
        {
        }
        
        public HousePropertiesMessage(Types.HouseInformations properties)
        {
            this.properties = properties;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(properties.TypeId);
            properties.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            properties = Types.ProtocolTypeManager.GetInstance<Types.HouseInformations>(reader.ReadShort());
            properties.Deserialize(reader);
        }
        
    }
    
}