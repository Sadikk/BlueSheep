namespace BlueSheep.Common.Data
{
    class I18N
    {
        #region Fields
        private static I18NFileAccessor m_I18NFileAccessor;
        #endregion

        #region Constructeurs
        public I18N(I18NFileAccessor i18NFileAccessor)
        {
            m_I18NFileAccessor = i18NFileAccessor;
        }
        #endregion

        #region Public methods
        public static string GetText(int id)
        {
            string result = m_I18NFileAccessor.GetText(id);

            if (result == null || result == "null")
            {
                return "[UNKNOWN_TEXT_ID_" + id + "]";
            }

            return result;
        }
        #endregion
    }
}
