using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Engine.Types;
using System;
using BlueSheep.Interface;
using System.IO;
using System.Text;
using BlueSheep.Interface.Text;
using System.Collections.Generic;

namespace BlueSheep.Engine.Handlers.Security
{
    class SecurityHandler
    {
        #region Public methods
        [MessageHandler(typeof (RawDataMessage))]
        public static void RawDataMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            //TODO : Bypass this fucking anti-bot
            Random random = new Random();
            int randomNumber = (int)Math.Floor(random.NextDouble() * 16777215);

            CheckIntegrityMessage checkIntegrityMessage = new CheckIntegrityMessage(new List<int>() {randomNumber} );

            using (BigEndianWriter writer = new BigEndianWriter())
            {
                checkIntegrityMessage.Serialize(writer);

                MessagePackaging messagePackaging = new MessagePackaging(writer);

                messagePackaging.Pack((int)checkIntegrityMessage.ProtocolID);

                account.SocketManager.Send(messagePackaging.Writer.Content);
            }
            //using (BigEndianReader reader = new BigEndianReader(packetDatas))
            //{
            //    byte[] content = reader.ReadBytes((int)reader.BytesAvailable);
            //    string d = DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-');
            //    File.WriteAllBytes(@"F:\RDMs\" + d + "_RDM.swf", content);
            //    //account.SocketManager.Disconnect();
            //}

            //using (BigEndianWriter writer = new BigEndianWriter())
            //{
            //    TrustStatusMessage tsm = new TrustStatusMessage(true);
            //    tsm.Serialize(writer);
            //    writer.WriteUTF(account.Ticket);
            //    MessagePackaging pack = new MessagePackaging(writer);
            //    pack.Pack((int)tsm.ProtocolID);
            //    account.SocketManager.SendRaw(pack.Writer.Content);
            //}
            //using (BigEndianWriter writer = new BigEndianWriter())
            //{
            //    writer.WriteBytes(packetDatas);
            //    MessagePackaging pack = new MessagePackaging(writer);
            //    pack.Pack(6253);
            //    account.SocketManager.SendRaw(pack.Writer.Content);
            //}
            //    System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            //    byte[] hash = md5.ComputeHash(pack.Writer.Content);
            //    StringBuilder sb = new StringBuilder();
            //    for (int i = 0; i < hash.Length; i++)
            //    {
            //        sb.Append(hash[i].ToString("X2"));
            //    }
            //    System.Windows.Forms.MessageBox.Show(sb.ToString());
            //}
            
        }

        [MessageHandler(typeof(CheckIntegrityMessage))]
        public static void CheckIntegrityMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            //using (BigEndianWriter writer = new BigEndianWriter())
            //{
            //    writer.WriteBytes(packetDatas);
            //    MessagePackaging pack = new MessagePackaging(writer);
            //    pack.Pack(6372);
            //    account.SocketManager.Send(pack.Writer.Content);
            //    account.Log(new BotTextInformation("Raw data traité avec succès."), 0);
            //}
        }

        public static int MakeCrc(int param1, int param2)
        {
            int loc1 = param2 & 65535;
            int loc2 = param1 & 65535;
            int loc3 = loc1 ^ 14736;
            int ret = loc3 << 16 | loc1 ^ loc2;
            return ret;
        }
        #endregion


    }
}
