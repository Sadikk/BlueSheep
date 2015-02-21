using System;
using System.Collections.Generic;
using System.Threading;
using BlueSheep.Common.Data;
using BlueSheep.Common.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using BlueSheep.Engine.Network;
using BlueSheep.Engine.Types;
using Timer = System.Threading.Timer;
using System.IO;

namespace BlueSheep.Core
{
    public class MainRunning
    {
        #region Attributs
        private Running m_Running;
        private Thread m_ConnectionThread;
        private Timer m_TimerConnectionThread;
        private int m_CurrentBotIndex;
        private List<Bot> m_Bots = new List<Bot>();
        private Bot m_CurrentBot;
        private DateTime m_NextMeal;
        #endregion

        #region Propriétés
        public Bot CurrentBot
        {
            get { return m_CurrentBot; }
            set { m_CurrentBot = value; }
        }

        public Bot SelectedBot { get; set; }

        public List<Bot> Bots
        {
            get { return m_Bots; }
            set { m_Bots = value; }
        }

        public int CurrentBotIndex
        {
            get { return m_CurrentBotIndex; }
            set { m_CurrentBotIndex = value; }
        }

        public Thread ConnectionThread
        {
            get { return m_ConnectionThread; }
            set { m_ConnectionThread = value; }
        }

        public Running Running
        {
            get { return m_Running; }
            set { m_Running = value; }
        }

        public GameDataFileAccessor GameDataFileAccessor
        {
            get { return m_GameDataFileAccessor; }
        }
        #endregion

        #region Constructeur
        public MainRunning()
        {
            m_CurrentBotIndex = 0;

            I18NFileAccessor i18NFileAccessor = new I18NFileAccessor();
            string path = "";
            if (File.Exists(@"C:\Program Files (x86)\Dofus2\app\data\i18n\i18n_fr.d2i"))
            {
                path = @"C:\Program Files (x86)\Dofus2\app\data\i18n\i18n_fr.d2i";
                i18NFileAccessor.Init(path);
                I18N i18N = new I18N(i18NFileAccessor);
                m_GameDataFileAccessor = new GameDataFileAccessor();
                m_GameDataFileAccessor.Init(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                    + @"\Dofus2\app\data\common\Effects.d2o");
                m_GameDataFileAccessor.Init(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                    + @"\Dofus2\app\data\common\Items.d2o");
                m_GameDataFileAccessor.Init(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                    + @"\Dofus2\app\data\common\ItemTypes.d2o");
            }
            else
            {
                path = @"D:\Dofus2\app\data\i18n\i18n_fr.d2i";
                i18NFileAccessor.Init(path);
                I18N i18N = new I18N(i18NFileAccessor);
                m_GameDataFileAccessor = new GameDataFileAccessor();
                m_GameDataFileAccessor.Init(@"D:\Dofus2\app\data\common\Effects.d2o");
                m_GameDataFileAccessor.Init(@"D:\Dofus2\app\data\common\Items.d2o");
                m_GameDataFileAccessor.Init(@"D:\Dofus2\app\data\common\ItemTypes.d2o");
                m_GameDataFileAccessor.Init(@"D:\Dofus2\app\data\common\Spells.d2o");
            }
        }
        #endregion

        #region Méthodes publiques
        public void Launch()
        {
            m_ConnectionThread = new Thread(Connect);
            m_ConnectionThread.Start();
        }

        //public void GetNextMeal()
        //{
        //    m_NextMeal = new DateTime();
            
        //    foreach (Bot bot in m_Bots)
        //    {
        //        if (m_NextMeal.Year == 1)
        //        {
        //            m_NextMeal = new DateTime(bot.NextMeal.Year, bot.NextMeal.Month, bot.NextMeal.Day, bot.NextMeal.Hour,
        //                bot.NextMeal.Minute, 0);
        //            continue;
        //        }

        //        if (bot.NextMeal <= m_NextMeal)
        //        {
        //            m_NextMeal = new DateTime(bot.NextMeal.Year, bot.NextMeal.Month, bot.NextMeal.Day, bot.NextMeal.Hour,
        //                bot.NextMeal.Minute, 0);
        //        }
        //    }

        //    if (m_NextMeal.Year != 1)
        //    {
        //        m_NextMeal = m_NextMeal.AddMinutes(2);

        //        DateTime difference = new DateTime((m_NextMeal - DateTime.Now).Ticks);

        //        MainForm.ActualMainForm.SetLogs(new GeneralTextInformation("Prochain repas dans " + difference.Hour + " heure(s) " +
        //            difference.Minute + " minute(s)."));

        //        if (m_TimerConnectionThread == null)
        //            m_TimerConnectionThread = new Timer(TimerConnectionThreadFinished, null, 
        //                (int)TimeSpan.FromHours(difference.Hour).TotalMilliseconds + 
        //                (int)TimeSpan.FromMinutes(difference.Minute).TotalMilliseconds, Timeout.Infinite);
        //        else
        //            m_TimerConnectionThread.Change((int)TimeSpan.FromHours(difference.Hour).TotalMilliseconds + 
        //                (int)TimeSpan.FromMinutes(difference.Minute).TotalMilliseconds, Timeout.Infinite);
        //    }
        //    else
        //    {
        //        MainForm.ActualMainForm.SetLogs(
        //            new GeneralTextInformation("Aucunes nourritures disponibles, pas de prochaine connexion."));
        //    }
        //}

        //public void TryReconnect(int minute)
        //{
        //    m_CurrentBot.SocketManager.Disconnect();

        //    MainForm.ActualMainForm.SetLogs(new ConnectionTextInformation("Reconnexion dans " + minute + " minute(s)."));

        //    if (m_TimerConnectionThread == null)
        //        m_TimerConnectionThread = new Timer(TimerConnectionThreadFinished, null, (int)TimeSpan.FromMinutes(minute).TotalMilliseconds,
        //            Timeout.Infinite);
        //    else
        //        m_TimerConnectionThread.Change((int)TimeSpan.FromMinutes(minute).TotalMilliseconds, Timeout.Infinite);

        //    m_CurrentBot.State = "En reconnexion (" + minute + ")";
        //}

        public void NextBot()
        {
            m_CurrentBot.SetNextMeal();
            m_CurrentBot.SocketManager.DisconnectFromGUI();

            m_CurrentBotIndex++;

            Connect();
        }

        public void StartFeeding()
        {
            m_Running = new Running();
            m_Running.Init();
        }
        #endregion

        #region Méthodes privées
        private void Connect()
        {
            if (m_TimerConnectionThread != null)
                m_TimerConnectionThread.Change(Timeout.Infinite, Timeout.Infinite);

            Thread.Sleep(GetRandomTime());

            m_Running = new Running();

            if (m_CurrentBotIndex < m_Bots.Count)
            {
                m_CurrentBot = m_Bots[m_CurrentBotIndex];

                if (m_CurrentBot.IsBegun)
                {
                    if ((m_CurrentBot.SocketManager != null) && (m_CurrentBot.SocketManager.Connected()))
                        return;

                    m_CurrentBot.Initialize();

                    //account.Log(new ConnectionTextInformation("Connexion."));

                    //m_CurrentBot.SocketManager = new SocketManager();

                    //m_CurrentBot.SocketManager.Connect(new ConnectionInformations("213.248.126.39", 5555, "d'identification"));
                }
                return;
            }

            m_CurrentBotIndex = 0;

           // GetNextMeal();
        }

        private static int GetRandomTime()
        {
            Random random = new Random();

            return random.Next(500, 1250);
        }

        private void TimerConnectionThreadFinished(object stateInfo)
        {
            Connect();
        }
        #endregion
    }
}
