


















// Generated on 12/11/2014 19:02:03
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class ActorRestrictionsInformations
{

public new const short ID = 204;
public virtual short TypeId
{
    get { return ID; }
}

            public bool cantBeAggressed= false;
        public bool cantBeChallenged= false;
        public bool cantTrade= false;
        public bool cantBeAttackedByMutant= false;
        public bool cantRun= false;
        public bool forceSlowWalk= false;
        public bool cantMinimize= false;
        public bool cantMove= false;
        public bool cantAggress= false;
        public bool cantChallenge= false;
        public bool cantExchange= false;
        public bool cantAttack= false;
        public bool cantChat= false;
        public bool cantBeMerchant= false;
        public bool cantUseObject= false;
        public bool cantUseTaxCollector= false;
        public bool cantUseInteractive= false;
        public bool cantSpeakToNPC= false;
        public bool cantChangeZone= false;
        public bool cantAttackMonster= false;
        public bool cantWalk8Directions= false;

public ActorRestrictionsInformations()
{
}



public virtual void Serialize(BigEndianWriter writer)
{



}

public virtual void Deserialize(BigEndianReader reader)
{

    var _loc_2 = reader.ReadByte();
            this.cantBeAggressed = BooleanByteWrapper.GetFlag(_loc_2, 0);
            this.cantBeChallenged = BooleanByteWrapper.GetFlag(_loc_2, 1);
            this.cantTrade = BooleanByteWrapper.GetFlag(_loc_2, 2);
            this.cantBeAttackedByMutant = BooleanByteWrapper.GetFlag(_loc_2, 3);
            this.cantRun = BooleanByteWrapper.GetFlag(_loc_2, 4);
            this.forceSlowWalk = BooleanByteWrapper.GetFlag(_loc_2, 5);
            this.cantMinimize = BooleanByteWrapper.GetFlag(_loc_2, 6);
            this.cantMove = BooleanByteWrapper.GetFlag(_loc_2, 7);
            var _loc_3 = reader.ReadByte();
            this.cantAggress = BooleanByteWrapper.GetFlag(_loc_3, 0);
            this.cantChallenge = BooleanByteWrapper.GetFlag(_loc_3, 1);
            this.cantExchange = BooleanByteWrapper.GetFlag(_loc_3, 2);
            this.cantAttack = BooleanByteWrapper.GetFlag(_loc_3, 3);
            this.cantChat = BooleanByteWrapper.GetFlag(_loc_3, 4);
            this.cantBeMerchant = BooleanByteWrapper.GetFlag(_loc_3, 5);
            this.cantUseObject = BooleanByteWrapper.GetFlag(_loc_3, 6);
            this.cantUseTaxCollector = BooleanByteWrapper.GetFlag(_loc_3, 7);
            var _loc_4 = reader.ReadByte();
            this.cantUseInteractive = BooleanByteWrapper.GetFlag(_loc_4, 0);
            this.cantSpeakToNPC = BooleanByteWrapper.GetFlag(_loc_4, 1);
            this.cantChangeZone = BooleanByteWrapper.GetFlag(_loc_4, 2);
            this.cantAttackMonster = BooleanByteWrapper.GetFlag(_loc_4, 3);
            this.cantWalk8Directions = BooleanByteWrapper.GetFlag(_loc_4, 4);

}


}


}