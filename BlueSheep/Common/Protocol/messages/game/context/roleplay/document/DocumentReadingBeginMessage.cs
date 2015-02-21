









// Generated on 12/11/2014 19:01:32
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class DocumentReadingBeginMessage : Message
    {
        public new const uint ID =5675;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short documentId;
        
        public DocumentReadingBeginMessage()
        {
        }
        
        public DocumentReadingBeginMessage(short documentId)
        {
            this.documentId = documentId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(documentId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            documentId = reader.ReadVarShort();
            if (documentId < 0)
                throw new Exception("Forbidden value on documentId = " + documentId + ", it doesn't respect the following condition : documentId < 0");
        }
        
    }
    
}