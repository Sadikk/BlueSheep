using BlueSheep.Common.IO;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BlueSheep.Engine.Constants
{
    public class HumanCheck
    {
        /* HASH FUNCTION BYPASS. PLEASE DON'T USE WITHOUT CREDITS */
        private const string BASE64_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        
        public byte[] _hashKey;

        public HumanCheck(AccountUC account)
        {
            this._hashKey = hashKey_Gen(account);
        }

        public byte[] hash_function(byte[] packet)
        {
            /* Hello Mr.Fourbasse :hap: */
            if (this._hashKey == null)
                return packet;
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                writer.WriteBytes(packet);
                BigEndianWriter writer2 = new BigEndianWriter();
                writer2.WriteBytes(this.hash(packet));
                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    myRijndael.GenerateIV();
                    // Encrypt the string to an array of bytes. 
                    byte[] encrypted = EncryptStringToBytes(writer2.Content.ToString(), this._hashKey, myRijndael.IV);
                }
                writer.WriteBytes(writer2.Content);
                return writer.Content;
            }
        }

        #region AES
        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.KeySize = 128;
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }
#endregion

        private byte[] hash(byte[] src)
        {
            uint _loc2_ = Convert.ToUInt32(src.Length * 8);
            while (src.Length % 4 != 0)
            {
                List<byte> list = src.ToList();
                list.RemoveAt(src.Length - 1);
                src = list.ToArray();
            }
            List<uint> _loc4_ = new List<uint>();
            uint _loc5_ = 0;
            byte[] _loc6_;
            using (MD5 md5Hash = MD5.Create())
            {
                _loc6_ = md5Hash.ComputeHash(src.ToArray());
            }
            BinaryReader reader = new BinaryReader(new MemoryStream(_loc6_), Encoding.UTF8);
            while (_loc5_ < _loc6_.Length)
            {
                _loc4_.Add(reader.ReadUInt32());
                _loc5_ = _loc5_ + 4;
            }
            BinaryWriter writer = new BinaryWriter(new MemoryStream(), Encoding.Default);
            _loc5_ = 0;
            while (_loc5_ < 4)
            {
                writer.Write(_loc4_.ToArray()[_loc5_]);
                _loc5_++;
            }
            byte[] content = new byte[writer.BaseStream.Length];

            writer.BaseStream.Position = 0;
            writer.BaseStream.Read(content, 0, (int)writer.BaseStream.Length);

            return content;
        }

        private uint[] core_md5(uint[] x, uint len)
        {
            x[len >> 5] |= (uint)(128) << ((int)(len) % 32);
            x[(len + 64 >> 9 << 4) + 14] = len;
            uint a = 0x67452301; // 1732584193;
            uint b = 0xEFCDAB89; //-271733879;
            uint c = 0x98BADCFE; //-1732584194;
            uint d = 0x10325476; // 271733878;
            for (uint i = 0; i < x.Length; i += 16)
            {
                x[i] = (uint)((int)x[i]) | (0);
                x[i + 1] = (uint)((int)x[i + 1]) | (0);
                x[i + 2] = (uint)((int)x[i + 2]) | (0);
                x[i + 3] = (uint)((int)x[i + 3]) | (0);
                x[i + 4] = (uint)((int)x[i + 4]) | (0);
                x[i + 5] = (uint)((int)x[i + 5]) | (0);
                x[i + 6] = (uint)((int)x[i + 6]) | (0);
                x[i + 7] = (uint)((int)x[i + 7]) | (0);
                x[i + 8] = (uint)((int)x[i + 8]) | (0);
                x[i + 9] = (uint)((int)x[i + 9]) | (0);
                x[i + 10] = (uint)((int)x[i + 10]) | (0);
                x[i + 11] = (uint)((int)x[i + 11]) | (0);
                x[i + 12] = (uint)((int)x[i + 12]) | (0);
                x[i + 13] = (uint)((int)x[i + 13]) | (0);
                x[i + 14] = (uint)((int)x[i + 14]) | (0);
                x[i + 15] = (uint)((int)x[i + 15]) | (0);
                uint olda = a;
                uint oldb = b;
                uint oldc = c;
                uint oldd = d;
                a = ff(a, b, c, d, x[i + 0], 7, 0xD76AA478);
                d = ff(d, a, b, c, x[i + 1], 12, 0xE8C7B756);
                c = ff(c, d, a, b, x[i + 2], 17, 0x242070DB);
                b = ff(b, c, d, a, x[i + 3], 22, 0xC1BDCEEE);
                a = ff(a, b, c, d, x[i + 4], 7, 0xF57C0FAF);
                d = ff(d, a, b, c, x[i + 5], 12, 0x4787C62A);
                c = ff(c, d, a, b, x[i + 6], 17, 0xA8304613);
                b = ff(b, c, d, a, x[i + 7], 22, 0xFD469501);
                a = ff(a, b, c, d, x[i + 8], 7, 0x698098D8);
                d = ff(d, a, b, c, x[i + 9], 12, 0x8B44F7AF);
                c = ff(c, d, a, b, x[i + 10], 17, 0xFFFF5BB1);
                b = ff(b, c, d, a, x[i + 11], 22, 0x895CD7BE);
                a = ff(a, b, c, d, x[i + 12], 7, 0x6B901122);
                d = ff(d, a, b, c, x[i + 13], 12, 0xFD987193);
                c = ff(c, d, a, b, x[i + 14], 17, 0xA679438E);
                b = ff(b, c, d, a, x[i + 15], 22, 0x49B40821);
                a = gg(a, b, c, d, x[i + 1], 5, 0xf61e2562);
                d = gg(d, a, b, c, x[i + 6], 9, 0xc040b340);
                c = gg(c, d, a, b, x[i + 11], 14, 0x265e5a51);
                b = gg(b, c, d, a, x[i + 0], 20, 0xe9b6c7aa);
                a = gg(a, b, c, d, x[i + 5], 5, 0xd62f105d);
                d = gg(d, a, b, c, x[i + 10], 9, 0x2441453);
                c = gg(c, d, a, b, x[i + 15], 14, 0xd8a1e681);
                b = gg(b, c, d, a, x[i + 4], 20, 0xe7d3fbc8);
                a = gg(a, b, c, d, x[i + 9], 5, 0x21e1cde6);
                d = gg(d, a, b, c, x[i + 14], 9, 0xc33707d6);
                c = gg(c, d, a, b, x[i + 3], 14, 0xf4d50d87);
                b = gg(b, c, d, a, x[i + 8], 20, 0x455a14ed);
                a = gg(a, b, c, d, x[i + 13], 5, 0xa9e3e905);
                d = gg(d, a, b, c, x[i + 2], 9, 0xfcefa3f8);
                c = gg(c, d, a, b, x[i + 7], 14, 0x676f02d9);
                b = gg(b, c, d, a, x[i + 12], 20, 0x8d2a4c8a);
                a = hh(a, b, c, d, x[i + 5], 4, 0xfffa3942);
                d = hh(d, a, b, c, x[i + 8], 11, 0x8771f681);
                c = hh(c, d, a, b, x[i + 11], 16, 0x6d9d6122);
                b = hh(b, c, d, a, x[i + 14], 23, 0xfde5380c);
                a = hh(a, b, c, d, x[i + 1], 4, 0xa4beea44);
                d = hh(d, a, b, c, x[i + 4], 11, 0x4bdecfa9);
                c = hh(c, d, a, b, x[i + 7], 16, 0xf6bb4b60);
                b = hh(b, c, d, a, x[i + 10], 23, 0xbebfbc70);
                a = hh(a, b, c, d, x[i + 13], 4, 0x289b7ec6);
                d = hh(d, a, b, c, x[i + 0], 11, 0xeaa127fa);
                c = hh(c, d, a, b, x[i + 3], 16, 0xd4ef3085);
                b = hh(b, c, d, a, x[i + 6], 23, 0x4881d05);
                a = hh(a, b, c, d, x[i + 9], 4, 0xd9d4d039);
                d = hh(d, a, b, c, x[i + 12], 11, 0xe6db99e5);
                c = hh(c, d, a, b, x[i + 15], 16, 0x1fa27cf8);
                b = hh(b, c, d, a, x[i + 2], 23, 0xc4ac5665);
                a = ii(a, b, c, d, x[i + 0], 6, 0xf4292244);
                d = ii(d, a, b, c, x[i + 7], 10, 0x432aff97);
                c = ii(c, d, a, b, x[i + 14], 15, 0xab9423a7);
                b = ii(b, c, d, a, x[i + 5], 21, 0xfc93a039);
                a = ii(a, b, c, d, x[i + 12], 6, 0x655b59c3);
                d = ii(d, a, b, c, x[i + 3], 10, 0x8f0ccc92);
                c = ii(c, d, a, b, x[i + 10], 15, 0xffeff47d);
                b = ii(b, c, d, a, x[i + 1], 21, 0x85845dd1);
                a = ii(a, b, c, d, x[i + 8], 6, 0x6fa87e4f);
                d = ii(d, a, b, c, x[i + 15], 10, 0xfe2ce6e0);
                c = ii(c, d, a, b, x[i + 6], 15, 0xa3014314);
                b = ii(b, c, d, a, x[i + 13], 21, 0x4e0811a1);
                a = ii(a, b, c, d, x[i + 4], 6, 0xf7537e82);
                d = ii(d, a, b, c, x[i + 11], 10, 0xbd3af235);
                c = ii(c, d, a, b, x[i + 2], 15, 0x2ad7d2bb);
                b = ii(b, c, d, a, x[i + 9], 21, 0xeb86d391);
                a += olda;
                b += oldb;
                c += oldc;
                d += oldd;
            }
            return new uint[4] { a, b, c, d };
        }

        //Bitwise rotate a 32-bit number to the left.
        private uint rol(uint num, uint cnt)
        {
            return Convert.ToUInt32(((int)num << (int)cnt) | (num >> (int)(32 - cnt)));
        }
        //These functions implement the four basic operations the algorithm uses.
        private uint cmn(uint param1, uint param2, uint param3, uint param4, uint param5, uint param6)
        {
            return rol(param2 + param1 + param4 + param6, param5) + param3;
        }

        private uint ff(uint param1, uint param2, uint param3, uint param4, uint param5, uint param6, uint param7)
        {
            return cmn(param2 & param3 | ~param2 & param4, param1, param2, param5, param6, param7);
        }

        private uint gg(uint param1, uint param2, uint param3, uint param4, uint param5, uint param6, uint param7)
        {
            return cmn(param2 & param4 | param3 & ~param4, param1, param2, param5, param6, param7);
        }

        private uint hh(uint param1, uint param2, uint param3, uint param4, uint param5, uint param6, uint param7)
        {
            return cmn(param2 ^ param3 ^ param4, param1, param2, param5, param6, param7);
        }

        private uint ii(uint param1, uint param2, uint param3, uint param4, uint param5, uint param6, uint param7)
        {
            return cmn(param3 ^ (param2 | ~param4), param1, param2, param5, param6, param7);
        }
   

        private byte[] hashKey_Gen(AccountUC account)
        {
            try
            {
                using (BigEndianWriter writer = new BigEndianWriter())
                {
                    writer.WriteUTF(account.Ticket);
                    byte[] key = hash(writer.Content);
                    return key;
                }
            }
            catch (Exception ex)
            {
                account.Log(new ErrorTextInformation(ex.Message + ex.InnerException), 0);
                return null;
            }
        }

        #region Private Methods
        private string dec(string param1, string param2)
        {
            byte[] _loc3_ = this.decodeToByteArray(param1);
            byte[] _loc4_ = this.decodeToByteArray(param2);
            uint _loc5_ = 0;
            while (_loc5_ < _loc3_.Length)
            {
                _loc3_[_loc5_] = Convert.ToByte(_loc3_[_loc5_] ^ _loc4_[_loc5_ % _loc4_.Length]);
                _loc5_++;
            }
            return _loc3_.ToString(); ;
        }

        private byte[] decodeToByteArray(string param1)
        {
            uint _loc6_ = 0;
            uint _loc7_ = 0;
            List<byte> _loc2_ = new List<byte>();
            uint[] _loc3_ = new uint[4];
            uint[] _loc4_ = new uint[3];
            uint _loc5_ = 0;
            while (_loc5_ < param1.Length)
            {
                _loc6_ = 0;
                while (_loc6_ < 4 && _loc5_ + _loc6_ < param1.Length)
                {
                    _loc3_[_loc6_] = (uint)BASE64_CHARS.IndexOf(param1[(int)_loc5_ + (int)_loc6_]);
                    _loc6_++;
                }
                _loc4_[0] = (_loc3_[0] << 2) + ((_loc3_[1] & 48) >> 4);
                _loc4_[1] = ((_loc3_[1] & 15) << 4) + ((_loc3_[2] & 60) >> 2);
                _loc4_[2] = ((_loc3_[2] & 3) << 6) + _loc3_[3];
                _loc7_ = 0;
                while (_loc7_ < _loc4_.Length)
                {
                    if (_loc3_[_loc7_ + 1] == 64)
                    {
                        break;
                    }
                    _loc2_.Add(Convert.ToByte(_loc4_[_loc7_]));
                    _loc7_++;
                }
                _loc5_ = _loc5_ + 4;
            }
            return _loc2_.ToArray();
        }
        #endregion
    }
}
