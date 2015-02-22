namespace BlueSheep.Common.Types
{
    public class Account
    {
        #region Fields
        private readonly string m_Name;
        #endregion

        #region Properties
        public string Name
        {
            get { return m_Name; }
        }

        public string Password { get; set; }
        #endregion

        #region Constructeurs
        public Account(string name, string password)
        {
            m_Name = name;
            Password = password;
        }
        #endregion
    }
}
