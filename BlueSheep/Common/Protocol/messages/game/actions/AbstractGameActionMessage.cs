









// Generated on 12/11/2014 19:01:15
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class AbstractGameActionMessage : Message
    {
        public new const uint ID =1000;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short actionId;
        public int sourceId;
        
        public AbstractGameActionMessage()
        {
        }
        
        public AbstractGameActionMessage(short actionId, int sourceId)
        {
            this.actionId = actionId;
            this.sourceId = sourceId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteVarShort(actionId);
            writer.WriteInt(sourceId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            actionId = reader.ReadVarShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            sourceId = reader.ReadInt();
        }
        
    }
    
}