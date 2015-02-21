


















// Generated on 12/11/2014 19:02:03
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class FightEntityDispositionInformations : EntityDispositionInformations
{

public new const short ID = 217;
public override short TypeId
{
    get { return ID; }
}

public int carryingCharacterId;
        

public FightEntityDispositionInformations()
{
}

public FightEntityDispositionInformations(short cellId, sbyte direction, int carryingCharacterId)
         : base(cellId, direction)
        {
            this.carryingCharacterId = carryingCharacterId;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteInt(carryingCharacterId);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            carryingCharacterId = reader.ReadInt();
            

}


}


}