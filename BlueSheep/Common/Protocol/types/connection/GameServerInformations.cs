


















// Generated on 12/11/2014 19:02:02
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class GameServerInformations
{

public new const short ID = 25;
public virtual short TypeId
{
    get { return ID; }
}

        public ushort id;
        public sbyte status;
        public sbyte completion;
        public bool isSelectable;
        public sbyte charactersCount;
        public double date;
        

public GameServerInformations()
{
}

public GameServerInformations(ushort id, sbyte status, sbyte completion, bool isSelectable, sbyte charactersCount, double date)
        {
            this.id = id;
            this.status = status;
            this.completion = completion;
            this.isSelectable = isSelectable;
            this.charactersCount = charactersCount;
            this.date = date;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

    writer.WriteUShort(id);
    writer.WriteSByte(status);
    writer.WriteSByte(completion);
    writer.WriteBoolean(isSelectable);
    writer.WriteSByte(charactersCount);
    writer.WriteDouble(date);
           
}

public virtual void Deserialize(BigEndianReader reader)
{

            id = reader.ReadUShort();
            if (id < 0 || id > 65535)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0 || id > 65535");
            status = reader.ReadSByte();
            if (status < 0)
                throw new Exception("Forbidden value on status = " + status + ", it doesn't respect the following condition : status < 0");
            completion = reader.ReadSByte();
            if (completion < 0)
                throw new Exception("Forbidden value on completion = " + completion + ", it doesn't respect the following condition : completion < 0");
            isSelectable = reader.ReadBoolean();
            charactersCount = reader.ReadSByte();
            if (charactersCount < 0)
                throw new Exception("Forbidden value on charactersCount = " + charactersCount + ", it doesn't respect the following condition : charactersCount < 0");
            date = reader.ReadDouble();
            if (date < -9.007199254740992E15 || date > 9.007199254740992E15)
                throw new Exception("Forbidden value on date = " + date + ", it doesn't respect the following condition : date < -9.007199254740992E15 || date > 9.007199254740992E15");
            

}


}


}