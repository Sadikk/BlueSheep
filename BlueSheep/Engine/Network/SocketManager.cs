using BlueSheep.Common.IO;
using BlueSheep.Engine.Frame;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
namespace BlueSheep.Engine.Network
{
    public class SocketManager
    {
        #region Fields
        private Socket m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Socket m_RawSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Socket m_DofSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Thread m_ReceptionThread;
        private Thread m_DofReceptionThread;
        private Thread m_RawReceptionThread;
        private MessageInformations m_MessageInformations;
        private bool m_IsChangingServer;
        private bool m_ForcedDisconnect;
        private AccountUC account;
        public Dictionary<int, Socket> MITMClients;
        public int i = 0;
        #endregion

        #region Properties
        public Socket Socket
        {
            get { return m_Socket; }
            set { m_Socket = value; }
        }
        public bool IsChangingServer
        {
            get { return m_IsChangingServer; }
            set { m_IsChangingServer = value; }
        }
        #endregion

        #region Constructeurs
        public SocketManager(AccountUC accountform)
        {
            account = accountform;
            if (account != null)
                account.LatencyFrame = new LatencyFrame(accountform);
        }
        #endregion

        #region Public methods
        public void DisconnectFromGUI()
        {
            if (m_ReceptionThread != null)
                m_ReceptionThread.Abort();
            Disconnect();
        }
        public void Connect(ConnectionInformations connectionInformations)
        {
            //if (MainForm.ActualMainForm.IpAdressChoice.SelectedIndex > -1)
            //{
            // IPEndPoint e = new IPEndPoint(new IPAddress(GetBytes(MainForm.ActualMainForm.IpAdressChoice.SelectedText)), 0);
            // m_Socket.Bind(e);
            //}
            if (m_RawSocket != null && !m_RawSocket.Connected)
                RawConnect();
            if (m_IsChangingServer)
            {
                m_Socket.Disconnect(false);
                m_Socket.Dispose();
                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                account.State = "Déconnecté";
            }
            try
            {
                m_Socket.Connect(connectionInformations.Address, connectionInformations.Port);
                if (m_Socket.Connected)
                {
                    account.State = "Connecté";
                    account.LatencyFrame = new LatencyFrame(account);
                    account.Log(new ConnectionTextInformation("Connexion établie au serveur " + connectionInformations.ServerName + "."), 0);
                    if ((!m_IsChangingServer))
                    {
                        m_ReceptionThread = new Thread(SocketReception);
                        m_ReceptionThread.Start();
                    }
                    m_IsChangingServer = false;
                }
                else
                    account.Log(new ConnectionTextInformation("Connexion échouée."), 0);
            }
            catch (SocketException sockEx)
            {
                account.Log(new ErrorTextInformation("[Socket Exception] " + sockEx.Message), 0);
                account.TryReconnect(1);
            }
        }
        public void Disconnect()
        {
            m_ForcedDisconnect = true;
            if ((m_Socket != null) && (Connected()))
            {
                m_Socket.Disconnect(false);
                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            //if (m_TimerInactivity != null)
            // m_TimerInactivity.Dispose();
            account.Log(new ConnectionTextInformation("Déconnecté du serveur."), 0);
            account.State = "Déconnecté";
            account.ModifBar(6, 0, 0, "Déconnecté");
            //account.Dispose();
            //MainForm.ActualMainForm.ActualizeAccountInformations();
        }
        public void DisconnectFromDofusClient()
        {
            m_DofReceptionThread.Abort();
            m_DofSocket.Shutdown(SocketShutdown.Both);
            m_DofSocket.Disconnect(false);
            m_DofSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Send(byte[] content)
        {
            if ((m_Socket != null) && (Connected()))
            {
                account.LatencyFrame.LowSend();
                m_Socket.Send(content);
            }
        }
        public void Send(Message msg)
        {
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                msg.Serialize(writer);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
                account.Log(new BotTextInformation("SND : " + msg.ProtocolID), 0);
            }
        }
        public bool Connected()
        {
            return !((m_Socket.Poll(1000, SelectMode.SelectRead) && (m_Socket.Available == 0)) || !m_Socket.Connected);
        }
        public void RawConnect()
        {
            //m_RawSocket.Connect("127.0.0.1", 6666);
            //if (m_RawSocket.Connected)
            //{
            //    account.Log(new ConnectionTextInformation("Connexion établie au serveur de sécurité"), 0);
            //    m_RawReceptionThread = new Thread(RawReception);
            //    m_RawReceptionThread.Start();
            //}
            //else
            //{
            //    account.Log(new ErrorTextInformation("Echec de la connexion serveur de sécurité. Contactez l'administrateur."), 0);
            //    DisconnectFromGUI();
            //}
        }
        public void SendRaw(byte[] rdm)
        {
            if (m_RawSocket != null && m_RawSocket.Connected)
                m_RawSocket.Send(rdm);
        }
        public void SendToDofusClient(byte[] buffer)
        {
            if ((m_DofSocket != null) && (m_DofSocket.Connected))
                m_DofSocket.Send(buffer);
        }
        #endregion

        #region Private methods
        private void SocketReception()
        {
            m_MessageInformations = new MessageInformations(account);
            while ((m_Socket != null) && (Connected()))
            {
                byte[] buffer = new byte[m_Socket.Available];
                if (buffer.Length != 0)
                {
                    m_Socket.Receive(buffer);
                    account.LatencyFrame.UpdateLatency();
                    m_MessageInformations.ParseBuffer(buffer);
                }
            }
            if (!m_ForcedDisconnect)
            {
                account.Log(
                new ErrorTextInformation("La connexion a été interrompue à cause d'une raison inconnue."), 0);
                if (account.IsMITM)
                {
                    account.Log(new ErrorTextInformation("MITM : Impossible de se reconnecter."), 0);
                    DisconnectServer();
                }
                else
                    account.TryReconnect(2);
            }
            m_ForcedDisconnect = false;
        }
        private void DofSocketReception()
        {
            while ((m_DofSocket != null) && (m_DofSocket.Connected))
            {
                byte[] buffer = new byte[m_DofSocket.Available];
                if (buffer.Length != 0)
                {
                    m_DofSocket.Receive(buffer);
                    account.LatencyFrame.UpdateLatency();
                    m_Socket.Send(buffer);
                }
            }
            if (!m_ForcedDisconnect)
            {
                account.Log(new ErrorTextInformation("La connexion a été interrompue à cause d'une raison inconnue."), 0);
                account.Log(new ErrorTextInformation("MITM : Impossible de se reconnecter."), 0);
            }
            m_ForcedDisconnect = false;
        }
        private void RawReception()
        {
            while (m_RawSocket != null && m_RawSocket.Connected)
            {
                byte[] buffer = new byte[m_RawSocket.Available];
                if (buffer.Length != 0)
                {
                    m_RawSocket.Receive(buffer);
                    m_MessageInformations.ParseBuffer(buffer);
                }
            }
        }
        private void TimerInactivityFinished(object statusInfos)
        {
            //m_TimerInactivity.Change(Timeout.Infinite, Timeout.Infinite);
            account.Log(new ErrorTextInformation("Inactivité prolongée."), 0);
            account.TryReconnect(1);
        }
        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public void ListenDofus()
        {
            TcpListener srv = null;
            srv = new TcpListener(IPAddress.Any, 5555);
            srv.Start();
            account.Log(new BotTextInformation(" >> Listening dofus client started"), 0);
            while (true)
            {
                m_DofSocket = srv.AcceptSocket();
                if (m_DofSocket.Connected)
                {
                    account.Log(new BotTextInformation(" >> Dofus is here"), 0);
                    if (!m_IsChangingServer)
                    {
                        Connect(new ConnectionInformations("213.248.126.39", 5555, "d'identification"));
                    }
                    m_DofReceptionThread = new Thread(new ThreadStart(DofSocketReception));
                    m_DofReceptionThread.Start();
                    return;
                }
            }
            srv.Stop();
        }
        public void DisconnectServer()
        {
            this.m_DofReceptionThread.Abort();
            this.m_DofSocket.Shutdown(SocketShutdown.Both);
            this.m_DofSocket.Close();
        }
        #endregion
    }
}