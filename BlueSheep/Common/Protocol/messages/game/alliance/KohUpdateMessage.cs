









// Generated on 12/11/2014 19:01:21
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class KohUpdateMessage : Message
    {
        public new const uint ID =6439;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.AllianceInformations[] alliances;
        public short[] allianceNbMembers;
        public int[] allianceRoundWeigth;
        public sbyte[] allianceMatchScore;
        public Types.BasicAllianceInformations allianceMapWinner;
        public int allianceMapWinnerScore;
        public int allianceMapMyAllianceScore;
        public double nextTickTime;
        
        public KohUpdateMessage()
        {
        }
        
        public KohUpdateMessage(Types.AllianceInformations[] alliances, short[] allianceNbMembers, int[] allianceRoundWeigth, sbyte[] allianceMatchScore, Types.BasicAllianceInformations allianceMapWinner, int allianceMapWinnerScore, int allianceMapMyAllianceScore, double nextTickTime)
        {
            this.alliances = alliances;
            this.allianceNbMembers = allianceNbMembers;
            this.allianceRoundWeigth = allianceRoundWeigth;
            this.allianceMatchScore = allianceMatchScore;
            this.allianceMapWinner = allianceMapWinner;
            this.allianceMapWinnerScore = allianceMapWinnerScore;
            this.allianceMapMyAllianceScore = allianceMapMyAllianceScore;
            this.nextTickTime = nextTickTime;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)alliances.Length);
            foreach (var entry in alliances)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)allianceNbMembers.Length);
            foreach (var entry in allianceNbMembers)
            {
                 writer.WriteVarShort(entry);
            }
            writer.WriteUShort((ushort)allianceRoundWeigth.Length);
            foreach (var entry in allianceRoundWeigth)
            {
                 writer.WriteVarInt(entry);
            }
            writer.WriteUShort((ushort)allianceMatchScore.Length);
            foreach (var entry in allianceMatchScore)
            {
                 writer.WriteSByte(entry);
            }
            allianceMapWinner.Serialize(writer);
            writer.WriteVarInt(allianceMapWinnerScore);
            writer.WriteVarInt(allianceMapMyAllianceScore);
            writer.WriteDouble(nextTickTime);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            alliances = new Types.AllianceInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 alliances[i] = new Types.AllianceInformations();
                 alliances[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            allianceNbMembers = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 allianceNbMembers[i] = reader.ReadVarShort();
            }
            limit = reader.ReadUShort();
            allianceRoundWeigth = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 allianceRoundWeigth[i] = reader.ReadVarInt();
            }
            limit = reader.ReadUShort();
            allianceMatchScore = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 allianceMatchScore[i] = reader.ReadSByte();
            }
            allianceMapWinner = new Types.BasicAllianceInformations();
            allianceMapWinner.Deserialize(reader);
            allianceMapWinnerScore = reader.ReadVarInt();
            if (allianceMapWinnerScore < 0)
                throw new Exception("Forbidden value on allianceMapWinnerScore = " + allianceMapWinnerScore + ", it doesn't respect the following condition : allianceMapWinnerScore < 0");
            allianceMapMyAllianceScore = reader.ReadVarInt();
            if (allianceMapMyAllianceScore < 0)
                throw new Exception("Forbidden value on allianceMapMyAllianceScore = " + allianceMapMyAllianceScore + ", it doesn't respect the following condition : allianceMapMyAllianceScore < 0");
            nextTickTime = reader.ReadDouble();
            if (nextTickTime < 0 || nextTickTime > 9.007199254740992E15)
                throw new Exception("Forbidden value on nextTickTime = " + nextTickTime + ", it doesn't respect the following condition : nextTickTime < 0 || nextTickTime > 9.007199254740992E15");
        }
        
    }
    
}