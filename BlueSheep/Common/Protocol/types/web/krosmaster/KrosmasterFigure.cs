


















// Generated on 12/11/2014 19:02:12
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class KrosmasterFigure
{

public new const short ID = 397;
public virtual short TypeId
{
    get { return ID; }
}

public string uid;
        public short figure;
        public short pedestal;
        public bool bound;
        

public KrosmasterFigure()
{
}

public KrosmasterFigure(string uid, short figure, short pedestal, bool bound)
        {
            this.uid = uid;
            this.figure = figure;
            this.pedestal = pedestal;
            this.bound = bound;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteUTF(uid);
            writer.WriteVarShort(figure);
            writer.WriteVarShort(pedestal);
            writer.WriteBoolean(bound);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

uid = reader.ReadUTF();
            figure = reader.ReadVarShort();
            if (figure < 0)
                throw new Exception("Forbidden value on figure = " + figure + ", it doesn't respect the following condition : figure < 0");
            pedestal = reader.ReadVarShort();
            if (pedestal < 0)
                throw new Exception("Forbidden value on pedestal = " + pedestal + ", it doesn't respect the following condition : pedestal < 0");
            bound = reader.ReadBoolean();
            

}


}


}