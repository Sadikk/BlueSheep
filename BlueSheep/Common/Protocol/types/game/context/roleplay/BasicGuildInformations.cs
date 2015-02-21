


















// Generated on 12/11/2014 19:02:06
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class BasicGuildInformations : AbstractSocialGroupInfos
{

public new const short ID = 365;
public override short TypeId
{
    get { return ID; }
}

public int guildId;
        public string guildName;
        

public BasicGuildInformations()
{
}

public BasicGuildInformations(int guildId, string guildName)
        {
            this.guildId = guildId;
            this.guildName = guildName;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarInt(guildId);
            writer.WriteUTF(guildName);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            guildId = reader.ReadVarInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            guildName = reader.ReadUTF();
            

}


}


}