


















// Generated on 12/11/2014 19:02:10
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class InteractiveElementNamedSkill : InteractiveElementSkill
{

public new const short ID = 220;
public override short TypeId
{
    get { return ID; }
}

public int nameId;
        

public InteractiveElementNamedSkill()
{
}

public InteractiveElementNamedSkill(int skillId, int skillInstanceUid, int nameId)
         : base(skillId, skillInstanceUid)
        {
            this.nameId = nameId;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarInt(nameId);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            nameId = reader.ReadVarInt();
            if (nameId < 0)
                throw new Exception("Forbidden value on nameId = " + nameId + ", it doesn't respect the following condition : nameId < 0");
            

}


}


}