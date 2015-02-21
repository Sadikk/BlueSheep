









// Generated on 12/11/2014 19:01:32
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameRolePlayAttackMonsterRequestMessage : Message
    {
        public new const uint ID =6191;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int monsterGroupId;
        
        public GameRolePlayAttackMonsterRequestMessage()
        {
        }
        
        public GameRolePlayAttackMonsterRequestMessage(int monsterGroupId)
        {
            this.monsterGroupId = monsterGroupId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(monsterGroupId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            monsterGroupId = reader.ReadInt();
        }
        
    }
    
}