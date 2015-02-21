









// Generated on 12/11/2014 19:01:30
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class MountInformationRequestMessage : Message
    {
        public new const uint ID =5972;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public double id;
        public double time;
        
        public MountInformationRequestMessage()
        {
        }
        
        public MountInformationRequestMessage(double id, double time)
        {
            this.id = id;
            this.time = time;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteDouble(id);
            writer.WriteDouble(time);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadDouble();
            if (id < -9.007199254740992E15 || id > 9.007199254740992E15)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < -9.007199254740992E15 || id > 9.007199254740992E15");
            time = reader.ReadDouble();
            if (time < -9.007199254740992E15 || time > 9.007199254740992E15)
                throw new Exception("Forbidden value on time = " + time + ", it doesn't respect the following condition : time < -9.007199254740992E15 || time > 9.007199254740992E15");
        }
        
    }
    
}