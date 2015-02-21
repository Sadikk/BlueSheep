









// Generated on 12/11/2014 19:01:15
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AchievementDetailsRequestMessage : Message
    {
        public new const uint ID =6380;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short achievementId;
        
        public AchievementDetailsRequestMessage()
        {
        }
        
        public AchievementDetailsRequestMessage(short achievementId)
        {
            this.achievementId = achievementId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(achievementId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            achievementId = reader.ReadVarShort();
            if (achievementId < 0)
                throw new Exception("Forbidden value on achievementId = " + achievementId + ", it doesn't respect the following condition : achievementId < 0");
        }
        
    }
    
}