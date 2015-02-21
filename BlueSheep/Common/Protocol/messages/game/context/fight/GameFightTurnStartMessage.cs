









// Generated on 12/11/2014 19:01:29
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightTurnStartMessage : Message
    {
        public new const uint ID =714;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int id;
        public int waitTime;
        
        public GameFightTurnStartMessage()
        {
        }
        
        public GameFightTurnStartMessage(int id, int waitTime)
        {
            this.id = id;
            this.waitTime = waitTime;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteVarInt(waitTime);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
            waitTime = reader.ReadVarInt();
            if (waitTime < 0)
                throw new Exception("Forbidden value on waitTime = " + waitTime + ", it doesn't respect the following condition : waitTime < 0");
        }
        
    }
    
}