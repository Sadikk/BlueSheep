


















// Generated on 12/11/2014 19:02:12
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class GuildFactSheetInformations : GuildInformations
{

public new const short ID = 424;
public override short TypeId
{
    get { return ID; }
}

public int leaderId;
        public byte guildLevel;
        public short nbMembers;
        

public GuildFactSheetInformations()
{
}

public GuildFactSheetInformations(int guildId, string guildName, Types.GuildEmblem guildEmblem, int leaderId, byte guildLevel, short nbMembers)
         : base(guildId, guildName, guildEmblem)
        {
            this.leaderId = leaderId;
            this.guildLevel = guildLevel;
            this.nbMembers = nbMembers;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarInt(leaderId);
            writer.WriteByte(guildLevel);
            writer.WriteVarShort(nbMembers);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            leaderId = reader.ReadVarInt();
            if (leaderId < 0)
                throw new Exception("Forbidden value on leaderId = " + leaderId + ", it doesn't respect the following condition : leaderId < 0");
            guildLevel = reader.ReadByte();
            if (guildLevel < 0 || guildLevel > 255)
                throw new Exception("Forbidden value on guildLevel = " + guildLevel + ", it doesn't respect the following condition : guildLevel < 0 || guildLevel > 255");
            nbMembers = reader.ReadVarShort();
            if (nbMembers < 0)
                throw new Exception("Forbidden value on nbMembers = " + nbMembers + ", it doesn't respect the following condition : nbMembers < 0");
            

}


}


}