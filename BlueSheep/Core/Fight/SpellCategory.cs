using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Core.Fight
{
    public enum SpellCategory
    {
        Healing = 0x0001,
        Teleport = 0x0002,
        Invocation = 0x0004,
        Buff = 0x0008,
        DamagesWater = 0x0010,
        DamagesEarth = 0x0020,
        DamagesAir = 0x0040,
        DamagesFire = 0x0080,
        DamagesNeutral = 0x0100,
        Curse = 0x0200,
        Damages = DamagesNeutral | DamagesFire | DamagesAir | DamagesEarth | DamagesWater,
        None = 0,
        All = 0x01FF,
    }
}
