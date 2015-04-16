using BlueSheep.Interface;
using BlueSheep.Interface.Text;

namespace BlueSheep.Engine.Enums
{
    class ServerStatusEnum
    {
        #region Attirbuts
        private const uint m_Offline = 1;
        private const uint m_Saving = 5;
        private const uint m_Full = 7;
        #endregion

        #region Public methods
        public static void Test(uint status, AccountUC account)
        {
            switch (status)
            {
                case m_Offline:
                   account.Log(new ConnectionTextInformation("Echec de connexion : serveur déconnecté."),0);
                   account.TryReconnect(600);
                    break;

                case m_Saving:
                    account.Log(new ConnectionTextInformation("Echec de connexion : serveur en sauvegarde."),0);
                    account.TryReconnect(600);
                    break;

                case m_Full:
                    account.Log(new ConnectionTextInformation("Echec de connexion : serveur complet."),0);
                    account.TryReconnect(60);
                    break;

                default:
                    account.Log(new ConnectionTextInformation("Echec de connexion : raison inconnue."),0);
                    account.TryReconnect(30);
                    break;
            }
        }
        #endregion
    }
}
