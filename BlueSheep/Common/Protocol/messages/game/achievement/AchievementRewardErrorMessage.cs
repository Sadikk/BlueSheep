









// Generated on 12/11/2014 19:01:15
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AchievementRewardErrorMessage : Message
    {
        public new const uint ID =6375;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short achievementId;
        
        public AchievementRewardErrorMessage()
        {
        }
        
        public AchievementRewardErrorMessage(short achievementId)
        {
            this.achievementId = achievementId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(achievementId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            achievementId = reader.ReadShort();
        }
        
    }
    
}