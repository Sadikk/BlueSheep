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
            var _loc4_= 0;
         var result = 0;
         var i = 0;
         bool _loc3_ = false;

         while (i < 32) //Int size
         {
             _loc4_ = this.ReadByte();
             _loc3_ = (_loc4_ & 128) == 128;

             if (i > 0)
             {
                 result = result + ((_loc4_ & 127) << i);
             }
             else
             {
                 result = result + (_loc4_ & 127);
             }
             i = i + 7; //Chunck bit size
             if (!_loc3_)
             {
                 return result;
             }
         }
         throw new Exception("Too much data");
        }

        public short ReadVarShort()
        {
            var _loc4_ = 0;
            var result= 0;
            var _loc2_ = 0;
            bool _loc3_ = false;

            while (_loc2_ < 16) //Short size
            {
                _loc4_ = this.ReadByte();
                _loc3_ = (_loc4_ & 128) == 128;
                if (_loc2_ > 0)
                {
                    result= result+ ((_loc4_ & 127) << _loc2_);
                }
                else
                {
                    result= result+ (_loc4_ & 127);
                }
                _loc2_ = _loc2_ + 7;
                //Chunck Bit size
                if (!_loc3_)
                {
                    if (result> 32767) // Short max value
                    {
                        result= result- 65536;
                        //Unsigned short max value
                    }
                    return (short)result;
                }
            }
            throw new Exception("Too much data");
        }

        public double ReadVarLong()
        {
            byte _loc_3 = 0;
            Int64 result = new Int64();
            uint _loc_4 = 0;
            while (true)
            {

                _loc_3 = this.ReadByte();
                if (_loc_4 == 28)
                {
                    break;
                }
                else if (_loc_3 >= 128)
                {
                    result.low = Convert.ToUInt32(result.low | Convert.ToUInt32(_loc_3 & 127) << Convert.ToInt32(_loc_4));
                }
                else
                {
                    result.low = Convert.ToUInt32(result.low | Convert.ToUInt32(_loc_3) << Convert.ToInt32(_loc_4));
                    return result.toNumber();
                }
                _loc_4 = _loc_4 + 7;
            }
            if (_loc_3 >= 128)
            {
                _loc_3 = (byte)(_loc_3 & 127);
                result.low = Convert.ToUInt32(result.low | Convert.ToUInt32(_loc_3) << Convert.ToInt32(_loc_4));
                result.high = Convert.ToUInt32(_loc_3) >> 4;
            }
            else
            {
                result.low = Convert.ToUInt32(result.low | Convert.ToUInt32(_loc_3) << Convert.ToInt32(_loc_4));
                result.high = Convert.ToUInt32(_loc_3) >> 4;
                return result.toNumber();
            }
            _loc_4 = 3;
            while (true)
            {

                _loc_3 = ReadByte();
                if (_loc_4 < 32)
                {
                    if (_loc_3 >= 128)
                    {
                        result.high = result.high | Convert.ToUInt32(_loc_3 & 127) << Convert.ToInt32(_loc_4);
                    }
                    else
                    {
                        result.high = result.high | Convert.ToUInt32(_loc_3) << Convert.ToInt32(_loc_4);
                        break;
                    }
                }
                _loc_4 = _loc_4 + 7;
            }
            return result.toNumber();
        }

        public double ReadVarUhLong()
        {
            int _loc_3  = 0;
            UInt64 result = new UInt64();
            int _loc_4 = 0;
            while (true)
            {
                
                _loc_3 = ReadByte();
                if (_loc_4 == 28)
                {
                    break;
                }
                else if (_loc_3 >= 128)
                {
                    result.low = result.low | Convert.ToUInt32(_loc_3 & 127) << Convert.ToInt32(_loc_4);
                }
                else
                {
                    result.low = result.low | Convert.ToUInt32(_loc_3) << Convert.ToInt32(_loc_4);
                    return result.toNumber();
                }
                _loc_4 = _loc_4 + 7;
            }
            if (_loc_3 >= 128)
            {
                _loc_3 = _loc_3 & 127;
                result.low = result.low | Convert.ToUInt32(_loc_3) << Convert.ToInt32(_loc_4);
                result.high = Convert.ToUInt32(_loc_3) >> 4;
            }
            else
            {
                result.low = result.low | Convert.ToUInt32(_loc_3) << Convert.ToInt32(_loc_4);
                result.high = Convert.ToUInt32(_loc_3) >> 4;
                return result.toNumber();
            }
            _loc_4 = 3;
            while (true)
            {
                
                _loc_3 = ReadByte();
                if (_loc_4 < 32)
                {
                    if (_loc_3 >= 128)
                    {
                        result.high = result.high | Convert.ToUInt32(_loc_3 & 127) << Convert.ToInt32(_loc_4);
                    }
                    else
                    {
                        result.high = result.high | Convert.ToUInt32(_loc_3) << Convert.ToInt32(_loc_4);
                        break;
                    }
                }
                _loc_4 = _loc_4 + 7;
            }
            return result.toNumber();
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
