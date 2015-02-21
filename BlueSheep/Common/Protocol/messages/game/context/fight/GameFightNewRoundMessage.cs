









// Generated on 12/11/2014 19:01:27
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightNewRoundMessage : Message
    {
        public new const uint ID =6239;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int roundNumber;
        
        public GameFightNewRoundMessage()
        {
        }
        
        public GameFightNewRoundMessage(int roundNumber)
        {
            this.roundNumber = roundNumber;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(roundNumber);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            roundNumber = reader.ReadVarInt();
            if (roundNumber < 0)
                throw new Exception("Forbidden value on roundNumber = " + roundNumber + ", it doesn't respect the following condition : roundNumber < 0");
        }
        
    }
    
}