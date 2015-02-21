









// Generated on 12/11/2014 19:01:19
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameActionFightTackledMessage : AbstractGameActionMessage
    {
        public new const uint ID =1004;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int[] tacklersIds;
        
        public GameActionFightTackledMessage()
        {
        }
        
        public GameActionFightTackledMessage(short actionId, int sourceId, int[] tacklersIds)
         : base(actionId, sourceId)
        {
            this.tacklersIds = tacklersIds;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)tacklersIds.Length);
            foreach (var entry in tacklersIds)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            tacklersIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 tacklersIds[i] = reader.ReadInt();
            }
        }
        
    }
    
}