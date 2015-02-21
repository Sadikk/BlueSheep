









// Generated on 12/11/2014 19:01:20
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AllianceInsiderInfoMessage : Message
    {
        public new const uint ID =6403;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.AllianceFactSheetInformations allianceInfos;
        public Types.GuildInsiderFactSheetInformations[] guilds;
        public Types.PrismSubareaEmptyInfo[] prisms;
        
        public AllianceInsiderInfoMessage()
        {
        }
        
        public AllianceInsiderInfoMessage(Types.AllianceFactSheetInformations allianceInfos, Types.GuildInsiderFactSheetInformations[] guilds, Types.PrismSubareaEmptyInfo[] prisms)
        {
            this.allianceInfos = allianceInfos;
            this.guilds = guilds;
            this.prisms = prisms;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            allianceInfos.Serialize(writer);
            writer.WriteUShort((ushort)guilds.Length);
            foreach (var entry in guilds)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)prisms.Length);
            foreach (var entry in prisms)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            allianceInfos = new Types.AllianceFactSheetInformations();
            allianceInfos.Deserialize(reader);
            var limit = reader.ReadUShort();
            guilds = new Types.GuildInsiderFactSheetInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 guilds[i] = new Types.GuildInsiderFactSheetInformations();
                 guilds[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            prisms = new Types.PrismSubareaEmptyInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 prisms[i] = Types.ProtocolTypeManager.GetInstance<Types.PrismSubareaEmptyInfo>(reader.ReadShort());
                 prisms[i].Deserialize(reader);
            }
        }
        
    }
    
}