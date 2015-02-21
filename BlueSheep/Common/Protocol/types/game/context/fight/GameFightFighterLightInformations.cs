


















// Generated on 12/11/2014 19:02:05
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class GameFightFighterLightInformations
{

public new const short ID = 413;
public virtual short TypeId
{
    get { return ID; }
}

public int id;
        public sbyte wave;
        public short level;
        public sbyte breed;
          public bool sex;
      
      public bool alive;
        

public GameFightFighterLightInformations()
{
}

public GameFightFighterLightInformations(int id, sbyte wave, short level, sbyte breed)
        {
            this.id = id;
            this.wave = wave;
            this.level = level;
            this.breed = breed;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{
    byte b = 0;
         b = BooleanByteWrapper.SetFlag(b,0,this.sex);
         b = BooleanByteWrapper.SetFlag(b,1,this.alive);
         writer.WriteByte(b);
writer.WriteInt(id);
            writer.WriteSByte(wave);
            writer.WriteVarShort(level);
            writer.WriteSByte(breed);
            

}

public virtual void Deserialize(BigEndianReader reader)
{
   byte _loc2_ = reader.ReadByte();
         this.sex = BooleanByteWrapper.GetFlag(_loc2_,0);
         this.alive = BooleanByteWrapper.GetFlag(_loc2_,1);
id = reader.ReadInt();
            wave = reader.ReadSByte();
            if (wave < 0)
                throw new Exception("Forbidden value on wave = " + wave + ", it doesn't respect the following condition : wave < 0");
            level = reader.ReadVarShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            breed = reader.ReadSByte();
            

}


}


}