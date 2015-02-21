









// Generated on 12/11/2014 19:01:33
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class HouseKickRequestMessage : Message
    {
        public new const uint ID =5698;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int id;
        
        public HouseKickRequestMessage()
        {
        }
        
        public HouseKickRequestMessage(int id)
        {
            this.id = id;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(id);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadVarInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
        }
        
    }
    
}