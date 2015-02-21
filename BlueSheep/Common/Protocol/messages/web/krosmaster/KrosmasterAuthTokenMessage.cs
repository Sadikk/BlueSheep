









// Generated on 12/11/2014 19:02:01
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class KrosmasterAuthTokenMessage : Message
    {
        public new const uint ID =6351;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string token;
        
        public KrosmasterAuthTokenMessage()
        {
        }
        
        public KrosmasterAuthTokenMessage(string token)
        {
            this.token = token;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(token);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            token = reader.ReadUTF();
        }
        
    }
    
}