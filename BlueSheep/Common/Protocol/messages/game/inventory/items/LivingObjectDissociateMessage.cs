









// Generated on 12/11/2014 19:01:54
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class LivingObjectDissociateMessage : Message
    {
        public new const uint ID =5723;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int livingUID;
        public byte livingPosition;
        
        public LivingObjectDissociateMessage()
        {
        }
        
        public LivingObjectDissociateMessage(int livingUID, byte livingPosition)
        {
            this.livingUID = livingUID;
            this.livingPosition = livingPosition;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(livingUID);
            writer.WriteByte(livingPosition);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            livingUID = reader.ReadVarInt();
            if (livingUID < 0)
                throw new Exception("Forbidden value on livingUID = " + livingUID + ", it doesn't respect the following condition : livingUID < 0");
            livingPosition = reader.ReadByte();
            if (livingPosition < 0 || livingPosition > 255)
                throw new Exception("Forbidden value on livingPosition = " + livingPosition + ", it doesn't respect the following condition : livingPosition < 0 || livingPosition > 255");
        }
        
    }
    
}