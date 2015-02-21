









// Generated on 12/11/2014 19:01:30
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class MountSterilizedMessage : Message
    {
        public new const uint ID =5977;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public double mountId;
        
        public MountSterilizedMessage()
        {
        }
        
        public MountSterilizedMessage(double mountId)
        {
            this.mountId = mountId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteDouble(mountId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            mountId = reader.ReadDouble();
            if (mountId < -9.007199254740992E15 || mountId > 9.007199254740992E15)
                throw new Exception("Forbidden value on mountId = " + mountId + ", it doesn't respect the following condition : mountId < -9.007199254740992E15 || mountId > 9.007199254740992E15");
        }
        
    }
    
}