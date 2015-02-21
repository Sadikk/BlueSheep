









// Generated on 12/11/2014 19:01:25
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class MoodSmileyResultMessage : Message
    {
        public new const uint ID =6196;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte resultCode;
        public sbyte smileyId;
        
        public MoodSmileyResultMessage()
        {
        }
        
        public MoodSmileyResultMessage(sbyte resultCode, sbyte smileyId)
        {
            this.resultCode = resultCode;
            this.smileyId = smileyId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(resultCode);
            writer.WriteSByte(smileyId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            resultCode = reader.ReadSByte();
            if (resultCode < 0)
                throw new Exception("Forbidden value on resultCode = " + resultCode + ", it doesn't respect the following condition : resultCode < 0");
            smileyId = reader.ReadSByte();
        }
        
    }
    
}