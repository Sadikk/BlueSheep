









// Generated on 12/11/2014 19:01:14
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AcquaintanceSearchMessage : Message
    {
        public new const uint ID =6144;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string nickname;
        
        public AcquaintanceSearchMessage()
        {
        }
        
        public AcquaintanceSearchMessage(string nickname)
        {
            this.nickname = nickname;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(nickname);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            nickname = reader.ReadUTF();
        }
        
    }
    
}