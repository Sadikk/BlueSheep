


















// Generated on 12/11/2014 19:02:04
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class IdentifiedEntityDispositionInformations : EntityDispositionInformations
{

public new const short ID = 107;
public override short TypeId
{
    get { return ID; }
}

public int id;
        

public IdentifiedEntityDispositionInformations()
{
}

public IdentifiedEntityDispositionInformations(short cellId, sbyte direction, int id)
         : base(cellId, direction)
        {
            this.id = id;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteInt(id);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            id = reader.ReadInt();
            

}


}


}