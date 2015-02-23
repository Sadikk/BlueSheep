









// Generated on 12/11/2014 19:01:13
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class HelloConnectMessage : Message
    {
        public new const uint ID =3;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public string salt;
        public sbyte[] key;
        
        public HelloConnectMessage()
        {
        }
        
        public HelloConnectMessage(string salt, sbyte[] key)
        {
            this.salt = salt;
            this.key = key;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(salt);
            writer.WriteVarInt(key.Length);
            foreach (var entry in key)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            salt = reader.ReadUTF();
            var limit = reader.ReadVarInt();
            key = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 key[i] = reader.ReadSByte();
            }
        }
        
    }
    
}