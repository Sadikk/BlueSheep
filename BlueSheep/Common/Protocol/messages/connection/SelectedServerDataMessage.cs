









// Generated on 12/11/2014 19:01:14
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class SelectedServerDataMessage : Message
    {
        public const uint ID =42;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool ssl;
        public bool canCreateNewCharacter;
        public short serverId;
        public string address;
        public ushort port;
        public string ticket;
        
        public SelectedServerDataMessage()
        {
        }
        
        public SelectedServerDataMessage(bool ssl, bool canCreateNewCharacter, short serverId, string address, ushort port, string ticket)
        {
            this.ssl = ssl;
            this.canCreateNewCharacter = canCreateNewCharacter;
            this.serverId = serverId;
            this.address = address;
            this.port = port;
            this.ticket = ticket;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, ssl);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, canCreateNewCharacter);
            writer.WriteByte(flag1);
            writer.WriteVarShort(serverId);
            writer.WriteUTF(address);
            writer.WriteUShort(port);
            writer.WriteUTF(ticket);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            ssl = BooleanByteWrapper.GetFlag(flag1, 0);
            canCreateNewCharacter = BooleanByteWrapper.GetFlag(flag1, 1);
            serverId = reader.ReadVarShort();
            if (serverId < 0)
                throw new Exception("Forbidden value on serverId = " + serverId + ", it doesn't respect the following condition : serverId < 0");
            address = reader.ReadUTF();
            port = reader.ReadUShort();
            if (port < 0 || port > 65535)
                throw new Exception("Forbidden value on port = " + port + ", it doesn't respect the following condition : port < 0 || port > 65535");
            ticket = reader.ReadUTF();
        }
        
    }
    
}