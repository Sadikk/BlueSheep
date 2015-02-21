









// Generated on 12/11/2014 19:01:59
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ContactLookMessage : Message
    {
        public new const uint ID =5934;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int requestId;
        public string playerName;
        public int playerId;
        public Types.EntityLook look;
        
        public ContactLookMessage()
        {
        }
        
        public ContactLookMessage(int requestId, string playerName, int playerId, Types.EntityLook look)
        {
            this.requestId = requestId;
            this.playerName = playerName;
            this.playerId = playerId;
            this.look = look;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(requestId);
            writer.WriteUTF(playerName);
            writer.WriteVarInt(playerId);
            look.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            requestId = reader.ReadVarInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            playerName = reader.ReadUTF();
            playerId = reader.ReadVarInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            look = new Types.EntityLook();
            look.Deserialize(reader);
        }
        
    }
    
}