









// Generated on 12/11/2014 19:01:29
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightTurnReadyMessage : Message
    {
        public new const uint ID =716;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool isReady;
        
        public GameFightTurnReadyMessage()
        {
        }
        
        public GameFightTurnReadyMessage(bool isReady)
        {
            this.isReady = isReady;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(isReady);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            isReady = reader.ReadBoolean();
        }
        
    }
    
}