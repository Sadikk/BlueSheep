using System;
using System.IO;
using BlueSheep.Common.IO;
using BlueSheep.Interface;
using System.Collections.Generic;
using BlueSheep.Interface.Text;
namespace BlueSheep.Engine.Types
{
    class MessageInformations
    {
        #region Fields
        private readonly BigEndianReader m_Reader = new BigEndianReader();
        private int? m_Header;
        private int? m_Length;
        private byte[] m_Data;
        private int? m_LenghtType;
        private int? m_ProtocolID;
        private AccountUC account;
        private List<int> ForbiddenHeaders = new List<int>() { 42, 6469 };
        #endregion

        #region Public methods
        public MessageInformations(AccountUC accountform)
        {
            account = accountform;
        }
        public void ParseBuffer(byte[] data)
        {
            m_Reader.Add(data, 0, data.Length);
            if (m_Reader.BytesAvailable <= 0)
                return;
            while (m_Reader.BytesAvailable != 0)
            {
                if (Build())
                {
                    if ((account.IsMITM) && (!ForbiddenHeaders.Contains((int)m_ProtocolID)))
                    {
                        using (BigEndianWriter writer = new BigEndianWriter())
                        {
                            writer.WriteBytes(m_Data);
                            MessagePackaging pack = new MessagePackaging(writer);
                            pack.Pack((int)m_ProtocolID);
                            account.SocketManager.SendToDofusClient(pack.Writer.Content);
                            account.Log(new BotTextInformation("SND (-> Dof) " + m_ProtocolID), 0);
                        }
                    }
                    //using (
                    // StreamWriter fileWriter =
                    // new StreamWriter(
                    // Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    // @"\BlueSheep\Packets.txt", true))
                    // fileWriter.WriteLine("[" + DateTime.Now.ToShortTimeString() + "] Paquet reçu : " + m_ProtocolID);
                    Treatment.Treatment treatment = new Treatment.Treatment(account);
                    if (m_ProtocolID == 6372)
                    {
                        using (BigEndianWriter writer = new BigEndianWriter())
                        {
                            writer.WriteBytes(m_Data);
                            MessagePackaging pack = new MessagePackaging(writer);
                            pack.Pack(6372);
                            account.SocketManager.Send(pack.Writer.Content);
                        }
                    }
                    if (m_ProtocolID != null)
                        treatment.Treat(m_ProtocolID.Value, m_Data);
                    m_Header = null;
                    m_Length = null;
                    m_Data = null;
                    m_LenghtType = null;
                    m_ProtocolID = null;
                }
                else
                    break;
            }
        }
        #endregion

        #region Private methods
        private bool Build()
        {
            if ((m_Header.HasValue) && (m_Length.HasValue) && (m_Length == m_Data.Length))
                return true;
            if ((m_Reader.BytesAvailable >= 2) && (!m_Header.HasValue))
            {
                m_Header = m_Reader.ReadShort();
                m_ProtocolID = m_Header >> 2;
                m_LenghtType = m_Header & 0x3;
            }
            if ((m_LenghtType.HasValue) &&
            (m_Reader.BytesAvailable >= m_LenghtType) && (!m_Length.HasValue))
            {
                if ((m_LenghtType < 0) || (m_LenghtType > 3))
                    throw new Exception("Malformated Message Header, invalid bytes number to read message length (inferior to 0 or superior to 3)");
                m_Length = 0;
                for (int i = m_LenghtType.Value - 1; i >= 0; i--)
                    m_Length |= m_Reader.ReadByte() << (i * 8);
            }
            if ((m_Data == null) && (m_Length.HasValue))
            {
                if (m_Length == 0)
                    m_Data = new byte[0];
                if (m_Reader.BytesAvailable >= m_Length)
                    m_Data = m_Reader.ReadBytes(m_Length.Value);
                else if (m_Length > m_Reader.BytesAvailable)
                    m_Data = m_Reader.ReadBytes((int)m_Reader.BytesAvailable);
            }
            if ((m_Data != null) && (m_Length.HasValue) && (m_Data.Length < m_Length))
            {
                int bytesToRead = 0;
                if (m_Data.Length + m_Reader.BytesAvailable < m_Length)
                    bytesToRead = (int)m_Reader.BytesAvailable;
                else if (m_Data.Length + m_Reader.BytesAvailable >= m_Length)
                    bytesToRead = m_Length.Value - m_Data.Length;
                if (bytesToRead != 0)
                {
                    int oldLength = m_Data.Length;
                    Array.Resize(ref m_Data, m_Data.Length + bytesToRead);
                    Array.Copy(m_Reader.ReadBytes(bytesToRead), 0, m_Data, oldLength, bytesToRead);
                }
            }
            return m_Data != null && ((m_Header.HasValue) && (m_Length.HasValue) && (m_Length == m_Data.Length));
        }
        #endregion
    }
}