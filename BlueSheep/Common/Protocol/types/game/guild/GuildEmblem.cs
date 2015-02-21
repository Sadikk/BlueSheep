


















// Generated on 12/11/2014 19:02:10
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class GuildEmblem
{

public new const short ID = 87;
public virtual short TypeId
{
    get { return ID; }
}

public short symbolShape;
        public int symbolColor;
        public sbyte backgroundShape;
        public int backgroundColor;
        

public GuildEmblem()
{
}

public GuildEmblem(short symbolShape, int symbolColor, sbyte backgroundShape, int backgroundColor)
        {
            this.symbolShape = symbolShape;
            this.symbolColor = symbolColor;
            this.backgroundShape = backgroundShape;
            this.backgroundColor = backgroundColor;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteVarShort(symbolShape);
            writer.WriteInt(symbolColor);
            writer.WriteSByte(backgroundShape);
            writer.WriteInt(backgroundColor);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

symbolShape = reader.ReadVarShort();
            if (symbolShape < 0)
                throw new Exception("Forbidden value on symbolShape = " + symbolShape + ", it doesn't respect the following condition : symbolShape < 0");
            symbolColor = reader.ReadInt();
            backgroundShape = reader.ReadSByte();
            if (backgroundShape < 0)
                throw new Exception("Forbidden value on backgroundShape = " + backgroundShape + ", it doesn't respect the following condition : backgroundShape < 0");
            backgroundColor = reader.ReadInt();
            

}


}


}