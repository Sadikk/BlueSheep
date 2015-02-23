









// Generated on 12/11/2014 19:02:00
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class CheckIntegrityMessage : Message
    {
        public new const uint ID =6372;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        //public sbyte[] data;
        public List<int> data;

        public CheckIntegrityMessage()
        {
        }
        
        //public CheckIntegrityMessage(sbyte[] data)
        //{
        //    this.data = data;
        //}

        public CheckIntegrityMessage(List<int> data)
        {
            this.data = data;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            //writer.WriteInt(this.data);
            //writer.WriteUShort((ushort)data.Length);
            //foreach (var entry in data)
            //{
            //     writer.WriteSByte(entry);
            //}
            writer.WriteVarInt(this.data.Count);
            int _loc_2 = 0;
            while (_loc_2 < this.data.Count)
            {
                
                writer.WriteByte((byte)this.data[_loc_2]);
                _loc_2 = _loc_2 + 1;
            }
        }

        public override void Deserialize(BigEndianReader reader)
        {

            //var limit = reader.ReadUShort();
            //data = new sbyte[limit];
            //for (int i = 0; i < limit; i++)
            //{
            //    data[i] = reader.ReadSByte();
            //}
        }
        
    }
    
}