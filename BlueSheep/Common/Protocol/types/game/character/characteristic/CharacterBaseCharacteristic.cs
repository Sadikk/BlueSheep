


















// Generated on 12/11/2014 19:02:03
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class CharacterBaseCharacteristic
{

public new const short ID = 4;
public virtual short TypeId
{
    get { return ID; }
}

public short @base;
        public short additionnal;
        public short objectsAndMountBonus;
        public short alignGiftBonus;
        public short contextModif;
        

public CharacterBaseCharacteristic()
{
}

public CharacterBaseCharacteristic(short @base, short additionnal, short objectsAndMountBonus, short alignGiftBonus, short contextModif)
        {
            this.@base = @base;
            this.additionnal = additionnal;
            this.objectsAndMountBonus = objectsAndMountBonus;
            this.alignGiftBonus = alignGiftBonus;
            this.contextModif = contextModif;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarShort(@base);
            writer.WriteVarShort(additionnal);
            writer.WriteVarShort(objectsAndMountBonus);
            writer.WriteVarShort(alignGiftBonus);
            writer.WriteVarShort(contextModif);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

@base = reader.ReadVarShort();
            additionnal = reader.ReadVarShort();
            objectsAndMountBonus = reader.ReadVarShort();
            alignGiftBonus = reader.ReadVarShort();
            contextModif = reader.ReadVarShort();
            

}


}


}