









// Generated on 12/11/2014 19:01:25
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class MoodSmileyRequestMessage : Message
    {
        public new const uint ID =6192;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte smileyId;
        
        public MoodSmileyRequestMessage()
        {
        }
        
        public MoodSmileyRequestMessage(sbyte smileyId)
        {
            this.smileyId = smileyId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(smileyId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            smileyId = reader.ReadSByte();
        }
        
    }
    
}