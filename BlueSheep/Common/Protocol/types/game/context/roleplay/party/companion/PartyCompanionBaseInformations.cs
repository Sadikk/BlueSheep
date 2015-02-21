


















// Generated on 12/11/2014 19:02:08
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class PartyCompanionBaseInformations
{

public new const short ID = 453;
public virtual short TypeId
{
    get { return ID; }
}

public sbyte indexId;
        public sbyte companionGenericId;
        public Types.EntityLook entityLook;
        

public PartyCompanionBaseInformations()
{
}

public PartyCompanionBaseInformations(sbyte indexId, sbyte companionGenericId, Types.EntityLook entityLook)
        {
            this.indexId = indexId;
            this.companionGenericId = companionGenericId;
            this.entityLook = entityLook;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteSByte(indexId);
            writer.WriteSByte(companionGenericId);
            entityLook.Serialize(writer);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

indexId = reader.ReadSByte();
            if (indexId < 0)
                throw new Exception("Forbidden value on indexId = " + indexId + ", it doesn't respect the following condition : indexId < 0");
            companionGenericId = reader.ReadSByte();
            if (companionGenericId < 0)
                throw new Exception("Forbidden value on companionGenericId = " + companionGenericId + ", it doesn't respect the following condition : companionGenericId < 0");
            entityLook = new Types.EntityLook();
            entityLook.Deserialize(reader);
            

}


}


}