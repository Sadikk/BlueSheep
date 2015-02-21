









// Generated on 12/11/2014 19:01:28
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightPlacementSwapPositionsOfferMessage : Message
    {
        public new const uint ID =6542;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int requestId;
        public int requesterId;
        public short requesterCellId;
        public int requestedId;
        public short requestedCellId;
        
        public GameFightPlacementSwapPositionsOfferMessage()
        {
        }
        
        public GameFightPlacementSwapPositionsOfferMessage(int requestId, int requesterId, short requesterCellId, int requestedId, short requestedCellId)
        {
            this.requestId = requestId;
            this.requesterId = requesterId;
            this.requesterCellId = requesterCellId;
            this.requestedId = requestedId;
            this.requestedCellId = requestedCellId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(requestId);
            writer.WriteVarInt(requesterId);
            writer.WriteVarShort(requesterCellId);
            writer.WriteVarInt(requestedId);
            writer.WriteVarShort(requestedCellId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            requesterId = reader.ReadVarInt();
            if (requesterId < 0)
                throw new Exception("Forbidden value on requesterId = " + requesterId + ", it doesn't respect the following condition : requesterId < 0");
            requesterCellId = reader.ReadVarShort();
            if (requesterCellId < 0 || requesterCellId > 559)
                throw new Exception("Forbidden value on requesterCellId = " + requesterCellId + ", it doesn't respect the following condition : requesterCellId < 0 || requesterCellId > 559");
            requestedId = reader.ReadVarInt();
            if (requestedId < 0)
                throw new Exception("Forbidden value on requestedId = " + requestedId + ", it doesn't respect the following condition : requestedId < 0");
            requestedCellId = reader.ReadVarShort();
            if (requestedCellId < 0 || requestedCellId > 559)
                throw new Exception("Forbidden value on requestedCellId = " + requestedCellId + ", it doesn't respect the following condition : requestedCellId < 0 || requestedCellId > 559");
        }
        
    }
    
}