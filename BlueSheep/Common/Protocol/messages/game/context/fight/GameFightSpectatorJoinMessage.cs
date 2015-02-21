









// Generated on 12/11/2014 19:01:28
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightSpectatorJoinMessage : GameFightJoinMessage
    {
        public new const uint ID =6504;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.NamedPartyTeam[] namedPartyTeams;
        
        public GameFightSpectatorJoinMessage()
        {
        }
        
        public GameFightSpectatorJoinMessage(bool canBeCancelled, bool canSayReady, bool isFightStarted, int timeMaxBeforeFightStart, sbyte fightType, Types.NamedPartyTeam[] namedPartyTeams)
         : base(canBeCancelled, canSayReady, isFightStarted, timeMaxBeforeFightStart, fightType)
        {
            this.namedPartyTeams = namedPartyTeams;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)namedPartyTeams.Length);
            foreach (var entry in namedPartyTeams)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            namedPartyTeams = new Types.NamedPartyTeam[limit];
            for (int i = 0; i < limit; i++)
            {
                 namedPartyTeams[i] = new Types.NamedPartyTeam();
                 namedPartyTeams[i].Deserialize(reader);
            }
        }
        
    }
    
}