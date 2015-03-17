using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace BlueSheep.Common.Cryptography
{
    class RSAKey // Class de MoonLight <33
    {
        #region Fields
        
        private const string m_PublicKey = "MIIBUzANBgkqhkiG9w0BAQEFAAOCAUAAMIIBOwKCATIAgucoka9J2PXcNdjcu6CuDmgteIMB+rih"
+ "2UZJIuSoNT/0J/lEKL/W4UYbDA4U/6TDS0dkMhOpDsSCIDpO1gPG6+6JfhADRfIJItyHZflyXNUj"
+"WOBG4zuxc/L6wldgX24jKo+iCvlDTNUedE553lrfSU23Hwwzt3+doEfgkgAf0l4ZBez5Z/ldp9it"
+"2NH6/2/7spHm0Hsvt/YPrJ+EK8ly5fdLk9cvB4QIQel9SQ3JE8UQrxOAx2wrivc6P0gXp5Q6bHQo"
+"ad1aUp81Ox77l5e8KBJXHzYhdeXaM91wnHTZNhuWmFS3snUHRCBpjDBCkZZ+CxPnKMtm2qJIi57R"
+"slALQVTykEZoAETKWpLBlSm92X/eXY2DdGf+a7vju9EigYbX0aXxQy2Ln2ZBWmUJyZE8B58CAwEA"
+"AQ==";
        #endregion

        #region Public methods
        public static sbyte[] Encrypt(sbyte[] helloConnectMessageKey, string accountName, string accountPassword, string salt)
        {
            List<byte> credentialsList = new List<byte>();

            RSACryptoServiceProvider decodedPublicKey = DecodeX509PublicKey(Convert.FromBase64String(m_PublicKey));

            byte[] decryptedHelloConnectMessageKey = DecryptHelloConnectMessageKey(helloConnectMessageKey, decodedPublicKey.ExportParameters(false));

            RSACryptoServiceProvider rsaProvider = DecodeX509PublicKey(decryptedHelloConnectMessageKey);

            string adaptedSalt = AdaptSalt(salt);

            credentialsList.AddRange(Encoding.UTF8.GetBytes(adaptedSalt));
            credentialsList.Add((byte)accountName.Length);
            credentialsList.AddRange(Encoding.UTF8.GetBytes(accountName));
            credentialsList.AddRange(Encoding.UTF8.GetBytes(accountPassword));

            byte[] unsignedCredentials = rsaProvider.Encrypt(credentialsList.ToArray(), false);

            sbyte[] credentials = new sbyte[unsignedCredentials.Length];
            Buffer.BlockCopy(unsignedCredentials, 0, credentials, 0, unsignedCredentials.Length);

            return credentials;
        }
        #endregion

        #region Private methods
        private static RSACryptoServiceProvider DecodeX509PublicKey(byte[] X509Key) // Méthode de M4xou
        {
            RSACryptoServiceProvider provider2 = null;
            byte[] secondArray = new byte[] { 0x30, 13, 6, 9, 0x2a, 0x86, 0x48, 0x86, 0xf7, 13, 1, 1, 1, 5, 0 };
            byte[] buffer2 = new byte[15];
            MemoryStream input = new MemoryStream(X509Key);
            BinaryReader reader = new BinaryReader(input);
            ushort num2 = 0;

            try
            {
                switch (reader.ReadUInt16())
                {
                    case 0x8130:
                        reader.ReadByte();
                        break;

                    case 0x8230:
                        reader.ReadInt16();
                        break;

                    default:
                        return null;
                }

                if (CompareByteArrays(reader.ReadBytes(15), secondArray))
                {
                    switch (reader.ReadUInt16())
                    {
                        case 0x8103:
                            reader.ReadByte();
                            goto Label_00B1;

                        case 0x8203:
                            reader.ReadInt16();
                            goto Label_00B1;
                    }
                }

                return null;

            Label_00B1:
                if (reader.ReadByte() == 0)
                {
                    switch (reader.ReadUInt16())
                    {
                        case 0x8130:
                            reader.ReadByte();
                            goto Label_00F9;

                        case 0x8230:
                            reader.ReadInt16();
                            goto Label_00F9;
                    }
                }

                return null;

            Label_00F9:
                num2 = reader.ReadUInt16();

                byte num3 = 0;
                byte num4 = 0;

                switch (num2)
                {
                    case 0x8102:
                        num3 = reader.ReadByte();
                        break;

                    case 0x8202:
                        num4 = reader.ReadByte();
                        num3 = reader.ReadByte();
                        break;

                    default:
                        return null;
                }

                byte[] buffer6 = new byte[4];
                buffer6[0] = num3;
                buffer6[1] = num4;
                byte[] buffer3 = buffer6;
                int count = BitConverter.ToInt32(buffer3, 0);
                byte num6 = reader.ReadByte();
                reader.BaseStream.Seek(-1L, SeekOrigin.Current);

                if (num6 == 0)
                {
                    reader.ReadByte();
                    count--;
                }

                byte[] buffer4 = reader.ReadBytes(count);

                if (reader.ReadByte() != 2)
                {
                    return null;
                }

                int num7 = reader.ReadByte();
                byte[] buffer5 = reader.ReadBytes(num7);
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                RSAParameters parameters = new RSAParameters
                {
                    Modulus = buffer4,
                    Exponent = buffer5
                };
                provider.ImportParameters(parameters);
                provider2 = provider;
            }
            catch (Exception ex)
            {
                provider2 = null;
                System.Windows.Forms.MessageBox.Show(ex.Message, ex.Source);
            }
            finally
            {
                reader.Close();
            }

            return provider2;
        }

        private static bool CompareByteArrays(byte[] firstArray, byte[] secondArray) // Méthode de M4xou
        {
            if (firstArray.Length != secondArray.Length)
            {
                return false;
            }
            int index = 0;
            foreach (byte num2 in firstArray)
            {
                if (num2 != secondArray[index])
                {
                    return false;
                }
                index++;
            }
            return true;
        }

        private static byte[] DecryptHelloConnectMessageKey(sbyte[] helloConnectMessageKey, RSAParameters parameters)
        {
            byte[] unsignedHelloConnectMessageKey = new byte[helloConnectMessageKey.Length];
            Buffer.BlockCopy(helloConnectMessageKey, 0, unsignedHelloConnectMessageKey, 0, helloConnectMessageKey.Length);

            BigInteger modulus = new BigInteger(parameters.Modulus.Reverse().Concat(new byte[] { 0 }).ToArray());
            BigInteger exponent = new BigInteger(parameters.Exponent.Reverse().Concat(new byte[] { 0 }).ToArray());
            BigInteger data = new BigInteger(unsignedHelloConnectMessageKey.Reverse().Concat(new byte[] { 0 }).ToArray());

            byte[] decryptedHelloConnectMessageKey = BigInteger.ModPow(data, exponent, modulus).ToByteArray().Reverse().ToArray();

            return decryptedHelloConnectMessageKey.SkipWhile(x => x != 0).Skip(1).ToArray();
        }

        private static string AdaptSalt(string salt)
        {
            if (salt.Length < 32)
            {
                while (salt.Length < 32)
                    salt += " ";
            }

            return salt;
        }
        #endregion
    }
}
