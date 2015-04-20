using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace BlueSheep.Engine.Types
{
    public class WatchDog
    {
        #region Fields
        private AccountUC m_Account;
        private DateTime PathAction;
        private Thread m_PathDog;
        #endregion

        #region Constructors
        public WatchDog(AccountUC account)
        {
            m_Account = account;
        }
        #endregion

        #region Public Methods
        public void StartPathDog()
        {
            m_PathDog = new Thread(new ThreadStart(PathDog));
            m_PathDog.Start();
        }

        public void StopPathDog()
        {
            if (m_PathDog != null)
                m_PathDog.Abort();
        }

        public void Update()
        {
            PathAction = DateTime.Now;
        }
        #endregion

        #region Private Methods
        private void PathDog()
        {
            DateTime now = PathAction;
            double endwait = Environment.TickCount + 10000;
            while (Environment.TickCount < endwait)
            {
                System.Threading.Thread.Sleep(1);
                System.Windows.Forms.Application.DoEvents();
            }
            DateTime after = PathAction;
            if (DateTime.Compare(now, after) == 0 && CheckState())
            {
                m_Account.Log(new DebugTextInformation("[WatchDog] Relaunch path"), 0);
                m_Account.Path.ParsePath();
                StartPathDog();
            }
            else
            {
                m_Account.Log(new DebugTextInformation("[WatchDog] Nothing to do."), 0);
                StartPathDog();
            }
        }

        private bool CheckState()
        {
            return (m_Account.state == Enums.Status.None ||
                m_Account.state == Enums.Status.Moving ||
                m_Account.state == Enums.Status.Busy);
        }
        #endregion
    }
}
