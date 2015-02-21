


















// Generated on 12/11/2014 19:02:04
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class FightTeamLightInformations : AbstractFightTeamInformations
{

public new const short ID = 115;
public override short TypeId
{
    get { return ID; }
}

public sbyte teamMembersCount;
        public int meanLevel;
          public bool hasFriend;
      
      public bool hasGuildMember;
      
      public bool hasAllianceMember;
      
      public bool hasGroupMember;
      
      public bool hasMyTaxCollector;
        

public FightTeamLightInformations()
{
}

public FightTeamLightInformations(sbyte teamId, int leaderId, sbyte teamSide, sbyte teamTypeId, sbyte nbWaves, sbyte teamMembersCount, int meanLevel)
         : base(teamId, leaderId, teamSide, teamTypeId, nbWaves)
        {
            this.teamMembersCount = teamMembersCount;
            this.meanLevel = meanLevel;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
   byte _loc2_ = 0;
         _loc2_ = BooleanByteWrapper.SetFlag(_loc2_,0,this.hasFriend);
         _loc2_ = BooleanByteWrapper.SetFlag(_loc2_,1,this.hasGuildMember);
         _loc2_ = BooleanByteWrapper.SetFlag(_loc2_,2,this.hasAllianceMember);
         _loc2_ = BooleanByteWrapper.SetFlag(_loc2_,3,this.hasGroupMember);
         _loc2_ = BooleanByteWrapper.SetFlag(_loc2_,4,this.hasMyTaxCollector);
         writer.WriteByte(_loc2_);
            writer.WriteSByte(teamMembersCount);
            writer.WriteVarInt(meanLevel);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
    byte _loc2_ = reader.ReadByte();
         this.hasFriend = BooleanByteWrapper.GetFlag(_loc2_,0);
         this.hasGuildMember = BooleanByteWrapper.GetFlag(_loc2_,1);
         this.hasAllianceMember = BooleanByteWrapper.GetFlag(_loc2_,2);
         this.hasGroupMember = BooleanByteWrapper.GetFlag(_loc2_,3);
         this.hasMyTaxCollector = BooleanByteWrapper.GetFlag(_loc2_,4);
            teamMembersCount = reader.ReadSByte();
            if (teamMembersCount < 0)
                throw new Exception("Forbidden value on teamMembersCount = " + teamMembersCount + ", it doesn't respect the following condition : teamMembersCount < 0");
            meanLevel = reader.ReadVarInt();
            if (meanLevel < 0)
                throw new Exception("Forbidden value on meanLevel = " + meanLevel + ", it doesn't respect the following condition : meanLevel < 0");
            

}


}


}