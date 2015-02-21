using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Engine.Constants;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BlueSheep.Core.Misc
{
    public class Flood
    {
        #region Fields
        AccountUC account;
        public bool stop;
#endregion
        
        #region Constructors
        public Flood(AccountUC Account)
        {
            account = Account;
        }
        #endregion

        #region Public Methods
        public void StartFlooding(int channel, bool useSmiley, bool useNumbers, string content, int interval)
        {
            stop = false;
            string ncontent = content;
            while (stop == false)
            {
                if (useSmiley == true)
                    ncontent = AddRandomSmiley(content);
                if (useNumbers == true)
                    ncontent = AddRandomNumber(content);
                SendMessage(channel, ncontent);
                account.Wait(interval * 1000, interval * 1000);
            }
        }

        public void SendMessage(int channel, string content)
        {
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                ChatClientMultiMessage msg = new ChatClientMultiMessage(content, (sbyte)channel);
                msg.Serialize(writer);
                writer.Content = account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
            }
        }

        public void SendPrivateTo(string name)
        {
            string content = account.FloodContent;
            if (account.IsRandomingSmileyBox.Checked == true)
                content = AddRandomSmiley(content);
            if (account.IsRandomingNumberBox.Checked == true)
                content = AddRandomNumber(content);
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                ChatClientPrivateMessage msg = new ChatClientPrivateMessage(content,name);
                msg.Serialize(writer);
                writer.Content = account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
                account.Log(new PrivateTextInformation("à " + name + " : " + content),1); 
            }
        }

        #endregion

        #region Private Methods
        private string AddRandomSmiley(string content)
        {
            int randomIndex = new Random().Next(0, 8);
            string nCon = content + " " + smileys[randomIndex];
            return nCon;
        }

        private string AddRandomNumber(string content)
        {
            int randomIndex = new Random().Next(0, 500);
            string nCon = content + " " + randomIndex.ToString();
            return nCon;
        }
        #endregion

        #region Enums
        public static readonly IList<String> smileys = new ReadOnlyCollection<string>
        (new List<String> {":)",";)","=)",":D",":p","=p",":d","=d","=P"});
        #endregion
    }
}
