









// Generated on 12/11/2014 19:01:35
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class EntityTalkMessage : Message
    {
        public new const uint ID =6110;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int entityId;
        public short textId;
        public string[] parameters;
        
        public EntityTalkMessage()
        {
        }
        
        public EntityTalkMessage(int entityId, short textId, string[] parameters)
        {
            this.entityId = entityId;
            this.textId = textId;
            this.parameters = parameters;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteVarShort(textId);
            writer.WriteUShort((ushort)parameters.Length);
            foreach (var entry in parameters)
            {
                 writer.WriteUTF(entry);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            entityId = reader.ReadInt();
            textId = reader.ReadVarShort();
            if (textId < 0)
                throw new Exception("Forbidden value on textId = " + textId + ", it doesn't respect the following condition : textId < 0");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 parameters[i] = reader.ReadUTF();
            }
        }
        
    }
    
}