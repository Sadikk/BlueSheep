using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Core.Fight
{
    /// <summary>
    /// Enum of condition that can be used in AI scripts.
    /// </summary>
    public enum FightConditionEnum
    {
        Null, // None
        MonstersNumber, // Number of monsters
        BossInFight,    // If a boss is in the fight or not
        DistanceFromTarget, // Distance from the target
        Turn,           // Current turn
        AvailableAP,    // Current AP
        AvailablePM,    // Current MP
        PlayerLife,     // Life of the current player
        TargetLife,     // Life of the target
        TargetResAir,   // %res air of the target
        TargetResWater, ///////////////////////////
        TargetResFire,  ///////////////////////////
        TargetResEarth, ///////////////////////////
        TargetResNeutral //////////////////////////
    }
}
