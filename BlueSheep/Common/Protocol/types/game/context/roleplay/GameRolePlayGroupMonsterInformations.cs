


















// Generated on 12/11/2014 19:02:06
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class GameRolePlayGroupMonsterInformations : GameRolePlayActorInformations
{

public new const short ID = 160;
public override short TypeId
{
    get { return ID; }
}

public Types.GroupMonsterStaticInformations staticInfos;
        public short ageBonus;
        public sbyte lootShare;
        public sbyte alignmentSide;
        public bool keyRingBonus;
        public bool hasHardcoreDrop;
        public bool hasAVARewardToken;
        

public GameRolePlayGroupMonsterInformations()
{
}

public GameRolePlayGroupMonsterInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, Types.GroupMonsterStaticInformations staticInfos, short ageBonus, sbyte lootShare, sbyte alignmentSide)
         : base(contextualId, look, disposition)
        {
            this.staticInfos = staticInfos;
            this.ageBonus = ageBonus;
            this.lootShare = lootShare;
            this.alignmentSide = alignmentSide;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteShort(staticInfos.TypeId);
            staticInfos.Serialize(writer);
            writer.WriteShort(ageBonus);
            writer.WriteSByte(lootShare);
            writer.WriteSByte(alignmentSide);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
byte b = reader.ReadByte();
this.keyRingBonus = BooleanByteWrapper.GetFlag(b, 0);
this.hasHardcoreDrop = BooleanByteWrapper.GetFlag(b, 1);
this.hasAVARewardToken = BooleanByteWrapper.GetFlag(b, 2);
short s = reader.ReadShort();
            staticInfos = Types.ProtocolTypeManager.GetInstance<Types.GroupMonsterStaticInformations>(s);
            staticInfos.Deserialize(reader);
            ageBonus = reader.ReadShort();
            if (ageBonus < -1 || ageBonus > 1000)
                throw new Exception("Forbidden value on ageBonus = " + ageBonus + ", it doesn't respect the following condition : ageBonus < -1 || ageBonus > 1000");
            lootShare = reader.ReadSByte();
            if (lootShare < -1 || lootShare > 8)
                throw new Exception("Forbidden value on lootShare = " + lootShare + ", it doesn't respect the following condition : lootShare < -1 || lootShare > 8");
            alignmentSide = reader.ReadSByte();
            

}


}


}