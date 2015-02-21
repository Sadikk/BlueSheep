









// Generated on 12/11/2014 19:01:27
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightLeaveMessage : Message
    {
        public new const uint ID =721;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int charId;
        
        public GameFightLeaveMessage()
        {
        }
        
        public GameFightLeaveMessage(int charId)
        {
            this.charId = charId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(charId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            charId = reader.ReadInt();
        }
        
    }
    
}