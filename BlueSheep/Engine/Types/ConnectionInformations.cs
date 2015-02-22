namespace BlueSheep.Engine.Types
{
    public class ConnectionInformations
    {
        #region Properties
        public string Address { get; set; }

        public int Port { get; set; }

        public string ServerName { get; set; }
        #endregion

        #region Constructeurs
        public ConnectionInformations(string address, int port, string serverName)
        {
            Address = address;
            Port = port;
            ServerName = serverName;
        }
        #endregion
    }
}
