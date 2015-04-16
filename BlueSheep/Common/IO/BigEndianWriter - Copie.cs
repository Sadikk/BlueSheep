using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueSheep.Common.IO
{
    [Serializable]
    public class BigEndianWriter : IDisposable
    {
        #region Fields
        private BinaryWriter m_BinaryWriter;
        #endregion

        #region Properties
        public byte[] Content
        {
            get
            {
                byte[] content = new byte[m_BinaryWriter.BaseStream.Length];

                m_BinaryWriter.BaseStream.Position = 0;
                m_BinaryWriter.BaseStream.Read(content, 0, (int)m_BinaryWriter.BaseStream.Length);

                return content;
            }
            set
            {
                m_BinaryWriter.BaseStream.Position = 0;
                m_BinaryWriter.BaseStream.Write(value, 0, value.Length);
            }
        }
        #endregion

        #region Constructeurs
        public BigEndianWriter()
        {
            m_BinaryWriter = new BinaryWriter(new MemoryStream(), Encoding.UTF8);
        }
        #endregion

        #region Public methods
        public void WriteByte(byte byteToWrite)
        {
            m_BinaryWriter.Write(byteToWrite);
        }

        public void WriteBytes(byte[] bytesToWrite)
        {
            m_BinaryWriter.Write(bytesToWrite);
        }

        public void WriteUTF(string stringUTF8ToWrite)
        {
            byte[] stringContentInByte = Encoding.UTF8.GetBytes(stringUTF8ToWrite);
            ushort stringLength = (ushort)stringContentInByte.Length;

            WriteUShort(stringLength);

            for (int index = 0; index < stringContentInByte.Length; index++)
            {
                m_BinaryWriter.Write(stringContentInByte[index]);
            }
        }

        public void WriteInt(int intToWrite)
        {
            WriteInBigEndian(BitConverter.GetBytes(intToWrite));
        }

        public void WriteDouble(double doubleToWrite)
        {
            WriteInBigEndian(BitConverter.GetBytes(doubleToWrite));
        }

        public void WriteShort(short shortToWrite)
        {
            WriteInBigEndian(BitConverter.GetBytes(shortToWrite));
        }

        public void WriteBoolean(bool boolToWrite)
        {
            if (boolToWrite)
                m_BinaryWriter.Write((byte)1);
            else
                m_BinaryWriter.Write((byte)0);
        }

        public void WriteChar(char charToWrite)
        {
            WriteInBigEndian(BitConverter.GetBytes(charToWrite));
        }

        public void WriteLong(long longToWrite)
        {
            WriteInBigEndian(BitConverter.GetBytes(longToWrite));
        }

        public void WriteFloat(float floatToWrite)
        {
            WriteInBigEndian(BitConverter.GetBytes(floatToWrite));
        }

        public void WriteSingle(Single singleToWrite)
        {
            WriteInBigEndian(BitConverter.GetBytes(singleToWrite));
        }

        public void WriteUInt(uint uintToWrite)
        {
            WriteInBigEndian(BitConverter.GetBytes(uintToWrite));
        }

        public void WriteUShort(ushort ushortToWrite)
        {
            WriteInBigEndian(BitConverter.GetBytes(ushortToWrite));
        }

        public void WriteULong(ulong ulongToWrite)
        {
            WriteInBigEndian(BitConverter.GetBytes(ulongToWrite));
        }

        public void WriteSByte(sbyte sbyteToWrite)
        {
            m_BinaryWriter.Write(sbyteToWrite);
        }

        public void WriteVarInt(int value)
        {
            //byte _loc5_ = 0;
            //Byte[] _loc2_ = new Byte[1];
            //if (VarToWrite >= 0 && VarToWrite <= 127)
            //{
            //    this.WriteInt(VarToWrite);
            //    return;
            //}
            //int _loc3_ = VarToWrite;
            //Byte[] _loc4_ = new Byte[1];
            //while (_loc3_ != 0)
            //{
            //    _loc4_[_loc4_.Length -1] = (byte)(_loc3_ & 127);
            //    _loc5_ = _loc4_[_loc4_.Length - 1];
            //    _loc3_ = Convert.ToInt32(Convert.ToUInt32(_loc3_) >> 7);
            //    if (_loc3_ > 0)
            //    {
            //        _loc5_ = (byte)(_loc5_ | 128);
            //    }
            //    _loc2_[_loc4_.Length - 1] = _loc5_;
            //}
            //this.WriteBytes(_loc2_);

            //byte _loc_5 = 0;
            //List<byte> result = new List<byte>();
            //if ((VarToWrite >= 0 & VarToWrite <= 127))
            //{
            //    this.WriteBytes(new Byte[] {Convert.ToByte(VarToWrite)});
            //    return;
            //}
            //int _loc_3 = VarToWrite & 65535;
            //List<byte> _loc_4 = new List<byte>();
            //while ((_loc_3 != 0))
            //{
            //    _loc_4.Add(Convert.ToByte(_loc_3 & 127));
            //    //     _loc_4.BaseStream.Position = _loc_4.BytesToSend.Count - 1
            //    _loc_5 = _loc_4[_loc_4.Count - 1];
            //    _loc_4.RemoveAt(_loc_4.Count - 1);
            //    _loc_3 = _loc_3 >> 7;
            //    if ((_loc_3 > 0))
            //    {
            //        _loc_5 = Convert.ToByte(_loc_5 | 128);
            //    }
            //    result.Add(_loc_5);
            //}
            //this.WriteBytes(result.ToArray());

            var _loc5_ = 0;
            BigEndianWriter buffer = new BigEndianWriter();
            if (value >= 0 && value <= 127)
            {
                buffer.WriteByte((byte)value);
                this.WriteBytes(BitConverter.GetBytes(value));
                return;
            }
            int _loc3_ = value;
            BigEndianWriter subBuffer = new BigEndianWriter();
            while (_loc3_ != 0)
            {
                subBuffer.WriteByte(Convert.ToByte(_loc3_ & 127));
                subBuffer.m_BinaryWriter.Seek(0, SeekOrigin.End);
                //subBuffer.Seek((int)subBuffer.BaseStream.Length, SeekOrigin.Begin);
                _loc5_ = subBuffer.m_BinaryWriter.BaseStream.ReadByte();
                _loc3_ = _loc3_ >> 7;
                if (_loc3_ > 0)
                {
                    _loc5_ = _loc5_ | 128;
                }
                //buffer.WriteBytes(BitConverter.GetBytes(_loc5_));
                buffer.WriteByte((byte)_loc5_);
            }
            this.WriteBytes(buffer.Content);

        }

        public void WriteVarShort(int value)
        {
            byte _loc5_ = 0;
            if (value > short.MaxValue || value < short.MinValue)
            {
                throw new Exception("Forbidden value");
            }
            else
            {
                BigEndianWriter buffer = new BigEndianWriter();
                if (value >= 0 && value <= 127)
                {
                    buffer.WriteByte((byte)value);
                    this.WriteBytes(buffer.Content);
                    return;
                }
                int tampon = value & 65535;
                BigEndianWriter subBuffer = new BigEndianWriter();
                while (tampon != 0)
                {
                    subBuffer.WriteByte(Convert.ToByte(tampon & 127));
                    subBuffer.m_BinaryWriter.Seek((int)subBuffer.m_BinaryWriter.BaseStream.Length, SeekOrigin.Begin);
                    _loc5_ = (byte)subBuffer.m_BinaryWriter.BaseStream.ReadByte();
                    tampon = tampon >> 7;
                    if (tampon > 0)
                    {
                        _loc5_ = Convert.ToByte(_loc5_ | 128);
                    }
                    buffer.WriteByte(_loc5_);
                }
                this.WriteBytes(buffer.Content);
                return;
            }
        }

        public void WriteVarLong(double param1)
        {

            Int64 result = Int64.fromNumber(param1);
            uint _loc3_ = 0;
            if (result.high == 0)
            {
                while (true)
                {

                    //if (result.low < 128)
                    //{
                    //    this.WriteByte((byte)result.low);
                    //    return;
                    //}
                    //WriteByte((byte)(result.low & 127 | 128));
                    //result.low = result.low >> 7;
                    this.WriteInt32(result.low);
                }
            }
            else
            {
                _loc3_ = 0;
                while (_loc3_ < 4)
                {
                    this.WriteByte((byte)(result.low & 127 | 128));
                    result.low = result.low >> 7;
                    _loc3_++;
                }
                if ((result.high & 268435455 << 3) == 0)
                {
                    this.WriteByte((byte)(result.high << 4 | result.low));
                }
                else
                {
                    this.WriteByte((byte)((result.high << 4 | result.low) & 127 | 128));
                    int p = Convert.ToInt32(Convert.ToUInt32(result.high) >> 3);
                    while (true)
                    {
                        this.WriteByte((byte)((result.high << 4 | result.low) & 127 | 128));
                        this.WriteInt32((uint)(result.high >> 3));
                    }
                }
            }
        }

      private void WriteInt32(uint param1)
      {
          while (param1 >= 128)
          {
              WriteByte((byte)(param1 & 127 | 128));
              param1 = param1 >> 7;
          }
          WriteByte((byte)param1);
      }

        public void Dispose()
        {
            m_BinaryWriter.Dispose();
            m_BinaryWriter = null;
        }
        #endregion

        #region Private methods
        private void WriteInBigEndian(byte[] contentToConvert)
        {
            for (int index = contentToConvert.Length - 1; index >= 0; index--)
                m_BinaryWriter.Write(contentToConvert[index]);
        }
        #endregion
    }
}
