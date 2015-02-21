


















// Generated on 12/11/2014 19:02:11
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ShortcutSmiley : Shortcut
{

public new const short ID = 388;
public override short TypeId
{
    get { return ID; }
}

public sbyte smileyId;
        

public ShortcutSmiley()
{
}

public ShortcutSmiley(sbyte slot, sbyte smileyId)
         : base(slot)
        {
            this.smileyId = smileyId;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteSByte(smileyId);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            smileyId = reader.ReadSByte();
            if (smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + smileyId + ", it doesn't respect the following condition : smileyId < 0");
            

}


}


}