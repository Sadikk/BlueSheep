









// Generated on 12/11/2014 19:01:28
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightStartingMessage : Message
    {
        public new const uint ID =700;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte fightType;
        public int attackerId;
        public int defenderId;
        
        public GameFightStartingMessage()
        {
        }
        
        public GameFightStartingMessage(sbyte fightType, int attackerId, int defenderId)
        {
            this.fightType = fightType;
            this.attackerId = attackerId;
            this.defenderId = defenderId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(fightType);
            writer.WriteInt(attackerId);
            writer.WriteInt(defenderId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            fightType = reader.ReadSByte();
            if (fightType < 0)
                throw new Exception("Forbidden value on fightType = " + fightType + ", it doesn't respect the following condition : fightType < 0");
            attackerId = reader.ReadInt();
            defenderId = reader.ReadInt();
        }
        
    }
    
}