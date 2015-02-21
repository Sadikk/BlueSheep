


















// Generated on 12/11/2014 19:02:06
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class GameRolePlayGroupMonsterWaveInformations : GameRolePlayGroupMonsterInformations
{

public new const short ID = 464;
public override short TypeId
{
    get { return ID; }
}

public sbyte nbWaves;
        public Types.GroupMonsterStaticInformations[] alternatives;
        

public GameRolePlayGroupMonsterWaveInformations()
{
}

public GameRolePlayGroupMonsterWaveInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, Types.GroupMonsterStaticInformations staticInfos, short ageBonus, sbyte lootShare, sbyte alignmentSide, sbyte nbWaves, Types.GroupMonsterStaticInformations[] alternatives)
         : base(contextualId, look, disposition, staticInfos, ageBonus, lootShare, alignmentSide)
        {
            this.nbWaves = nbWaves;
            this.alternatives = alternatives;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteSByte(nbWaves);
            writer.WriteUShort((ushort)alternatives.Length);
            foreach (var entry in alternatives)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            nbWaves = reader.ReadSByte();
            if (nbWaves < 0)
                throw new Exception("Forbidden value on nbWaves = " + nbWaves + ", it doesn't respect the following condition : nbWaves < 0");
            var limit = reader.ReadUShort();
            alternatives = new Types.GroupMonsterStaticInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 alternatives[i] = Types.ProtocolTypeManager.GetInstance<Types.GroupMonsterStaticInformations>(reader.ReadShort());
                 alternatives[i].Deserialize(reader);
            }
            

}


}


}