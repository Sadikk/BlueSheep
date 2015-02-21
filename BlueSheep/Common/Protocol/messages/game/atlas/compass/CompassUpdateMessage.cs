









// Generated on 12/11/2014 19:01:21
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class CompassUpdateMessage : Message
    {
        public new const uint ID =5591;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte type;
        public Types.MapCoordinates coords;
        
        public CompassUpdateMessage()
        {
        }
        
        public CompassUpdateMessage(sbyte type, Types.MapCoordinates coords)
        {
            this.type = type;
            this.coords = coords;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(type);
            writer.WriteShort(coords.TypeId);
            coords.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            coords = Types.ProtocolTypeManager.GetInstance<Types.MapCoordinates>(reader.ReadShort());
            coords.Deserialize(reader);
        }
        
    }
    
}