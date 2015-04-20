using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Engine.Constants;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using BlueSheep.Interface.Text.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace BlueSheep.Core.Misc
{
    public class Flood
    {
        #region Fields
        AccountUC account;
        public bool stop;
        public Dictionary<string,int> listOfPlayers;
        
        #endregion
        
        #region Constructors
        public Flood(AccountUC Account)
        {
            account = Account;
            listOfPlayers = new Dictionary<string, int>();      
        }
        #endregion

        #region Public Methods
        public void StartFlood(int channel, bool useSmiley, bool useNumbers, string content, int interval)
        {
            Thread t = new Thread(() => StartFlooding(channel, useSmiley, useNumbers, content, interval));
            t.Start();
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
                if (account.DebugMode.Checked)
                    account.Log(new DebugTextInformation("[SND] 861 (ChatClientMultiMessage)"), 0);
                account.FloodUC.Increase(false);
            }
        }

        public void SendPrivateTo(BlueSheep.Common.Protocol.Types.GameRolePlayCharacterInformations infos, string content = "")
        {
            if (content == "")
                content = account.FloodUC.FloodContent;
            int level = Math.Abs((infos.alignmentInfos.characterPower - infos.contextualId));
            content = content.Replace("%name%", infos.name).Replace("%level%", Convert.ToString(level));
            if (account.FloodUC.IsRandomingSmileyBox.Checked == true)
                content = AddRandomSmiley(content);
            if (account.FloodUC.IsRandomingNumberBox.Checked == true)
                content = AddRandomNumber(content);
            SendPrivateTo(infos.name, content);
            account.FloodUC.Increase(true);
        }

        public void SendPrivateTo(string name, string content)
        {
            if (mods.Contains(name))
            {
                account.Log(new ErrorTextInformation("[Flood] Annulation de l'envoi d'un message privé à " + name + " (Modo)"), 0);
                return;
            }
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                ChatClientPrivateMessage msg = new ChatClientPrivateMessage(content, name);
                msg.Serialize(writer);
                writer.Content = account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
                account.Log(new PrivateTextInformation("à " + name + " : " + content), 1);
                if (account.DebugMode.Checked)
                    account.Log(new DebugTextInformation("[SND] 851 (ChatClientPrivateMessage)"), 0);
            }
        }
 

        public void SaveNameInMemory(BlueSheep.Common.Protocol.Types.GameRolePlayCharacterInformations infos)
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlueSheep", "Accounts", account.AccountName, "Flood");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            try
            {
                if (listOfPlayers.Count > 0)
                {
                    if (listOfPlayers.Keys.ToList().Find(p => p == infos.name) != null)
                    {
                        account.Log(new ErrorTextInformation("[ADVANCED FLOOD] Player already loaded !"), 5);
                        return;
                    }                        
                }
                var swriter = new StreamWriter(path + @"\Players.txt", true);
                int level = Math.Abs((infos.alignmentInfos.characterPower - infos.contextualId));
                swriter.WriteLine(infos.name + "," + Convert.ToString(level));
                swriter.Close();
                listOfPlayers.Add(infos.name, level);
                account.FloodUC.AddItem(infos.name + "," + Convert.ToString(level));
                account.Log(new BotTextInformation("[ADVANCED FLOOD] Player added."), 5);
            }
            catch (Exception ex)
            {
                account.Log(new ErrorTextInformation("[ADVANCED FLOOD] Unable to add the player."), 5);
                account.Log(new ErrorTextInformation(ex.ToString()), 5);
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

        private void StartFlooding(int channel, bool useSmiley, bool useNumbers, string content, int interval)
        {
            stop = false;
            string ncontent = content;
            while (stop == false)
            {
                if (useSmiley == true)
                    ncontent = AddRandomSmiley(content);
                if (useNumbers == true)
                    ncontent = AddRandomNumber(ncontent);
                SendMessage(channel, ncontent);
                account.Wait(interval * 1000, interval * 1000);
            }
        }
        #endregion

        #region Enums
        public static readonly IList<String> smileys = new ReadOnlyCollection<string>
        (new List<String> {":)",";)","=)",":D",":p","=p",":d","=d","=P"});

        private static readonly IList<String> mods = new ReadOnlyCollection<string>
        (new List<String> { "[Japlo]" ,"[Lobeline]","[Eknelis]" ,"[Miaidaouh]", "[Alkalino]", "[Seekah]","[Taikorg]",
            "[Lytimelle]","[Gowolik]","[Diospyros]", "[TobliK]","[Simeth]","[Gazviv]", "[Prag-Matik]","[Maatastrea]",
            "[Griffinx]", "[Selvetarm]", "[Jial]", "[Haeo-Lien]", "[VeniVidi]", "[Falgoryn]","[Ayuzal]", "[Pad-Panikk]",
            "[Portgas]", "[Arkansyelle]","[Padalgarath]", "[Semitam]", "[Latnac]", "[Fumikiri]", "[Saskhya]", "[Vandavarya]",
            "[Modorak]", "[Yesht]", "[Alikaric]", "[Enyden]" });
        #endregion
    }
}
