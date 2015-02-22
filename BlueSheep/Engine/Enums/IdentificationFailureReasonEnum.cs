using BlueSheep.Interface;
using BlueSheep.Interface.Text;

namespace BlueSheep.Engine.Enums
{
    class IdentificationFailureReasonEnum
    {
        #region Fields
        private const int m_WrongCredentials = 2;
        private const int m_Banned = 3;
        private const int m_Kicked = 4;
        private const int m_InMaintenance = 5;
        #endregion

        #region Public methods
        public static void Test(int reason, AccountUC account)
        {
            switch (reason)
            {
                case m_WrongCredentials:
                    account.Log(new ErrorTextInformation("Echec de connexion : mauvais identifiants."),0);
                    break;

                case m_Banned:
                    account.Log(new ErrorTextInformation("Echec de connexion : compte banni."), 0);
                    break;

                case m_Kicked:
                    account.Log(new ErrorTextInformation("Echec de connexion : compte banni temporairement."), 0);
                    break;

                case m_InMaintenance:
                    account.Log(new ErrorTextInformation("Echec de connexion : serveur en maintenance."), 0);
                    account.TryReconnect(15);
                    break;

                default:
                    account.Log(new ErrorTextInformation("Echec de connexion : erreur inconnue."), 0);
                    break;
            }
        }
        #endregion
    }
}
