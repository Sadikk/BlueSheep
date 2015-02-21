


















// Generated on 12/11/2014 19:02:11
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ShortcutSpell : Shortcut
{

public new const short ID = 368;
public override short TypeId
{
    get { return ID; }
}

public short spellId;
        

public ShortcutSpell()
{
}

public ShortcutSpell(sbyte slot, short spellId)
         : base(slot)
        {
            this.spellId = spellId;
        }
        

public override void Serialize(BigEndianWriter writer)
{

base.Serialize(writer);
            writer.WriteVarShort(spellId);
            

}

public override void Deserialize(BigEndianReader reader)
{

base.Deserialize(reader);
            spellId = reader.ReadVarShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            

}


}


}