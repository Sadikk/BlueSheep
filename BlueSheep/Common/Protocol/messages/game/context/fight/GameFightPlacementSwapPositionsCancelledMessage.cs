









// Generated on 12/11/2014 19:01:28
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightPlacementSwapPositionsCancelledMessage : Message
    {
        public new const uint ID =6546;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int requestId;
        public int cancellerId;
        
        public GameFightPlacementSwapPositionsCancelledMessage()
        {
        }
        
        public GameFightPlacementSwapPositionsCancelledMessage(int requestId, int cancellerId)
        {
            this.requestId = requestId;
            this.cancellerId = cancellerId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(requestId);
            writer.WriteVarInt(cancellerId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            cancellerId = reader.ReadVarInt();
            if (cancellerId < 0)
                throw new Exception("Forbidden value on cancellerId = " + cancellerId + ", it doesn't respect the following condition : cancellerId < 0");
        }
        
    }
    
}