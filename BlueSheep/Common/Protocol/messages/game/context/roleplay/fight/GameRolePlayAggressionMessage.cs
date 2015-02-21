









// Generated on 12/11/2014 19:01:32
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameRolePlayAggressionMessage : Message
    {
        public new const uint ID =6073;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int attackerId;
        public int defenderId;
        
        public GameRolePlayAggressionMessage()
        {
        }
        
        public GameRolePlayAggressionMessage(int attackerId, int defenderId)
        {
            this.attackerId = attackerId;
            this.defenderId = defenderId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(attackerId);
            writer.WriteVarInt(defenderId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            attackerId = reader.ReadVarInt();
            if (attackerId < 0)
                throw new Exception("Forbidden value on attackerId = " + attackerId + ", it doesn't respect the following condition : attackerId < 0");
            defenderId = reader.ReadVarInt();
            if (defenderId < 0)
                throw new Exception("Forbidden value on defenderId = " + defenderId + ", it doesn't respect the following condition : defenderId < 0");
        }
        
    }
    
}