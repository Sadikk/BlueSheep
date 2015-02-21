









// Generated on 12/11/2014 19:01:47
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class InteractiveUseErrorMessage : Message
    {
        public new const uint ID =6384;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int elemId;
        public int skillInstanceUid;
        
        public InteractiveUseErrorMessage()
        {
        }
        
        public InteractiveUseErrorMessage(int elemId, int skillInstanceUid)
        {
            this.elemId = elemId;
            this.skillInstanceUid = skillInstanceUid;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(elemId);
            writer.WriteVarInt(skillInstanceUid);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            elemId = reader.ReadVarInt();
            if (elemId < 0)
                throw new Exception("Forbidden value on elemId = " + elemId + ", it doesn't respect the following condition : elemId < 0");
            skillInstanceUid = reader.ReadVarInt();
            if (skillInstanceUid < 0)
                throw new Exception("Forbidden value on skillInstanceUid = " + skillInstanceUid + ", it doesn't respect the following condition : skillInstanceUid < 0");
        }
        
    }
    
}