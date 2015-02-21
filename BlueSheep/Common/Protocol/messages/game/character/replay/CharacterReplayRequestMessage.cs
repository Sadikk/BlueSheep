









// Generated on 12/11/2014 19:01:24
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class CharacterReplayRequestMessage : Message
    {
        public new const uint ID =167;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int characterId;
        
        public CharacterReplayRequestMessage()
        {
        }
        
        public CharacterReplayRequestMessage(int characterId)
        {
            this.characterId = characterId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(characterId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
        }
        
    }
    
}