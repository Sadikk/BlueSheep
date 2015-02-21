









// Generated on 12/11/2014 19:01:58
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class UpdateMapPlayersAgressableStatusMessage : Message
    {
        public new const uint ID =6454;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public int[] playerIds;
        public sbyte[] enable;
        
        public UpdateMapPlayersAgressableStatusMessage()
        {
        }
        
        public UpdateMapPlayersAgressableStatusMessage(int[] playerIds, sbyte[] enable)
        {
            this.playerIds = playerIds;
            this.enable = enable;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)playerIds.Length);
            foreach (var entry in playerIds)
            {
                 writer.WriteVarInt(entry);
            }
            writer.WriteUShort((ushort)enable.Length);
            foreach (var entry in enable)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            playerIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 playerIds[i] = reader.ReadVarInt();
            }
            limit = reader.ReadUShort();
            enable = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 enable[i] = reader.ReadSByte();
            }
        }
        
    }
    
}