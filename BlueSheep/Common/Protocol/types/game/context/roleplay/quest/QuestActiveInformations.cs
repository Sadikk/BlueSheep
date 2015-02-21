


















// Generated on 12/11/2014 19:02:08
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class QuestActiveInformations
{

public new const short ID = 381;
public virtual short TypeId
{
    get { return ID; }
}

public short questId;
        

public QuestActiveInformations()
{
}

public QuestActiveInformations(short questId)
        {
            this.questId = questId;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarShort(questId);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

questId = reader.ReadVarShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
            

}


}


}