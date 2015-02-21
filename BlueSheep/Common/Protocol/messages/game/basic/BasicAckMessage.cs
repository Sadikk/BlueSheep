









// Generated on 12/11/2014 19:01:21
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class BasicAckMessage : Message
    {
        public new const uint ID =6362;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int seq;
        public short lastPacketId;
        
        public BasicAckMessage()
        {
        }
        
        public BasicAckMessage(int seq, short lastPacketId)
        {
            this.seq = seq;
            this.lastPacketId = lastPacketId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarInt(seq);
            writer.WriteVarShort(lastPacketId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            seq = reader.ReadVarInt();
            if (seq < 0)
                throw new Exception("Forbidden value on seq = " + seq + ", it doesn't respect the following condition : seq < 0");
            lastPacketId = reader.ReadVarShort();
            if (lastPacketId < 0)
                throw new Exception("Forbidden value on lastPacketId = " + lastPacketId + ", it doesn't respect the following condition : lastPacketId < 0");
        }
        
    }
    
}