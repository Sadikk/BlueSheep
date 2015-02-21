


















// Generated on 12/11/2014 19:02:04
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class FightTeamMemberCompanionInformations : FightTeamMemberInformations
{

public new const short ID = 451;
public override short TypeId
{
    get { return ID; }
}

public sbyte companionId;
        public byte level;
        public int masterId;
        

public FightTeamMemberCompanionInformations()
{
}

public FightTeamMemberCompanionInformations(int id, sbyte companionId, byte level, int masterId)
         : base(id)
        {
            this.companionId = companionId;
            this.level = level;
            this.masterId = masterId;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteSByte(companionId);
            writer.WriteByte(level);
            writer.WriteInt(masterId);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            companionId = reader.ReadSByte();
            if (companionId < 0)
                throw new Exception("Forbidden value on companionId = " + companionId + ", it doesn't respect the following condition : companionId < 0");
            level = reader.ReadByte();
            if (level < 1 || level > 200)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 1 || level > 200");
            masterId = reader.ReadInt();
            

}


}


}