









// Generated on 12/11/2014 19:01:44
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildInAllianceFactsMessage : GuildFactsMessage
    {
        public new const uint ID =6422;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.BasicNamedAllianceInformations allianceInfos;
        
        public GuildInAllianceFactsMessage()
        {
        }
        
        public GuildInAllianceFactsMessage(Types.GuildFactSheetInformations infos, int creationDate, short nbTaxCollectors, bool enabled, Types.CharacterMinimalInformations[] members, Types.BasicNamedAllianceInformations allianceInfos)
         : base(infos, creationDate, nbTaxCollectors, enabled, members)
        {
            this.allianceInfos = allianceInfos;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            allianceInfos.Serialize(writer);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            allianceInfos = new Types.BasicNamedAllianceInformations();
            allianceInfos.Deserialize(reader);
        }
        
    }
    
}