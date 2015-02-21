


















// Generated on 12/11/2014 19:02:12
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class AllianceVersatileInformations
{

public new const short ID = 432;
public virtual short TypeId
{
    get { return ID; }
}

public int allianceId;
        public short nbGuilds;
        public short nbMembers;
        public short nbSubarea;
        

public AllianceVersatileInformations()
{
}

public AllianceVersatileInformations(int allianceId, short nbGuilds, short nbMembers, short nbSubarea)
        {
            this.allianceId = allianceId;
            this.nbGuilds = nbGuilds;
            this.nbMembers = nbMembers;
            this.nbSubarea = nbSubarea;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarInt(allianceId);
            writer.WriteVarShort(nbGuilds);
            writer.WriteVarShort(nbMembers);
            writer.WriteVarShort(nbSubarea);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

allianceId = reader.ReadVarInt();
            if (allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + allianceId + ", it doesn't respect the following condition : allianceId < 0");
            nbGuilds = reader.ReadVarShort();
            if (nbGuilds < 0)
                throw new Exception("Forbidden value on nbGuilds = " + nbGuilds + ", it doesn't respect the following condition : nbGuilds < 0");
            nbMembers = reader.ReadVarShort();
            if (nbMembers < 0)
                throw new Exception("Forbidden value on nbMembers = " + nbMembers + ", it doesn't respect the following condition : nbMembers < 0");
            nbSubarea = reader.ReadVarShort();
            if (nbSubarea < 0)
                throw new Exception("Forbidden value on nbSubarea = " + nbSubarea + ", it doesn't respect the following condition : nbSubarea < 0");
            

}


}


}