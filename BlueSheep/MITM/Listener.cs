using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;

namespace BlueSheep.MITM
{
    class Listener
    {
        #region Attributs
        TcpListener tcp_listener;
        Thread accept_thread;
        bool startstop;
        List<object> ClientsList;
        bool Connected;
        BigEndianReader m_Reader;
        private MessageInformations m_MessageInformations;
        Socket _mitmGameListenerSocket;
        #endregion

        #region Constructeurs
        public Listener(int Port)
        {
            tcp_listener = new TcpListener(IPAddress.Any, Port);
            accept_thread = new Thread(new ThreadStart(accept));
            accept_thread.Start();
        }
        #endregion

        #region Private Methodes
        private void accept()
        {
            while (startstop)
            {
                if (tcp_listener.Pending())
                {
                    TcpClient client = tcp_listener.AcceptTcpClient();
                    ClientsList.Add(client);
                    AccountUC Account = new AccountUC(null, null);
                    listen(client, Account);
                }
            }
        }

        private void listen(TcpClient client, AccountUC account)
        {
                m_Reader = new BigEndianReader(client.GetStream());
                //m_MessageInformations = new MessageInformations(account);

                //while ((client.Client != null) && (client.Client.Connected))
                //{
                //    byte[] buffer = new byte[client.Client.Available];

                //    if (buffer.Length != 0)
                //    {
                //        if (m_TimerInactivity != null)
                //            m_TimerInactivity.Change(60000, Timeout.Infinite);
                //        else
                //            m_TimerInactivity = new Timer(TimerInactivityFinished, null, 60000, Timeout.Infinite);

                //        client.Client.Receive(buffer);

                //        account.LatencyFrame.UpdateLatency();
                //        m_MessageInformations.ParseBuffer(buffer);
                //    }
                //}


                Connected = true;
                Thread task_id = new Thread(computeid);
                task_id.Start();
            
        }

        private void computeid()
        {
            while (Connected)
            {
                Thread.Sleep(1);
                ushort num = m_Reader.ReadUShort();
            }
        }

        private void StartMitmGameServer()
        {
            _mitmGameListenerSocket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _mitmGameListenerSocket.Bind(new IPEndPoint(IPAddress.Any, 5556)); // _mitmGamePort = 5556
            _mitmGameListenerSocket.Listen(10);

            AsyncCallback acceptSocketAsyncCallback = new AsyncCallback(MitmGameListenerAcceptSocket);
            _mitmGameListenerSocket.BeginAccept(acceptSocketAsyncCallback, _mitmGameListenerSocket);
        }

        private void MitmGameListenerAcceptSocket(IAsyncResult result)
        {
            //_gameClientSocket = _mitmGameListenerSocket.EndAccept(result);

            //Thread receiveDataFromGameClient = new Thread(new ThreadStart(ReceiveDataFromGameClient));
            //receiveDataFromGameClient.Start();


            //AsyncCallback acceptSocketAsyncCallback = new AsyncCallback(MitmGameListenerAcceptSocket);
            //_mitmGameListenerSocket.BeginAccept(acceptSocketAsyncCallback, _mitmGameListenerSocket);
        }

        private void treat(UInt16 num)
        {

        }
        #endregion
    }
}
