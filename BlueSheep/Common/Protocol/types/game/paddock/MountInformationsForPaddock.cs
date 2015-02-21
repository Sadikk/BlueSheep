


















// Generated on 12/11/2014 19:02:11
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class MountInformationsForPaddock
{

public new const short ID = 184;
public virtual short TypeId
{
    get { return ID; }
}

public sbyte modelId;
        public string name;
        public string ownerName;
        

public MountInformationsForPaddock()
{
}

public MountInformationsForPaddock(sbyte modelId, string name, string ownerName)
        {
            this.modelId = modelId;
            this.name = name;
            this.ownerName = ownerName;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteSByte(modelId);
            writer.WriteUTF(name);
            writer.WriteUTF(ownerName);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

modelId = reader.ReadSByte();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
            name = reader.ReadUTF();
            ownerName = reader.ReadUTF();
            

}


}


}