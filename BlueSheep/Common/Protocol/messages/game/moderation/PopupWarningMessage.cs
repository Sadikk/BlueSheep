









// Generated on 12/11/2014 19:01:57
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class PopupWarningMessage : Message
    {
        public new const uint ID =6134;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public byte lockDuration;
        public string author;
        public string content;
        
        public PopupWarningMessage()
        {
        }
        
        public PopupWarningMessage(byte lockDuration, string author, string content)
        {
            this.lockDuration = lockDuration;
            this.author = author;
            this.content = content;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteByte(lockDuration);
            writer.WriteUTF(author);
            writer.WriteUTF(content);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            lockDuration = reader.ReadByte();
            if (lockDuration < 0 || lockDuration > 255)
                throw new Exception("Forbidden value on lockDuration = " + lockDuration + ", it doesn't respect the following condition : lockDuration < 0 || lockDuration > 255");
            author = reader.ReadUTF();
            content = reader.ReadUTF();
        }
        
    }
    
}