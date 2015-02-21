


















// Generated on 12/11/2014 19:02:04
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class FightOptionsInformations
{

public new const short ID = 20;
public virtual short TypeId
{
    get { return ID; }
}

          public bool isSecret;  
      public bool isRestrictedToPartyOnly;      
     public bool isClosed;     
      public bool isAskingForHelp;


public FightOptionsInformations()
{
}



public virtual void Serialize(BigEndianWriter writer)
{
   byte _loc2_ = 0;
         _loc2_ = BooleanByteWrapper.SetFlag(_loc2_,0,this.isSecret);
         _loc2_ = BooleanByteWrapper.SetFlag(_loc2_,1,this.isRestrictedToPartyOnly);
         _loc2_ = BooleanByteWrapper.SetFlag(_loc2_,2,this.isClosed);
         _loc2_ = BooleanByteWrapper.SetFlag(_loc2_,3,this.isAskingForHelp);
         writer.WriteByte(_loc2_);


}

public virtual void Deserialize(BigEndianReader reader)
{

    byte _loc2_ = reader.ReadByte();
         this.isSecret = BooleanByteWrapper.GetFlag(_loc2_,0);
         this.isRestrictedToPartyOnly = BooleanByteWrapper.GetFlag(_loc2_,1);
         this.isClosed = BooleanByteWrapper.GetFlag(_loc2_,2);
         this.isAskingForHelp = BooleanByteWrapper.GetFlag(_loc2_,3);

}


}


}