


















// Generated on 12/11/2014 19:02:07
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class MonsterInGroupLightInformations
{

public new const short ID = 395;
public virtual short TypeId
{
    get { return ID; }
}

public int creatureGenericId;
        public sbyte grade;
        

public MonsterInGroupLightInformations()
{
}

public MonsterInGroupLightInformations(int creatureGenericId, sbyte grade)
        {
            this.creatureGenericId = creatureGenericId;
            this.grade = grade;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteInt(creatureGenericId);
            writer.WriteSByte(grade);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

creatureGenericId = reader.ReadInt();
            grade = reader.ReadSByte();
            if (grade < 0)
                throw new Exception("Forbidden value on grade = " + grade + ", it doesn't respect the following condition : grade < 0");
            

}


}


}