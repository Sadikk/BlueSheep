









// Generated on 12/11/2014 19:01:15
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AchievementDetailedListRequestMessage : Message
    {
        public new const uint ID =6357;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short categoryId;
        
        public AchievementDetailedListRequestMessage()
        {
        }
        
        public AchievementDetailedListRequestMessage(short categoryId)
        {
            this.categoryId = categoryId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(categoryId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            categoryId = reader.ReadVarShort();
            if (categoryId < 0)
                throw new Exception("Forbidden value on categoryId = " + categoryId + ", it doesn't respect the following condition : categoryId < 0");
        }
        
    }
    
}