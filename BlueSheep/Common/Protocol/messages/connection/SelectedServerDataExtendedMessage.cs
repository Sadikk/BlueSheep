









// Generated on 12/11/2014 19:01:14
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class SelectedServerDataExtendedMessage : SelectedServerDataMessage
    {
        public new const uint ID =6469;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short[] serverIds;
        
        public SelectedServerDataExtendedMessage()
        {
        }
        
        public SelectedServerDataExtendedMessage(bool ssl, bool canCreateNewCharacter, short serverId, string address, ushort port, string ticket, short[] serverIds)
         : base(ssl, canCreateNewCharacter, serverId, address, port, ticket)
        {
            this.serverIds = serverIds;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)serverIds.Length);
            foreach (var entry in serverIds)
            {
                 writer.WriteVarShort(entry);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            serverIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 serverIds[i] = reader.ReadVarShort();
            }
        }
        
    }
    
}