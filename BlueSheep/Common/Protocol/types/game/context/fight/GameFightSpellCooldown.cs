


















// Generated on 12/11/2014 19:02:05
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class GameFightSpellCooldown
{

public new const short ID = 205;
public virtual short TypeId
{
    get { return ID; }
}

public int spellId;
        public sbyte cooldown;
        

public GameFightSpellCooldown()
{
}

public GameFightSpellCooldown(int spellId, sbyte cooldown)
        {
            this.spellId = spellId;
            this.cooldown = cooldown;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteInt(spellId);
            writer.WriteSByte(cooldown);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

spellId = reader.ReadInt();
            cooldown = reader.ReadSByte();
            if (cooldown < 0)
                throw new Exception("Forbidden value on cooldown = " + cooldown + ", it doesn't respect the following condition : cooldown < 0");
            

}


}


}