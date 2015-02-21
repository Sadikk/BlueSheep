









// Generated on 12/11/2014 19:01:43
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildChangeMemberParametersMessage : Message
    {
        public new const uint ID =5549;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int memberId;
        public short rank;
        public sbyte experienceGivenPercent;
        public int rights;
        
        public GuildChangeMemberParametersMessage()
        {
        }
        
        public GuildChangeMemberParametersMessage(int memberId, short rank, sbyte experienceGivenPercent, int rights)
        {
            this.memberId = memberId;
            this.rank = rank;
            this.experienceGivenPercent = experienceGivenPercent;
            this.rights = rights;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(memberId);
            writer.WriteVarShort(rank);
            writer.WriteSByte(experienceGivenPercent);
            writer.WriteVarInt(rights);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            memberId = reader.ReadVarInt();
            if (memberId < 0)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
            rank = reader.ReadVarShort();
            if (rank < 0)
                throw new Exception("Forbidden value on rank = " + rank + ", it doesn't respect the following condition : rank < 0");
            experienceGivenPercent = reader.ReadSByte();
            if (experienceGivenPercent < 0 || experienceGivenPercent > 100)
                throw new Exception("Forbidden value on experienceGivenPercent = " + experienceGivenPercent + ", it doesn't respect the following condition : experienceGivenPercent < 0 || experienceGivenPercent > 100");
            rights = reader.ReadVarInt();
            if (rights < 0)
                throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0");
        }
        
    }
    
}