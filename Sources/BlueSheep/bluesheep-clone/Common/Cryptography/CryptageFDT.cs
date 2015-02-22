using System;

namespace BlueSheep.Common.Cryptography
{
    class CryptageBS
    {
        #region Attributs
        private static readonly Random m_Random = new Random();
        #endregion

        #region Méthodes publiques
        public static string EncryptBS(string textToEncrypt)
        {
            string cryptedMessage = string.Empty;
            int messageLenght = textToEncrypt.Length;

            for (int index = 0; index < messageLenght; index++)
            {
                int x = RandomNumber();
                int z = RandomNumber();
                int y = RandomNumber();

                cryptedMessage = cryptedMessage + x + y + z;

                int w = (x * z - y + 2 * x + z) / 4;

                for (int i = 0; i < w; i++)
                    cryptedMessage = cryptedMessage + RandomChar();

                cryptedMessage = cryptedMessage + textToEncrypt[index];

                bool b = true;

                for (int i = 0; i < w; i++)
                {
                    if (b)
                    {
                        cryptedMessage = cryptedMessage + RandomChar();
                        b = false;
                    }
                    else
                    {
                        cryptedMessage = cryptedMessage + RandomSymbol();
                        b = true;
                    }
                }
            }

            return cryptedMessage;
        }

        public static string DecryptBS(string textToDecrypt)
        {
            string decryptedMessage = string.Empty;
            int messageLenght = textToDecrypt.Length;
            int index = 0;

            while (index < messageLenght)
            {
                char xChar = textToDecrypt[index];
                int x = (int)Char.GetNumericValue(xChar);

                index++;

                char yChar = textToDecrypt[index];
                int y = (int)Char.GetNumericValue(yChar);

                index++;

                char zChar = textToDecrypt[index];
                int z = (int)Char.GetNumericValue(zChar);

                int w = (x * z - y + 2 * x + z) / 4;

                if (w > 0)
                    index = index + w + 1;
                else
                    index++;

                decryptedMessage = decryptedMessage + textToDecrypt[index];

                if (w > 0)
                    index = index + w + 1;
                else
                    index++;
            }

            return decryptedMessage;
        }
        #endregion

        #region Méthodes privées
        private static int RandomNumber()
        {
            return m_Random.Next(1, 10);
        }

        private static char RandomChar()
        {
            const string arrayChar = "azertyuiopqsdfghjklmwxcvbnAZERTYUIOPQSDFGHJKLMWXCVBN&é'(-è_çà)=^$ù*,;:!<<¨£µ%/§.+°~#{[|`^@]}¤0123456789";

            int index = m_Random.Next(0, 103);

            return arrayChar[index];
        }

        private static char RandomSymbol()
        {
            const string arrayChar = "&é'(-è_çà)=^$ù*,;:!<<¨£µ%/§.+°~#{[|`^@]}¤";

            int index = m_Random.Next(0, 41);

            return arrayChar[index];
        }
        #endregion
    }
}
