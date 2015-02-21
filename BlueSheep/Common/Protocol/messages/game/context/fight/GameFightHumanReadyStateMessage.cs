









// Generated on 12/11/2014 19:01:27
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightHumanReadyStateMessage : Message
    {
        public new const uint ID =740;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int characterId;
        public bool isReady;
        
        public GameFightHumanReadyStateMessage()
        {
        }
        
        public GameFightHumanReadyStateMessage(int characterId, bool isReady)
        {
            this.characterId = characterId;
            this.isReady = isReady;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(characterId);
            writer.WriteBoolean(isReady);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            characterId = reader.ReadVarInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
            isReady = reader.ReadBoolean();
        }
        
    }
    
}