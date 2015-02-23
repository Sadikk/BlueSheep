









// Generated on 12/11/2014 19:01:27
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class DisplayNumericalValuePaddockMessage : Message
    {
        public new const uint ID =6563;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int rideId;
        public int value;
        public sbyte type;
        
        public DisplayNumericalValuePaddockMessage()
        {
        }
        
        public DisplayNumericalValuePaddockMessage(int entityId, int value, sbyte type)
        {
            this.rideId = entityId;
            this.value = value;
            this.type = type;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(rideId);
            writer.WriteInt(value);
            writer.WriteSByte(type);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            rideId = reader.ReadInt();
            value = reader.ReadInt();
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
    }
    
}