using System;
using System.Threading;
using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;

namespace BlueSheep.Engine.Handlers.Basic
{
    class BasicHandler
    {
        #region Public methods
        [MessageHandler(typeof(SequenceNumberRequestMessage))]
        public static void SequenceNumberRequestMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            account.Sequence++;

            SequenceNumberMessage sequenceNumberMessage = new SequenceNumberMessage((ushort)account.Sequence);
            account.SocketManager.Send(sequenceNumberMessage);
        }

        [MessageHandler(typeof(BasicLatencyStatsRequestMessage))]
        public static void BasicLatencyStatsRequestMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            BasicLatencyStatsMessage basicLatencyStatsMessage = new BasicLatencyStatsMessage((ushort)account.LatencyFrame.GetLatencyAvg(),
                account.LatencyFrame.GetSamplesCount(), account.LatencyFrame.GetSamplesMax());
            //account.Log(new BotTextInformation("ROLEPLEY RECU"),0);
            //BasicLatencyStatsMessage basicLatencyStatsMessage = new BasicLatencyStatsMessage(account.LatencyFrame.RolePley(),
            //    account.LatencyFrame.GetSamplesCount(), account.LatencyFrame.GetSamplesMax());
            if (!account.IsMITM)
            {
                using (BigEndianWriter writer = new BigEndianWriter())
                {
                    basicLatencyStatsMessage.Serialize(writer);
                    writer.Content = account.HumanCheck.hash_function(writer.Content);
                    MessagePackaging messagePackaging = new MessagePackaging(writer);

                    messagePackaging.Pack((int)basicLatencyStatsMessage.ProtocolID);

                    account.SocketManager.Send(messagePackaging.Writer.Content);
                    if (account.DebugMode.Checked)
                        account.Log(new BlueSheep.Interface.Text.DebugTextInformation("[SND] 5663 (BasicLatencyStatsMessage)"), 0);
                }
            }
            
        }

        [MessageHandler(typeof (BasicAckMessage))]
        public static void BasicAckMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            BasicAckMessage basicAckMessage = (BasicAckMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                basicAckMessage.Deserialize(reader);
            }

            account.LastPacketID.Enqueue(basicAckMessage.lastPacketId);
            account.LastPacket = basicAckMessage.lastPacketId;

                        
        }

        [MessageHandler(typeof(BasicNoOperationMessage))]
        public static void BasicNoOperationMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            //MainForm.ActualMainForm.ActualizeAccountInformations();
            if (account.checkBoxBegin.Checked)
                account.ActualizeFamis();
            Thread.Sleep(GetRandomTime());

            if (account.LastPacketID.Count == 0)
                return;

            //switch ((uint)account.LastPacketID.Dequeue())
            switch ((uint)account.LastPacket)
            {
                case InteractiveUseRequestMessage.ID:
                    if (account.Running != null && account.Running.OnSafe)
                    {
                        account.Log(new CharacterTextInformation("Ouverture du coffre."), 2);

                        account.Running.Init();
                    }
                    return;

                case ExchangeObjectMoveMessage.ID:
                    if (account.Running.OnLeaving)
                    {
                        account.Running.OnLeaving = false;
                        account.Log(new ActionTextInformation("Dépôt d'un objet dans le coffre."), 3);
                        account.Running.Init();
                    }
                    else if (account.Running.OnGetting)
                    {
                        account.Running.OnGetting = false;
                        account.Log(new ActionTextInformation("Récupération d'un objet du coffre."), 3);
                        account.Running.Init();
                    }
                    return;

                case ObjectFeedMessage.ID:
                    if (account.Running != null && !account.Running.Feeding.SecondFeeding)
                        account.Running.CheckStatisticsUp();
                    else if (account.Running != null)
                    {
                        account.Running.CurrentPetIndex++;
                        account.Running.Init();
                    }
                    return;

                case LeaveDialogRequestMessage.ID:
                    account.Log(new ActionTextInformation("Fermeture du coffre."), 3);
                    if (account.Running != null)
                        account.Running.Init();
                    return;
                //case GameMapMovementRequestMessage.ID:

                //    return;

                //case GameMapMovementConfirmMessage.ID:
                //    account.Fight.LaunchFight(account.Fight.flag);
                //    return;

                default:
                    return;
            }
            
        }
         [MessageHandler(typeof (BasicTimeMessage))]
        public static void BasicTimeMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            BasicTimeMessage btmsg = (BasicTimeMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                btmsg.Deserialize(reader);
            }
            //double serverTimeLag = btmsg.timestamp + btmsg.timezoneOffset * 60 * 1000; // - ((new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc) - DateTime.MinValue) + DateTime.MinValue).TotalMilliseconds;
            //DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            ////System.DateTime date = System.DateTime.FromOADate(msg.subscriptionEndDate); 
            ////epoch.AddSeconds(msg.subscriptionEndDate / 1000) + 3600);
            //epoch = epoch.AddMilliseconds(account.serverTimeLag + serverTimeLag);
            //if (epoch.Minute > 0)
            //{
            //    // account.AboDofLabel.Text = date.Date.ToString();
            //    account.Log(new BotTextInformation(epoch.Date.ToShortDateString()), 0);
            //}
                //account.serverTimeLag = serverTimeLag;
         }

         [MessageHandler(typeof(AccountLoggingKickedMessage))]
         public static void AccountLoggingKickedMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
         {
             AccountLoggingKickedMessage btmsg = (AccountLoggingKickedMessage)message;

             using (BigEndianReader reader = new BigEndianReader(packetDatas))
             {
                 btmsg.Deserialize(reader);
             }
             account.Log(new ErrorTextInformation(String.Format("Compte banni {0} jours, {1} heures, {2} minutes :'( ", btmsg.days, btmsg.hours, btmsg.minutes)), 0);
         }
        
        #endregion

        #region Private methods
        private static int GetRandomTime()
        {
            Random random = new Random();

            return random.Next(250, 750);
        }
        #endregion
    }
}
