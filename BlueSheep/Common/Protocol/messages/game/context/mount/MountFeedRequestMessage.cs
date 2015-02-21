









// Generated on 12/11/2014 19:01:30
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class MountFeedRequestMessage : Message
    {
        public new const uint ID =6189;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public double mountUid;
        public sbyte mountLocation;
        public int mountFoodUid;
        public int quantity;
        
        public MountFeedRequestMessage()
        {
        }
        
        public MountFeedRequestMessage(double mountUid, sbyte mountLocation, int mountFoodUid, int quantity)
        {
            this.mountUid = mountUid;
            this.mountLocation = mountLocation;
            this.mountFoodUid = mountFoodUid;
            this.quantity = quantity;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarLong(mountUid);
            writer.WriteSByte(mountLocation);
            writer.WriteVarInt(mountFoodUid);
            writer.WriteVarInt(quantity);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            mountUid = reader.ReadVarUhLong();
            if (mountUid < 0 || mountUid > 9.007199254740992E15)
                throw new Exception("Forbidden value on mountUid = " + mountUid + ", it doesn't respect the following condition : mountUid < 0 || mountUid > 9.007199254740992E15");
            mountLocation = reader.ReadSByte();
            mountFoodUid = reader.ReadVarInt();
            if (mountFoodUid < 0)
                throw new Exception("Forbidden value on mountFoodUid = " + mountFoodUid + ", it doesn't respect the following condition : mountFoodUid < 0");
            quantity = reader.ReadVarInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}