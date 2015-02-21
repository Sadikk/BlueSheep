









// Generated on 12/11/2014 19:01:49
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeHandleMountStableMessage : Message
    {
        public new const uint ID =5965;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte actionType;
        public int rideId;
        
        public ExchangeHandleMountStableMessage()
        {
        }
        
        public ExchangeHandleMountStableMessage(sbyte actionType, int rideId)
        {
            this.actionType = actionType;
            this.rideId = rideId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(actionType);
            writer.WriteVarInt(rideId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            actionType = reader.ReadSByte();
            rideId = reader.ReadVarInt();
            if (rideId < 0)
                throw new Exception("Forbidden value on rideId = " + rideId + ", it doesn't respect the following condition : rideId < 0");
        }
        
    }
    
}