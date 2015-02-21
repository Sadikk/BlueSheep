


















// Generated on 12/11/2014 19:02:07
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class HumanOptionTitle : HumanOption
{

public new const short ID = 408;
public override short TypeId
{
    get { return ID; }
}

public short titleId;
        public string titleParam;
        

public HumanOptionTitle()
{
}

public HumanOptionTitle(short titleId, string titleParam)
        {
            this.titleId = titleId;
            this.titleParam = titleParam;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarShort(titleId);
            writer.WriteUTF(titleParam);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            titleId = reader.ReadVarShort();
            if (titleId < 0)
                throw new Exception("Forbidden value on titleId = " + titleId + ", it doesn't respect the following condition : titleId < 0");
            titleParam = reader.ReadUTF();
            

}


}


}