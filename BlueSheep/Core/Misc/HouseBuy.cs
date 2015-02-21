using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Core.Misc
{
    public class HouseBuy
    {
        private AccountUC account;
        public int priceHouse;
        public int ElementIdd;
        public int SkillInstanceID;

        public HouseBuy(AccountUC Account)
        {
            account = Account;
        }

        #region Methods
        private void Say(string sentence)
        {
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                ChatClientMultiMessage msg = new ChatClientMultiMessage(sentence, (sbyte)0);
                msg.Serialize(writer);
                writer.Content = account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
            }
        }

        public void UseHouse()
        {
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                InteractiveUseRequestMessage msg = new InteractiveUseRequestMessage(ElementIdd,SkillInstanceID);
                msg.Serialize(writer);
                writer.Content = account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
            }
        }

        public void Buy()
        {
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                HouseBuyRequestMessage msg = new HouseBuyRequestMessage(priceHouse);
                msg.Serialize(writer);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
            }
            account.Log(new BotTextInformation("Maison achetée pour " + priceHouse + " kamas !"),0);
            if (account.PhraseADire.Text.Length != 0)
            {
                Say(account.PhraseADire.Text);
            }
        }

        #endregion
    }
}
