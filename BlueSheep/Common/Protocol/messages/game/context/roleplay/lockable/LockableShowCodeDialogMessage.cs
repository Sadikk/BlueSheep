









// Generated on 12/11/2014 19:01:35
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class LockableShowCodeDialogMessage : Message
    {
        public new const uint ID =5740;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool changeOrUse;
        public sbyte codeSize;
        
        public LockableShowCodeDialogMessage()
        {
        }
        
        public LockableShowCodeDialogMessage(bool changeOrUse, sbyte codeSize)
        {
            this.changeOrUse = changeOrUse;
            this.codeSize = codeSize;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(changeOrUse);
            writer.WriteSByte(codeSize);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            changeOrUse = reader.ReadBoolean();
            codeSize = reader.ReadSByte();
            if (codeSize < 0)
                throw new Exception("Forbidden value on codeSize = " + codeSize + ", it doesn't respect the following condition : codeSize < 0");
        }
        
    }
    
}