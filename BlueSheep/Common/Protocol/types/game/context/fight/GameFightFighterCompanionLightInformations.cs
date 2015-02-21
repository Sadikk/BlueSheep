


















// Generated on 12/11/2014 19:02:05
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class GameFightFighterCompanionLightInformations : GameFightFighterLightInformations
{

public new const short ID = 454;
public override short TypeId
{
    get { return ID; }
}

public sbyte companionId;
        public int masterId;
        

public GameFightFighterCompanionLightInformations()
{
}

public GameFightFighterCompanionLightInformations(int id, sbyte wave, short level, sbyte breed, sbyte companionId, int masterId)
         : base(id, wave, level, breed)
        {
            this.companionId = companionId;
            this.masterId = masterId;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteSByte(companionId);
            writer.WriteInt(masterId);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            companionId = reader.ReadSByte();
            if (companionId < 0)
                throw new Exception("Forbidden value on companionId = " + companionId + ", it doesn't respect the following condition : companionId < 0");
            masterId = reader.ReadInt();
            

}


}


}