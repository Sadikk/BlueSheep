









// Generated on 12/11/2014 19:01:33
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameRolePlayArenaRegisterMessage : Message
    {
        public new const uint ID =6280;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int battleMode;
        
        public GameRolePlayArenaRegisterMessage()
        {
        }
        
        public GameRolePlayArenaRegisterMessage(int battleMode)
        {
            this.battleMode = battleMode;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(battleMode);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            battleMode = reader.ReadInt();
            if (battleMode < 0)
                throw new Exception("Forbidden value on battleMode = " + battleMode + ", it doesn't respect the following condition : battleMode < 0");
        }
        
    }
    
}