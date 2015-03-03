using System;
using System.IO;
using System.Text;

namespace BlueSheep.Common.IO
{
    public class BigEndianReader : IDisposable
    {
        #region Fields
        private BinaryReader m_BinaryReader;
        #endregion

        #region Properties
        public long BytesAvailable
        {
            get { return (m_BinaryReader.BaseStream.Length - m_BinaryReader.BaseStream.Position); }
        }

        public long Position
        {
            get { return m_BinaryReader.BaseStream.Position; }
        }

        public Stream BaseStream
        {
            get { return m_BinaryReader.BaseStream; }
        }
        #endregion

        #region Constructeurs
        public BigEndianReader()
        {
            m_BinaryReader = new BinaryReader(new MemoryStream(), Encoding.UTF8);
        }

        public BigEndianReader(byte[] content)
        {
            m_BinaryReader = new BinaryReader(new MemoryStream(content), Encoding.UTF8);
        }

        public BigEndianReader(Stream stream)
        {
            m_BinaryReader = new BinaryReader(stream, Encoding.UTF8);
        }
        #endregion

        #region Public methods
        public byte ReadByte()
        {
            return m_BinaryReader.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            return m_BinaryReader.ReadBytes(count);
        }

        public string ReadUTF()
        {
            ushort stringLength = ReadUShort();

            byte[] stringContentInByte = ReadBytes(stringLength);

            return Encoding.UTF8.GetString(stringContentInByte);
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(ConvertToBigEndian(4), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(ConvertToBigEndian(8), 0);
        }

        public short ReadShort()
        {
            return BitConverter.ToInt16(ConvertToBigEndian(2), 0);
        }

        public bool ReadBoolean()
        {
            return m_BinaryReader.ReadByte() == 1;
        }

        public char ReadChar()
        {
            return (char)ReadUShort();
        }

        public long ReadLong()
        {
            return BitConverter.ToInt64(ConvertToBigEndian(8), 0);
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(ConvertToBigEndian(4), 0);
        }

        public Single ReadSingle()
        {
            return BitConverter.ToSingle(ConvertToBigEndian(4), 0);
        }

        public uint ReadUInt()
        {
            return BitConverter.ToUInt32(ConvertToBigEndian(4), 0);
        }

        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(ConvertToBigEndian(2), 0);
        }

        public ulong ReadULong()
        {
            return BitConverter.ToUInt64(ConvertToBigEndian(8), 0);
        }

        public sbyte ReadSByte()
        {
            return m_BinaryReader.ReadSByte();
        }

        public string ReadStringUTF7()
        {
            int stringLength = ReadInt();

            byte[] stringContentInByte = ReadBytes(stringLength);

            return Encoding.UTF8.GetString(stringContentInByte);
        }

        public int ReadVarInt()
        {
            int resultVar = 0;
            for (int offset = 0; offset < 32; offset += 7)
            {
                byte readByte = ReadByte();
                bool hasContinuationFlag = (readByte & 128) == 128;
                int extractedValue = (readByte & 127);

                if (offset > 0) //TODO: not sure if the if statement is needed
                    extractedValue = extractedValue << offset;

                resultVar += extractedValue;

                if (hasContinuationFlag == false)
                    return resultVar;
            }
            throw new Exception("Too much data");
        }

        public short ReadVarShort()
        {
            var resultVar = 0;

            for (int offset = 0; offset < 16; offset += 7)
            {
                byte readByte = this.ReadByte();
                bool hasContinuationFlag = (readByte & 128) == 128;
                int extractedValue = (readByte & 127);
                if (offset > 0)
                    extractedValue = extractedValue << offset;

                resultVar += extractedValue;

                if (hasContinuationFlag == false)
                {
                    if (resultVar > 32767)
                        resultVar -= 65536;

                    return (short)resultVar;
                }
            }
            throw new Exception("Too much data");
        }

        public Int64 _ReadVarLong()
        {
            byte readByte = 0;
            Int64 result = new Int64();
            for (int offset = 0; offset != 28; offset += 7)
            {
                readByte = ReadByte();
                bool missingContinuationFlag = (readByte < 128);
                if (missingContinuationFlag)
                {
                    result.low = result.low | (uint)(readByte << offset);
                    return result;
                }

                result.low = result.low | (uint)((readByte & 127) << offset);
            }

            readByte = this.ReadByte();
            if (readByte < 128)
            {
                result.low = (uint)(result.low | readByte << 28);
                result.high = (uint)(readByte >> 4);
                return result;
            }

            readByte = (byte)(readByte & 127);
            result.low = (uint)(result.low | readByte << 28);
            result.high = (uint)(readByte >> 4);

            for (int offset = 3; offset < 32; offset += 7)
            {
                readByte = ReadByte();
                bool missingContinuationFlag = (readByte < 128);
                if (missingContinuationFlag)
                {
                    result.high = result.high | (uint)(readByte << offset);
                    break; //TODO: This MUST be hit otherwise I dont know if this will work
                }
                result.high = result.high | (uint)((readByte & 127) << offset);
            }

            return result;
        }

        public long ReadVarLong()
        {
            return _ReadVarLong().toNumber();
        }
            
        public ulong ReadVarUhLong()
        {
            return (ulong)_ReadVarLong().toNumber();
        }
    

        public void Seek(int position)
        {
            m_BinaryReader.BaseStream.Position = position;
        }

        public void Add(byte[] data, int offset, int count)
        {
            long pos = m_BinaryReader.BaseStream.Position;

            m_BinaryReader.BaseStream.Position = m_BinaryReader.BaseStream.Length;
            m_BinaryReader.BaseStream.Write(data, offset, count);
            m_BinaryReader.BaseStream.Position = pos;
        }

        public void Dispose()
        {
            m_BinaryReader.Dispose();
            m_BinaryReader = null;
        }
        #endregion

        #region Private methods
        private byte[] ConvertToBigEndian(int count)
        {
            byte[] convertedContent = new byte[count];

            for (int index = count - 1; index >= 0; index--)
                convertedContent[index] = (byte)m_BinaryReader.BaseStream.ReadByte();

            return convertedContent;
        }
        #endregion
    }
}
