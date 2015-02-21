









// Generated on 12/11/2014 19:01:19
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class SequenceEndMessage : Message
    {
        public new const uint ID =956;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short actionId;
        public int authorId;
        public sbyte sequenceType;
        
        public SequenceEndMessage()
        {
        }
        
        public SequenceEndMessage(short actionId, int authorId, sbyte sequenceType)
        {
            this.actionId = actionId;
            this.authorId = authorId;
            this.sequenceType = sequenceType;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(actionId);
            writer.WriteInt(authorId);
            writer.WriteSByte(sequenceType);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            actionId = reader.ReadVarShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            authorId = reader.ReadInt();
            sequenceType = reader.ReadSByte();
        }
        
    }
    
}