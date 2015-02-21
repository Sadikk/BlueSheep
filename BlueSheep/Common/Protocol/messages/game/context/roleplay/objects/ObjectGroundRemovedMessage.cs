









// Generated on 12/11/2014 19:01:36
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ObjectGroundRemovedMessage : Message
    {
        public new const uint ID =3014;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short cell;
        
        public ObjectGroundRemovedMessage()
        {
        }
        
        public ObjectGroundRemovedMessage(short cell)
        {
            this.cell = cell;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(cell);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            cell = reader.ReadVarShort();
            if (cell < 0 || cell > 559)
                throw new Exception("Forbidden value on cell = " + cell + ", it doesn't respect the following condition : cell < 0 || cell > 559");
        }
        
    }
    
}