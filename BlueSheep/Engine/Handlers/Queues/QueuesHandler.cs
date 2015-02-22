using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;

namespace BlueSheep.Engine.Handlers.Queues
{
    class QueuesHandler
    {
        #region Public methods
        [MessageHandler(typeof(LoginQueueStatusMessage))]
        public static void LoginQueueStatusTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            LoginQueueStatusMessage loginQueueStatusMessage = (LoginQueueStatusMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                loginQueueStatusMessage.Deserialize(reader);
            }

            account.Log(new QueueTextInformation("File d'attente : " + loginQueueStatusMessage.position +
                "/" + loginQueueStatusMessage.total + "."),0);
        }

        [MessageHandler(typeof(QueueStatusMessage))]
        public static void QueueStatusMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            QueueStatusMessage queueStatusMessage = (QueueStatusMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                queueStatusMessage.Deserialize(reader);
            }

            account.Log(new QueueTextInformation("File d'attente : " + queueStatusMessage.position + "/"
                + queueStatusMessage.total + "."),0);
        }
        #endregion
    }
}
