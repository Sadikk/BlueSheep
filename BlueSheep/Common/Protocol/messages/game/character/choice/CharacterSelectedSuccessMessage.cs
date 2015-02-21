









// Generated on 12/11/2014 19:01:23
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class CharacterSelectedSuccessMessage : Message
    {
        public new const uint ID =153;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.CharacterBaseInformations infos;
        public bool isCollectingStats;
        
        public CharacterSelectedSuccessMessage()
        {
        }
        
        public CharacterSelectedSuccessMessage(Types.CharacterBaseInformations infos, bool isCollectingStats)
        {
            this.infos = infos;
            this.isCollectingStats = isCollectingStats;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            infos.Serialize(writer);
            writer.WriteBoolean(isCollectingStats);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            infos = new Types.CharacterBaseInformations();
            infos.Deserialize(reader);
            isCollectingStats = reader.ReadBoolean();
        }
        
    }
    
}