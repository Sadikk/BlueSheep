









// Generated on 12/11/2014 19:01:32
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ComicReadingBeginMessage : Message
    {
        public new const uint ID =6536;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short comicId;
        
        public ComicReadingBeginMessage()
        {
        }
        
        public ComicReadingBeginMessage(short comicId)
        {
            this.comicId = comicId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(comicId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            comicId = reader.ReadVarShort();
            if (comicId < 0)
                throw new Exception("Forbidden value on comicId = " + comicId + ", it doesn't respect the following condition : comicId < 0");
        }
        
    }
    
}