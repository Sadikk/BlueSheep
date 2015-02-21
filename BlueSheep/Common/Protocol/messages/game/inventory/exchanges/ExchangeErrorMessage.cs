









// Generated on 12/11/2014 19:01:49
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class ExchangeErrorMessage : Message
    {
        public new const uint ID =5513;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte errorType;
        
        public ExchangeErrorMessage()
        {
        }
        
        public ExchangeErrorMessage(sbyte errorType)
        {
            this.errorType = errorType;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(errorType);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            errorType = reader.ReadSByte();
        }
        
    }
    
}