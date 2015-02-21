


















// Generated on 12/11/2014 19:02:12
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class AllianceFactSheetInformations : AllianceInformations
{

public new const short ID = 421;
public override short TypeId
{
    get { return ID; }
}

public int creationDate;
        

public AllianceFactSheetInformations()
{
}

public AllianceFactSheetInformations(int allianceId, string allianceTag, string allianceName, Types.GuildEmblem allianceEmblem, int creationDate)
         : base(allianceId, allianceTag, allianceName, allianceEmblem)
        {
            this.creationDate = creationDate;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteInt(creationDate);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            creationDate = reader.ReadInt();
            if (creationDate < 0)
                throw new Exception("Forbidden value on creationDate = " + creationDate + ", it doesn't respect the following condition : creationDate < 0");
            

}


}


}