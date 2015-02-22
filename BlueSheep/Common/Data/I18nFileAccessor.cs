using System;
using System.Collections.Generic;
using System.IO;
using BlueSheep.Common.IO;

namespace BlueSheep.Common.Data
{
    class I18NFileAccessor
    {
        #region Fields
        private BigEndianReader m_Stream;
        private Dictionary<int, int> m_Indexes;
        private Dictionary<int, int> m_UnDiacriticalIndex;
        private Dictionary<string, int> m_TextIndexes;
        private Dictionary<string, string> m_TextIndexesOverride;
        private Dictionary<int, int> m_TextSortIndex;
        #endregion

        #region Public methods
        public void Init(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
                throw new Exception("I18n file not readable.");

            if (m_Indexes == null)
                m_Indexes = new Dictionary<int, int>();

            if (m_UnDiacriticalIndex == null)
                m_UnDiacriticalIndex = new Dictionary<int, int>();

            if (m_TextIndexes == null)
                m_TextIndexes = new Dictionary<string, int>();

            if (m_TextIndexesOverride == null)
                m_TextIndexesOverride = new Dictionary<string, string>();

            if (m_TextSortIndex == null)
                m_TextSortIndex = new Dictionary<int, int>();

            byte[] fileContent = new byte[fileInfo.Length];
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileStream.Read(fileContent, 0, fileContent.Length);
            fileStream.Dispose();

            m_Stream = new BigEndianReader(fileContent);

            int position = m_Stream.ReadInt();
            m_Stream.Seek(position);

            int indexCount = m_Stream.ReadInt();

            for (int index = 0; index < indexCount; index += 9)
            {
                int key = m_Stream.ReadInt();
                bool diacriticalText = m_Stream.ReadBoolean();
                int pointeur = m_Stream.ReadInt();
                m_Indexes.Add(key, pointeur);

                if (diacriticalText)
                {
                    index += 4;
                    m_UnDiacriticalIndex.Add(key, m_Stream.ReadInt());
                }
                else
                    m_UnDiacriticalIndex.Add(key, pointeur);
            }

            int textIndexesCount = m_Stream.ReadInt();

            while (textIndexesCount > 0)
            {
                position = (int) m_Stream.Position;
                m_TextIndexes.Add(m_Stream.ReadUTF(), m_Stream.ReadInt());
                textIndexesCount -= (int) (m_Stream.Position - position);
            }

            textIndexesCount = m_Stream.ReadInt();

            int i = 0;

            while (textIndexesCount > 0)
            {
                position = (int)m_Stream.Position;
                m_TextSortIndex.Add(m_Stream.ReadInt(), ++i);
                textIndexesCount -= (int)(m_Stream.Position - position);
            }
        }

        public string GetText(int id)
        {
            if ((m_Indexes == null) || (!m_Indexes.ContainsKey(id)))
                return null;

            m_Stream.Seek(m_Indexes[id]);
            return m_Stream.ReadUTF();
        }
        #endregion
    }
}
