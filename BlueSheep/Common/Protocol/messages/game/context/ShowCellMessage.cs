









// Generated on 12/11/2014 19:01:27
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ShowCellMessage : Message
    {
        public new const uint ID =5612;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int sourceId;
        public short cellId;
        
        public ShowCellMessage()
        {
        }
        
        public ShowCellMessage(int sourceId, short cellId)
        {
            this.sourceId = sourceId;
            this.cellId = cellId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(sourceId);
            writer.WriteVarShort(cellId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            sourceId = reader.ReadInt();
            cellId = reader.ReadVarShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
        }
        
    }
    
}