


















// Generated on 12/11/2014 19:02:08
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class PortalInformation
{

public new const short ID = 466;
public virtual short TypeId
{
    get { return ID; }
}

public short portalId;
        public short areaId;
        

public PortalInformation()
{
}

public PortalInformation(short portalId, short areaId)
        {
            this.portalId = portalId;
            this.areaId = areaId;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarShort(portalId);
            writer.WriteShort(areaId);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

portalId = reader.ReadVarShort();
            if (portalId < 0)
                throw new Exception("Forbidden value on portalId = " + portalId + ", it doesn't respect the following condition : portalId < 0");
            areaId = reader.ReadShort();
            

}


}


}