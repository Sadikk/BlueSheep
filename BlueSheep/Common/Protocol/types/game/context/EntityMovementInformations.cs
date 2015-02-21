


















// Generated on 12/11/2014 19:02:03
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class EntityMovementInformations
{

public new const short ID = 63;
public virtual short TypeId
{
    get { return ID; }
}

public int id;
        public sbyte[] steps;
        

public EntityMovementInformations()
{
}

public EntityMovementInformations(int id, sbyte[] steps)
        {
            this.id = id;
            this.steps = steps;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteInt(id);
            writer.WriteUShort((ushort)steps.Length);
            foreach (var entry in steps)
            {
                 writer.WriteSByte(entry);
            }
            

}

public virtual void Deserialize(BigEndianReader reader)
{

id = reader.ReadInt();
            var limit = reader.ReadUShort();
            steps = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 steps[i] = reader.ReadSByte();
            }
            

}


}


}