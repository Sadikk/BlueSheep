using BlueSheep.Interface;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlueSheep.Engine.Frame
{
    public class LatencyFrame
    {
        #region Fields
        private uint m_LatestSent;
        private readonly List<uint> m_LatencyBuffer;
        private const uint m_LatencyAvgBufferSize = 50;
        private AccountUC account;
        #endregion

        #region Constructeurs
        public LatencyFrame(AccountUC Account)
        {
            m_LatencyBuffer = new List<uint>();
            account = Account;
            //string blah = Str2.Str7(-1820302795);
            //pId = Player id
            //ct = character Ticket
            // hash loader info : 40c2dfcf773f83a6bbe1777b6e3a87f1
        }
        #endregion

        #region Public methods
        public void LowSend()
        {
            m_LatestSent = (uint)Environment.TickCount;
        }

        public void UpdateLatency()
        {
            uint lastReceive = (uint)Environment.TickCount;

            if (m_LatestSent != 0)
            {
                m_LatencyBuffer.Add(lastReceive - m_LatestSent);

                m_LatestSent = 0;

                if (m_LatencyBuffer.Count > m_LatencyAvgBufferSize)
                    m_LatencyBuffer.RemoveAt(0);
            }
        }

        public short GetLatencyAvg()
        {
            if (m_LatencyBuffer.Count == 0)
                return 0;

            uint totalLatency = 0;

            foreach (uint latency in m_LatencyBuffer)
                totalLatency += latency;

            return (short)(totalLatency / m_LatencyBuffer.Count);
        }

        public short GetSamplesCount()
        {
            return (short)m_LatencyBuffer.Count;
        }

        public short GetSamplesMax()
        {
            return (short)m_LatencyAvgBufferSize;
        }
        #endregion

        #region RolePley

        public ushort RolePley()
        {
            int pId = account.CharacterBaseInformations.id;
            uint ct = Convert.ToUInt32(account.Ticket);
            string loaderInfo = "4dd3a4c8490e4517d1883a5c367e1B5f";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(loaderInfo);
            BinaryWriter m_writer = new BinaryWriter(new MemoryStream());
            int len = bytes.Length;

            for (int i = 0; i < len; i++)
                m_writer.Write(bytes[i]);

            BinaryReader m_reader = new BinaryReader(m_writer.BaseStream);
            int j = 0;
            uint h = 0;

            while (j < bytes.Length)
            {
                j++;  
                h = Convert.ToUInt32((h + m_reader.ReadByte()) + (ct & (j << 1)) % 128);
            }
           //(_loc5_) || (h + md5Byte.readByte() + (ct & (i << 1)) % 128);

            return Convert.ToUInt16(this.GetLatencyAvg() + ((pId + h) % 20) - 10);
        }

        public class Str2
        {

            public Str2()
            {
                //var _loc1_:* = false;
                //var _loc2_:* = true;
                //!_loc1_;
                //super();
            }

            private static byte[] Str8;

            private static byte[] Str9;

            private static byte[] Str10;

            private static string[] Str11 = new List<string>().ToArray();

            private static byte[][] Str12 = new List<byte[]>().ToArray();

            private static bool Str13 = false;

            private static int Str14;

            private static void Str15()
            {

                byte[] _loc1_ = new List<byte>().ToArray();
                byte[] _loc2_ = new List<byte>().ToArray();
                byte[] _loc3_ = new List<byte>().ToArray();
                BinaryReader reader = new BinaryReader(new MemoryStream(_loc3_));
                Str14 = reader.ReadInt32();
                reader = new BinaryReader(new MemoryStream(_loc2_));
                int _loc4_ = reader.ReadByte();
                int _loc5_ = 0;
                while (_loc5_ < _loc4_)
                {
                    Str16(_loc2_);
                    _loc5_++;
                }
                reader = new BinaryReader(new MemoryStream(_loc1_));
                _loc4_ = reader.ReadInt32();
                int i = 0;
                while (i < _loc4_)
                {
                    Str17(_loc1_, Str12[i % Str12.Length]);
                    i++;
                }
                Str13 = true;
                return;
            }

            private static void Str17(byte[] param1, byte[] param2)
            {
                BinaryReader reader = new BinaryReader(new MemoryStream(param1));
                int _loc3_ = reader.ReadInt32();
                byte[] _loc4_ = new List<byte>().ToArray();
                _loc4_ = reader.ReadBytes(_loc3_);
                Str5 _loc5_ = new Str5(param2);
                _loc5_.Str27(_loc4_);
                reader = new BinaryReader(new MemoryStream(_loc4_));
                Str11[Str11.Length - 1] = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(_loc4_.Length));
                return;
            }

            private static void Str16(byte[] param1)
            {
                BinaryReader reader = new BinaryReader(new MemoryStream(param1));
                byte[] _loc2_ = reader.ReadBytes(16);
                //while (_loc2_.position = 0)
                //{
                Str12[Str12.Length - 1] = _loc2_;
                return;
                //}
                //var _loc3_ = _loc3_;
                //var _loc0_ = Str2;
                return;
            }

            public static string Str7(int param1)
            {
                if (!Str13)
                {
                    Str15();
                }
                return Str11[param1 ^ Str14];
            }
        }

        public class Str5
        {
            private const uint Str19 = 256;

            private int Str20 = 0;
            private int Str21 = 0;
            private byte[] Str22;

            public Str5(byte[] param1 = null)
            {
                this.Str22 = new List<byte>().ToArray();
                if (param1 != null)
                {
                    this.Str23(param1);
                }
                return;
            }

            public uint Str24()
            {
                return Str19;
            }

            public void Str23(byte[] param1)
            {
                int _loc2_;
                int _loc3_;
                int _loc4_;
                _loc2_ = 0;
                while (_loc2_ < 256)
                {
                    this.Str22[_loc2_] = (byte)_loc2_;
                    _loc2_++;
                }
                _loc3_ = 0;
                _loc2_ = 0;
                for (_loc2_ = 0; _loc2_ < 256; _loc2_++)
                {
                    _loc3_ = (_loc3_ + this.Str22[_loc2_] + param1[_loc2_ % param1.Length]) & 255;
                    //goto lab2;
                    _loc4_ = this.Str22[_loc2_];
                    do
                    {
                        this.Str22[_loc2_] = this.Str22[_loc3_];
                        this.Str22[_loc3_] = (byte)_loc4_;
                        continue;

                    lab1:
                        _loc4_ = this.Str22[_loc2_];
                    } while (true);

                    //lab2:
                    //    _loc3_ = (_loc3_ + this.Str22[_loc2_] + param1[_loc2_ % param1.Length]) & 255;
                    //    goto lab1;
                    //var _loc6_ = null;
                }
                this.Str20 = 0;
                this.Str21 = 0;
            }

            private uint Str25()
            {
                int _loc1_ = 0;
                this.Str20 = (this.Str20 + 1) & 255;
                this.Str21 = (this.Str21 + this.Str22[this.Str20]) & 255;
                while (_loc1_ == this.Str22[this.Str20])
                {
                    this.Str22[this.Str20] = this.Str22[this.Str21];
                    goto lab1;
                }
                //var _loc2_ = _loc2_;
                var _loc0_ = this;

            lab1:
                this.Str22[this.Str21] = (byte)_loc1_;
                return (this.Str22[(_loc1_ + this.Str22[this.Str20]) & 255]);
            }

            public uint Str26()
            {
                return 1;
            }

            public void Str27(byte[] param1)
            {
                uint _loc2_ = 0;
                while (_loc2_ < param1.Length)
                {
                    var _loc3_ = _loc2_++;
                    param1[_loc3_] = Convert.ToByte((param1[_loc3_] ^ this.Str25()));
                }
                return;
            }
        }
        #endregion
    }
}
