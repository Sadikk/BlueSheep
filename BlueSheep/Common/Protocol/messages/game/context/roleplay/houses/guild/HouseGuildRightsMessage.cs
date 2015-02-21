









// Generated on 12/11/2014 19:01:34
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class HouseGuildRightsMessage : Message
    {
        public new const uint ID =5703;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short houseId;
        public Types.GuildInformations guildInfo;
        public int rights;
        
        public HouseGuildRightsMessage()
        {
        }
        
        public HouseGuildRightsMessage(short houseId, Types.GuildInformations guildInfo, int rights)
        {
            this.houseId = houseId;
            this.guildInfo = guildInfo;
            this.rights = rights;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(houseId);
            guildInfo.Serialize(writer);
            writer.WriteVarInt(rights);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            houseId = reader.ReadVarShort();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            guildInfo = new Types.GuildInformations();
            guildInfo.Deserialize(reader);
            rights = reader.ReadVarInt();
            if (rights < 0)
                throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0");
        }
        
    }
    
}