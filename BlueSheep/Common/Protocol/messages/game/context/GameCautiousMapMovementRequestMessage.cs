









// Generated on 12/11/2014 19:01:26
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameCautiousMapMovementRequestMessage : GameMapMovementRequestMessage
    {
        public new const uint ID =6496;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        
        public GameCautiousMapMovementRequestMessage()
        {
        }
        
        public GameCautiousMapMovementRequestMessage(short[] keyMovements, int mapId)
         : base(keyMovements, mapId)
        {
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}