


















// Generated on 12/11/2014 19:02:02
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ServerSessionConstantString : ServerSessionConstant
{

public new const short ID = 436;
public override short TypeId
{
    get { return ID; }
}

public string value;
        

public ServerSessionConstantString()
{
}

public ServerSessionConstantString(short id, string value)
         : base(id)
        {
            this.value = value;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteUTF(value);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            value = reader.ReadUTF();
            

}


}


}