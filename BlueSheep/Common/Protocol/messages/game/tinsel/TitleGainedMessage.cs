









// Generated on 12/11/2014 19:02:00
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class TitleGainedMessage : Message
    {
        public new const uint ID =6364;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short titleId;
        
        public TitleGainedMessage()
        {
        }
        
        public TitleGainedMessage(short titleId)
        {
            this.titleId = titleId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(titleId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            titleId = reader.ReadVarShort();
            if (titleId < 0)
                throw new Exception("Forbidden value on titleId = " + titleId + ", it doesn't respect the following condition : titleId < 0");
        }
        
    }
    
}