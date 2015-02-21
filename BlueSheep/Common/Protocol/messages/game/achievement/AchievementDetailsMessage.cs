









// Generated on 12/11/2014 19:01:15
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AchievementDetailsMessage : Message
    {
        public new const uint ID =6378;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.Achievement achievement;
        
        public AchievementDetailsMessage()
        {
        }
        
        public AchievementDetailsMessage(Types.Achievement achievement)
        {
            this.achievement = achievement;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            achievement.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            achievement = new Types.Achievement();
            achievement.Deserialize(reader);
        }
        
    }
    
}