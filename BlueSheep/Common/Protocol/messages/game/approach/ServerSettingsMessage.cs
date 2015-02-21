









// Generated on 12/11/2014 19:01:21
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ServerSettingsMessage : Message
    {
        public new const uint ID =6340;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string lang;
        public sbyte community;
        public sbyte gameType;
        
        public ServerSettingsMessage()
        {
        }
        
        public ServerSettingsMessage(string lang, sbyte community, sbyte gameType)
        {
            this.lang = lang;
            this.community = community;
            this.gameType = gameType;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(lang);
            writer.WriteSByte(community);
            writer.WriteSByte(gameType);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            lang = reader.ReadUTF();
            community = reader.ReadSByte();
            if (community < 0)
                throw new Exception("Forbidden value on community = " + community + ", it doesn't respect the following condition : community < 0");
            gameType = reader.ReadSByte();
            if (gameType < 0)
                throw new Exception("Forbidden value on gameType = " + gameType + ", it doesn't respect the following condition : gameType < 0");
        }
        
    }
    
}