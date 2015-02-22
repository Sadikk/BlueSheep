using System;
using System.IO;
using BlueSheep.Common.IO;

namespace BlueSheep.Engine.Types
{
    class MessagePackaging
    {
        #region Fields
        private BigEndianWriter m_Writer;
        private byte[] m_Packet;
        #endregion

        #region Properties
        public BigEndianWriter Writer
        {
            get { return m_Writer; }
            set { m_Writer = value; }
        }
        #endregion

        #region Constructeur
        public MessagePackaging(BigEndianWriter writer)
        {
            m_Writer = writer;
        }
        #endregion

        #region Public methods
        public void Pack(int protocolID)
        {
            int index = 0;
            int packetLenght = m_Writer.Content.Length;
            int packetLenghtType = GetLenghtType();
            short packetHeader = GetHeader(protocolID);

            // Création du Header
            m_Packet = new byte[2 + packetLenghtType + packetLenght];
            m_Packet[0] = (byte)(packetHeader >> 8);
            m_Packet[1] = (byte)(packetHeader - 256 * m_Packet[0]);

            // Différentes tailles
            switch (packetLenghtType)
            {
                case 1:
                    m_Packet[2] = (byte)(packetLenght);
                    index = 3;
                    break;
                case 2:
                    m_Packet[2] = (byte)(packetLenght >> 8);
                    m_Packet[3] = (byte)(packetLenght - 256 * m_Packet[2]);
                    index = 4;
                    break;
                case 3:
                    m_Packet[2] = (byte)(packetLenght >> 16);
                    m_Packet[3] = (byte)(packetLenght >> 8);
                    m_Packet[4] = (byte)(packetLenght - 256 * m_Packet[3] - 256 * 256 * m_Packet[2]);
                    index = 5;
                    break;
            }

            // Remplissage du paquet
            for (int i = index; i < index + packetLenght; i++)
                m_Packet[i] = m_Writer.Content[i - index];

            m_Writer.Content = m_Packet;

            //using (
            //    StreamWriter fileWriter =
            //        new StreamWriter(
            //            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
            //            @"\BlueSheep\Packets.txt", true))
            //    fileWriter.WriteLine("[" + DateTime.Now.ToShortTimeString() + "] Paquet envoyé : " + protocolID);
        }
        #endregion

        #region Private methods
        private int GetLenghtType()
        {
            // Différents types de tailles
            int packetLenghtType = 0;

            if (m_Writer.Content.Length > short.MaxValue)
                packetLenghtType = 3;
            else if (m_Writer.Content.Length > byte.MaxValue)
                packetLenghtType = 2;
            else if (m_Writer.Content.Length > 0)
                packetLenghtType = 1;

            return packetLenghtType;
        }

        private short GetHeader(int protocolID)
        {
            // Création du Header
            return (short)((protocolID << 2) | GetLenghtType());
        }
        #endregion
    }
}
