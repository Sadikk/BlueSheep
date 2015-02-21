


















// Generated on 12/11/2014 19:02:11
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ShortcutObject : Shortcut
{

public new const short ID = 367;
public override short TypeId
{
    get { return ID; }
}



public ShortcutObject()
{
}

public ShortcutObject(sbyte slot)
         : base(slot)
        {
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            

}


}


}