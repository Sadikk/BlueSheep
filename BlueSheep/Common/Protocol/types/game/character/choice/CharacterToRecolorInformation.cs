


















// Generated on 12/11/2014 19:02:03
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class CharacterToRecolorInformation : AbstractCharacterToRefurbishInformation
{

public new const short ID = 212;
public override short TypeId
{
    get { return ID; }
}



public CharacterToRecolorInformation()
{
}

public CharacterToRecolorInformation(int id, int[] colors, int cosmeticId)
         : base(id, colors, cosmeticId)
        {
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            

}


}


}