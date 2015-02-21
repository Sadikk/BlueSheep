









// Generated on 12/11/2014 19:01:15
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AchievementListMessage : Message
    {
        public new const uint ID =6205;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short[] finishedAchievementsIds;
        public Types.AchievementRewardable[] rewardableAchievements;
        
        public AchievementListMessage()
        {
        }
        
        public AchievementListMessage(short[] finishedAchievementsIds, Types.AchievementRewardable[] rewardableAchievements)
        {
            this.finishedAchievementsIds = finishedAchievementsIds;
            this.rewardableAchievements = rewardableAchievements;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)finishedAchievementsIds.Length);
            foreach (var entry in finishedAchievementsIds)
            {
                 writer.WriteVarShort(entry);
            }
            writer.WriteUShort((ushort)rewardableAchievements.Length);
            foreach (var entry in rewardableAchievements)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            finishedAchievementsIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 finishedAchievementsIds[i] = reader.ReadVarShort();
            }
            limit = reader.ReadUShort();
            rewardableAchievements = new Types.AchievementRewardable[limit];
            for (int i = 0; i < limit; i++)
            {
                 rewardableAchievements[i] = new Types.AchievementRewardable();
                 rewardableAchievements[i].Deserialize(reader);
            }
        }
        
    }
    
}