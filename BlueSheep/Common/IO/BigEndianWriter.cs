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

        public BigEndianWriter(Stream stream)
        {
            m_BinaryWriter = new BinaryWriter(stream, Encoding.UTF8);
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
            BigEndianWriter buffer = new BigEndianWriter();
            if (value >= 0 && value <= 127)
            {
                WriteBytes(BitConverter.GetBytes(value));
                return;
            }
            int valueToWrite = value;
            while (valueToWrite != 0)
            {
                byte desiredValue = Convert.ToByte(valueToWrite & 127);// extract the first 7 bits
                valueToWrite = valueToWrite >> 7; //get the next 7 bits
                if (valueToWrite > 0) //we're not finished
                {
                    desiredValue = (byte)(desiredValue | 128); //Set the continuation flag
                }
                buffer.WriteByte(desiredValue);
            }
            WriteBytes(buffer.Content);
        }

        public void WriteVarShort(int value)
        {
            if (value > short.MaxValue || value < short.MinValue)
            {
                throw new Exception("Forbidden value");
            }

            BigEndianWriter buffer = new BigEndianWriter();
            if (value >= 0 && value <= 127)
            {
                buffer.WriteByte((byte)value);
                WriteBytes(buffer.Content);
                return;
            }

            short valueToWrite = (short)(value & 65535);
            while (valueToWrite != 0)
            {
                byte byteToWrite = Convert.ToByte(valueToWrite & 127);
                valueToWrite = (short)(valueToWrite >> 7);
                if (valueToWrite > 0)
                {
                    byteToWrite = Convert.ToByte(byteToWrite | 128);
                }
                buffer.WriteByte(byteToWrite);
            }
            WriteBytes(buffer.Content);
        }

        public void WriteVarLong(double param1)
        {

            Int64 result = Int64.fromNumber(param1);
            if (result.high == 0)
            {
                WriteInt32(result.low);
                return;
            }

            for (int i = 0; i < 4; i += 1)
            {
                WriteByte((byte)(result.low & 127 | 128));
                result.low = result.low >> 7;
            }

            if ((result.high & 268435455 << 3) == 0)
            {
                WriteByte((byte)(result.high << 4 | result.low));
            }
            else
            {
                WriteByte((byte)((result.high << 4 | result.low) & 127 | 128));
                WriteInt32((result.high >> 3));
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
