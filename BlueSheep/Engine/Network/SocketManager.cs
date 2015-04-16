//using BlueSheep.Common.IO;
//using BlueSheep.Engine.Enums;
//using BlueSheep.Engine.Frame;
//using BlueSheep.Engine.Types;
//using BlueSheep.Interface;
//using BlueSheep.Interface.Text;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.NetworkInformation;
//using System.Net.Sockets;
//using System.Threading;
//using System.Windows.Forms;
//namespace BlueSheep.Engine.Network
//{
//    public class SocketManager
//    {
//        #region Fields
//        private Socket m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        private Socket m_RawSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        private Socket m_DofSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        private Thread m_ReceptionThread;
//        private Thread m_DofReceptionThread;
//        private Thread m_RawReceptionThread;
//        private MessageInformations m_MessageInformations;
//        private bool m_IsChangingServer;
//        private bool m_ForcedDisconnect;
//        private AccountUC account;
//        public Dictionary<int, Socket> MITMClients;
//        public int i = 0;
//        #endregion

//        #region Properties
//        public Socket Socket
//        {
//            get { return m_Socket; }
//            set { m_Socket = value; }
//        }
//        public bool IsChangingServer
//        {
//            get { return m_IsChangingServer; }
//            set { m_IsChangingServer = value; }
//        }
//        #endregion

//        #region Constructeurs
//        public SocketManager(AccountUC accountform)
//        {
//            account = accountform;
//            if (account != null)
//                account.LatencyFrame = new LatencyFrame(accountform);
//        }
//        #endregion

//        #region Public methods
//        public void DisconnectFromGUI()
//        {
//            if (account.Path != null)
//                account.Path.StopPath();
//            if (m_ReceptionThread != null)
//                m_ReceptionThread.Abort();
//            Disconnect();
//        }
//        public void Connect(ConnectionInformations connectionInformations)
//        {
//            //if (MainForm.ActualMainForm.IpAdressChoice.SelectedIndex > -1)
//            //{
//            // IPEndPoint e = new IPEndPoint(new IPAddress(GetBytes(MainForm.ActualMainForm.IpAdressChoice.SelectedText)), 0);
//            // m_Socket.Bind(e);
//            //}
//            if (m_RawSocket != null && !m_RawSocket.Connected)
//                RawConnect();
//            if (m_IsChangingServer)
//            {
//                m_Socket.Disconnect(false);
//                m_Socket.Dispose();
//                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//                account.SetStatus(Status.Disconnected);
//            }
//            try
//            {
//                m_Socket.Connect(connectionInformations.Address, connectionInformations.Port);
//                if (m_Socket.Connected)
//                {
//                    account.SetStatus(Status.None);
//                    account.LatencyFrame = new LatencyFrame(account);
//                    account.Log(new ConnectionTextInformation("Connexion établie au serveur " + connectionInformations.ServerName + "."), 0);
//                    if ((!m_IsChangingServer))
//                    {
//                        m_ReceptionThread = new Thread(SocketReception);
//                        m_ReceptionThread.Start();
//                    }
//                    m_IsChangingServer = false;
//                }
//                else
//                    account.Log(new ConnectionTextInformation("Connexion échouée."), 0);
//            }
//            catch (SocketException sockEx)
//            {
//                account.Log(new ErrorTextInformation("[Socket Exception] " + sockEx.Message), 0);
//                account.TryReconnect(1);
//            }
//        }
//        public void Disconnect()
//        {
            
//            m_ForcedDisconnect = true;
//            if ((m_Socket != null) && (Connected()))
//            {
//                m_Socket.Shutdown(SocketShutdown.Both);
//                m_Socket.Close();
//                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            }
//            //if (m_TimerInactivity != null)
//            // m_TimerInactivity.Dispose();
//            if (!account.Disposing)
//            {
//                account.SetStatus(Status.Disconnected);
//                account.Log(new ConnectionTextInformation("Déconnecté du serveur."), 0);
//            }
//            //account.Dispose();
//            //MainForm.ActualMainForm.ActualizeAccountInformations();
//        }
//        public void DisconnectFromDofusClient()
//        {
//            m_DofReceptionThread.Abort();
//            m_DofSocket.Shutdown(SocketShutdown.Both);
//            m_DofSocket.Close();
//            m_DofSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        }

//        public void Send(byte[] content)
//        {
//            if ((m_Socket != null) && (Connected()))
//            {
//                account.LatencyFrame.LowSend();
//                m_Socket.Send(content);
//            }
//        }
//        public void Send(BlueSheep.Engine.Types.Message msg)
//        {
//            using (BigEndianWriter writer = new BigEndianWriter())
//            {
//                msg.Serialize(writer);
//                MessagePackaging pack = new MessagePackaging(writer);
//                pack.Pack((int)msg.ProtocolID);
//                account.SocketManager.Send(pack.Writer.Content);
//                if (account.DebugMode.Checked)
//                    account.Log(new DebugTextInformation("[SND] " + msg.ProtocolID), 0);
//            }
//        }
//        public bool Connected()
//        {
//            return !((m_Socket.Poll(1000, SelectMode.SelectRead) && (m_Socket.Available == 0)) || !m_Socket.Connected);
//        }
//        public void RawConnect()
//        {
//            //m_RawSocket.Connect("127.0.0.1", 6666);
//            //if (m_RawSocket.Connected)
//            //{
//            //    account.Log(new ConnectionTextInformation("Connexion établie au serveur de sécurité."), 0);
//            //    m_RawReceptionThread = new Thread(RawReception);
//            //    m_RawReceptionThread.Start();
//            //}
//            //else
//            //{
//            //    account.Log(new ErrorTextInformation("Echec de la connexion serveur de sécurité. Contactez l'administrateur."), 0);
//            //    DisconnectFromGUI();
//            //}
//        }
//        public void SendRaw(byte[] rdm)
//        {
//            if (m_RawSocket != null && m_RawSocket.Connected)
//                m_RawSocket.Send(rdm);
//        }
//        public void SendToDofusClient(byte[] buffer)
//        {
//            if ((m_DofSocket != null) && (m_DofSocket.Connected))
//                m_DofSocket.Send(buffer);
//        }
//        #endregion

//        #region Private methods
//        private void SocketReception()
//        {
//            try
//            {
//                m_MessageInformations = new MessageInformations(account);
//                while ((m_Socket != null) && (Connected()))
//                {
//                    byte[] buffer = new byte[m_Socket.Available];
//                    if (buffer.Length != 0)
//                    {
//                        m_Socket.Receive(buffer);
//                        account.LatencyFrame.UpdateLatency();
//                        m_MessageInformations.ParseBuffer(buffer);
//                    }
//                }
//                if (!m_ForcedDisconnect)
//                {
//                    account.Log(
//                    new ErrorTextInformation("La connexion a été interrompue à cause d'une raison inconnue."), 0);
//                    if (account.IsMITM)
//                    {
//                        account.Log(new ErrorTextInformation("MITM : Impossible de se reconnecter."), 0);
//                        DisconnectServer();
//                    }
//                    else
//                        account.TryReconnect(2);
//                }
//                m_ForcedDisconnect = false;
//            }
//            catch (SocketException ex)
//            {
//                account.Log(new ErrorTextInformation(ex.Message + ex.StackTrace), 0); 
//                account.TryReconnect(2);
//            }
//        }
//        private void DofSocketReception()
//        {
//            while ((m_DofSocket != null) && (m_DofSocket.Connected))
//            {
//                byte[] buffer = new byte[m_DofSocket.Available];
//                if (buffer.Length != 0)
//                {
//                    m_DofSocket.Receive(buffer);
//                    account.LatencyFrame.UpdateLatency();
//                    m_Socket.Send(buffer);
//                }
//            }
//            if (!m_ForcedDisconnect)
//            {
//                account.Log(new ErrorTextInformation("La connexion a été interrompue à cause d'une raison inconnue."), 0);
//                account.Log(new ErrorTextInformation("MITM : Impossible de se reconnecter."), 0);
//            }
//            m_ForcedDisconnect = false;
//        }
//        private void RawReception()
//        {
//            while (m_RawSocket != null && m_RawSocket.Connected)
//            {
//                byte[] buffer = new byte[m_RawSocket.Available];
//                if (buffer.Length != 0)
//                {
//                    m_RawSocket.Receive(buffer);
//                    m_MessageInformations.ParseBuffer(buffer);
//                }
//            }
//        }
//        private void TimerInactivityFinished(object statusInfos)
//        {
//            account.Log(new ErrorTextInformation("Inactivité prolongée."), 0);
//            account.TryReconnect(1);
//        }
//        private byte[] GetBytes(string str)
//        {
//            byte[] bytes = new byte[str.Length * sizeof(char)];
//            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
//            return bytes;
//        }
//        public void ListenDofus()
//        {
//            TcpListener srv = null;
//            srv = new TcpListener(IPAddress.Any, 5555);
//            srv.Start();
//            account.Log(new BotTextInformation(" >> Listening dofus client started"), 0);
//            while (true)
//            {
//                Application.DoEvents();
//                m_DofSocket = srv.AcceptSocket();
//                if (m_DofSocket.Connected)
//                {
//                    account.Log(new BotTextInformation(" >> Dofus is here"), 0);
//                    if (!m_IsChangingServer)
//                    {
//                        Connect(new ConnectionInformations("213.248.126.39", 5555, "d'identification"));
//                    }
//                    m_DofReceptionThread = new Thread(new ThreadStart(DofSocketReception));
//                    m_DofReceptionThread.Start();
//                    return;
//                }
//            }
//            srv.Stop();
//        }
//        public void DisconnectServer()
//        {
//            this.m_DofReceptionThread.Abort();
//            this.m_DofSocket.Shutdown(SocketShutdown.Both);
//            this.m_DofSocket.Close();
//        }
//        #endregion
//    }
//}

using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Engine.Frame;
using BlueSheep.Interface.Text;
using BlueSheep.Common.IO;

namespace BlueSheep.Engine.Network
{
    #region Enums
    public enum SocketState
    {
        Closed,
        Closing,
        Connected,
        Connecting,
        Listening,
    }
    #endregion

    #region Event Args
    public class NetSocketConnectedEventArgs : EventArgs
    {
        public IPAddress SourceIP;
        public NetSocketConnectedEventArgs(IPAddress ip)
        {
            this.SourceIP = ip;
        }
    }

    public class NetSocketDisconnectedEventArgs : EventArgs
    {
        public string Reason;
        public NetSocketDisconnectedEventArgs(string reason)
        {
            this.Reason = reason;
        }
    }

    public class NetSockStateChangedEventArgs : EventArgs
    {
        public SocketState NewState;
        public SocketState PrevState;
        public NetSockStateChangedEventArgs(SocketState newState, SocketState prevState)
        {
            this.NewState = newState;
            this.PrevState = prevState;
        }
    }

    public class NetSockDataArrivalEventArgs : EventArgs
    {
        public byte[] Data;
        public NetSockDataArrivalEventArgs(byte[] data)
        {
            this.Data = data;
        }
    }

    public class NetSockErrorReceivedEventArgs : EventArgs
    {
        public string Function;
        public Exception Exception;
        public NetSockErrorReceivedEventArgs(string function, Exception ex)
        {
            this.Function = function;
            this.Exception = ex;
        }
    }

    public class NetSockConnectionRequestEventArgs : EventArgs
    {
        public Socket Client;
        public NetSockConnectionRequestEventArgs(Socket client)
        {
            this.Client = client;
        }
    }
    #endregion

    #region Socket Classes
    public abstract class NetBase
    {
        #region Fields
        /// <summary>Current socket state</summary>
        protected SocketState state = SocketState.Closed;
        /// <summary>The socket object, obviously</summary>
        protected Socket socket;

        /// <summary>Keep track of when data is being sent</summary>
        protected bool isSending = false;

        /// <summary>Queue of objects to be sent out</summary>
        protected Queue<byte[]> sendBuffer = new Queue<byte[]>();

        /// <summary>Store incoming bytes to be processed</summary>
        protected byte[] byteBuffer = new byte[8192];

        /// <summary>Position of the bom header in the rxBuffer</summary>
        protected int rxHeaderIndex = -1;
        /// <summary>Expected length of the message from the bom header</summary>
        protected int rxBodyLen = -1;
        /// <summary>Buffer of received data</summary>
        protected MemoryStream rxBuffer = new MemoryStream();

        /// <summary>TCP inactivity before sending keep-alive packet (ms)</summary>
        protected uint KeepAliveInactivity = 500;
        /// <summary>Interval to send keep-alive packet if acknowledgement was not received (ms)</summary>
        protected uint KeepAliveInterval = 100;

        /// <summary>Threaded timer checks if socket is busted</summary>
        protected Timer connectionTimer;
        /// <summary>Interval for socket checks (ms)</summary>
        protected int ConnectionCheckInterval = 1000;
        #endregion

        #region Public Properties
        /// <summary>Current state of the socket</summary>
        public SocketState State { get { return this.state; } }

        /// <summary>Port the socket control is listening on.</summary>
        public int LocalPort
        {
            get
            {
                try
                {
                    return ((IPEndPoint)this.socket.LocalEndPoint).Port;
                }
                catch
                {
                    return -1;
                }
            }
        }

        /// <summary>IP address enumeration for local computer</summary>
        public static string[] LocalIP
        {
            get
            {
                IPHostEntry h = Dns.GetHostEntry(Dns.GetHostName());
                List<string> s = new List<string>(h.AddressList.Length);
                foreach (IPAddress i in h.AddressList)
                    s.Add(i.ToString());
                return s.ToArray();
            }
        }
        #endregion

        #region Events
        /// <summary>Socket is connected</summary>
        public event EventHandler<NetSocketConnectedEventArgs> Connected;
        /// <summary>Socket connection closed</summary>
        public event EventHandler<NetSocketDisconnectedEventArgs> Disconnected;
        /// <summary>Socket state has changed</summary>
        /// <remarks>This has the ability to fire very rapidly during connection / disconnection.</remarks>
        public event EventHandler<NetSockStateChangedEventArgs> StateChanged;
        /// <summary>Recived a new object</summary>
        public event EventHandler<NetSockDataArrivalEventArgs> DataArrived;
        /// <summary>An error has occurred</summary>
        public event EventHandler<NetSockErrorReceivedEventArgs> ErrorReceived;
        #endregion

        #region Constructor
        /// <summary>Base constructor sets up buffer and timer</summary>
        public NetBase()
        {
            this.connectionTimer = new Timer(
                new TimerCallback(this.connectedTimerCallback),
                null, Timeout.Infinite, Timeout.Infinite);
        }
        #endregion

        #region Send
        /// <summary>Send data</summary>
        /// <param name="bytes">Bytes to send</param>
        public void Send(byte[] data)
        {
            try
            {
                if (data == null)
                    throw new NullReferenceException("data cannot be null");
                else if (data.Length == 0)
                    throw new NullReferenceException("data cannot be empty");
                else
                {
                    lock (this.sendBuffer)
                    {
                        this.sendBuffer.Enqueue(data);
                    }

                    if (!this.isSending)
                    {
                        this.isSending = true;
                        this.SendNextQueued();
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("Send", ex);
            }
        }

        /// <summary>Send data for real</summary>
        private void SendNextQueued()
        {
            try
            {
                if (IsConnected(this.socket))
                {
                    List<ArraySegment<byte>> send = new List<ArraySegment<byte>>(3);
                    int length = 0;
                    lock (this.sendBuffer)
                    {
                        if (this.sendBuffer.Count == 0)
                        {
                            this.isSending = false;
                            return; // nothing more to send
                        }

                        byte[] data = this.sendBuffer.Dequeue();
                        send.Add(new ArraySegment<byte>(data));

                        length = data.Length;
                    }
                    this.socket.BeginSend(send, SocketFlags.None, new AsyncCallback(this.SendCallback), this.socket);
                }
                else
                {
                    Close("Lost sync with server. Try to reconnect.");
                }
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("Sending", ex);
            }
        }

        /// <summary>Callback for BeginSend</summary>
        /// <param name="ar"></param>
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket sock = (Socket)ar.AsyncState;
                int didSend = sock.EndSend(ar);

                if (this.socket != sock)
                {
                    this.Close("Async Connect Socket mismatched");
                    return;
                }

                this.SendNextQueued();
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.ConnectionReset)
                    this.Close("Remote Socket Closed");
                else
                    throw;
            }
            catch (Exception ex)
            {
                this.Close("Socket Send Exception");
                this.OnErrorReceived("Socket Send", ex);
            }
        }
        #endregion

        #region Close
        /// <summary>Disconnect the socket</summary>
        /// <param name="reason"></param>
        public void Close(string reason)
        {
            try
            {
                if (this.state == SocketState.Closing || this.state == SocketState.Closed)
                    return; // already closing/closed

                this.OnChangeState(SocketState.Closing);

                if (this.socket != null)
                {
                    this.socket.Close();
                    this.socket = null;
                }
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("Close", ex);
            }

            try
            {
                //lock (this.rxBuffer)
                //{
                    this.rxBuffer.SetLength(0);
                //}
                //lock (this.sendBuffer)
                //{
                    this.sendBuffer.Clear();
                    this.isSending = false;
                //}
                this.OnChangeState(SocketState.Closed);
                if (this.Disconnected != null)
                    this.Disconnected(this, new NetSocketDisconnectedEventArgs(reason));
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("Close Cleanup", ex);
            }
        }
        #endregion

        #region Receive
        /// <summary>Receive data asynchronously</summary>
        protected void Receive()
        {
            try
            {
                this.socket.BeginReceive(this.byteBuffer, 0, this.byteBuffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), this.socket);
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("Receive", ex);
            }
        }

        /// <summary>Callback for BeginReceive</summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket sock = (Socket)ar.AsyncState;
                int size = sock.EndReceive(ar);
                if (this.socket != sock)
                {
                    this.Close("Async Receive Socket mismatched");
                    return;
                }
                if (size < 1)
                {
                    //this.Close("No Bytes Received");
                    return;
                }

                lock (this.rxBuffer)
                {
                    // put at the end for safe writing
                    this.rxBuffer.Position = this.rxBuffer.Length;
                    this.rxBuffer.Write(this.byteBuffer, 0, size);

                    bool more = false;
                    do
                    {
                        try
                        {
                            //this.rxBuffer.Position = this.rxHeaderIndex + this.bomBytes.Count + sizeof(int);
                            this.rxBuffer.Position = 0;
                            byte[] data = new byte[this.rxBuffer.Length];
                            this.rxBuffer.Read(data, 0, data.Length);
                            if (this.DataArrived != null)
                                this.DataArrived(this, new NetSockDataArrivalEventArgs(data));
                        }
                        catch (Exception ex)
                        {
                            this.OnErrorReceived("Receiving", ex);
                        }

                        if (this.rxBuffer.Position == this.rxBuffer.Length)
                        {
                            // no bytes left
                            // just resize buffer
                            this.rxBuffer.SetLength(0);
                            this.rxBuffer.Capacity = this.byteBuffer.Length;
                            more = false;
                        }
                        else
                        {
                            // leftover bytes after current message
                            // copy these bytes to the beginning of the rxBuffer
                            this.CopyBack();
                            more = true;
                        }

                        // reset header info
                        this.rxHeaderIndex = -1;
                        this.rxBodyLen = -1;

                    } while (more);
                }
                if (this.socket != null)
                    this.socket.BeginReceive(this.byteBuffer, 0, this.byteBuffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), this.socket);
            }
            catch (ObjectDisposedException)
            {
                return; // socket disposed, let it die quietly
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.ConnectionReset)
                    this.Close("Remote Socket Closed");
                else
                    this.Close("Unknown error. Code : " + ex.ErrorCode);
            }
            catch (Exception ex)
            {
                this.Close("Socket Receive Exception");
                this.OnErrorReceived("Socket Receive", ex);
            }
        }

        /// <summary>
        /// Copies the stuff after the current position, back to the start of the stream,
        /// resizes the stream to only include the new content, and
        /// limits the capacity to length + another buffer.
        /// </summary>
        private void CopyBack()
        {
            int count;
            long readPos = this.rxBuffer.Position;
            long writePos = 0;
            do
            {
                count = this.rxBuffer.Read(this.byteBuffer, 0, this.byteBuffer.Length);
                readPos = this.rxBuffer.Position;
                this.rxBuffer.Position = writePos;
                this.rxBuffer.Write(this.byteBuffer, 0, count);
                writePos = this.rxBuffer.Position;
                this.rxBuffer.Position = readPos;
            }
            while (count > 0);
            this.rxBuffer.SetLength(writePos);
            this.rxBuffer.Capacity = (int)this.rxBuffer.Length + this.byteBuffer.Length;
        }

        /// <summary>
        /// Return if we are connected or not
        /// </summary>
        private bool IsConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        /// <summary>Find first position the specified byte within the stream, or -1 if not found</summary>
        /// <param name="ms"></param>
        /// <param name="find"></param>
        /// <returns></returns>
        private int IndexOfByteInStream(MemoryStream ms, byte find)
        {
            int b;
            do
            {
                b = ms.ReadByte();
            } while (b > -1 && b != find);

            if (b == -1)
                return -1;
            else
                return (int)ms.Position - 1; // position is +1 byte after the byte we want
        }

        /// <summary>Find first position the specified bytes within the stream, or -1 if not found</summary>
        /// <param name="ms"></param>
        /// <param name="find"></param>
        /// <returns></returns>
        private int IndexOfBytesInStream(MemoryStream ms, byte[] find)
        {
            int index;
            do
            {
                index = this.IndexOfByteInStream(ms, find[0]);

                if (index > -1)
                {
                    bool found = true;
                    for (int i = 1; i < find.Length; i++)
                    {
                        if (find[i] != ms.ReadByte())
                        {
                            found = false;
                            ms.Position = index + 1;
                            break;
                        }
                    }
                    if (found)
                        return index;
                }
            } while (index > -1);
            return -1;
        }
        #endregion

        #region OnEvents
        protected void OnErrorReceived(string function, Exception ex)
        {
            if (this.ErrorReceived != null)
                this.ErrorReceived(this, new NetSockErrorReceivedEventArgs(function, ex));
        }

        protected void OnConnected(Socket sock)
        {
            if (this.Connected != null)
                this.Connected(this, new NetSocketConnectedEventArgs(((IPEndPoint)sock.RemoteEndPoint).Address));
        }

        protected void OnChangeState(SocketState newState)
        {
            SocketState prev = this.state;
            this.state = newState;
            if (this.StateChanged != null)
                this.StateChanged(this, new NetSockStateChangedEventArgs(this.state, prev));

            if (this.state == SocketState.Connected)
                this.connectionTimer.Change(0, this.ConnectionCheckInterval);
            else if (this.state == SocketState.Closed)
                this.connectionTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        #endregion

        #region Keep-alives
        /*
		 * Note about usage of keep-alives
		 * The TCP protocol does not successfully detect "abnormal" socket disconnects at both
		 * the client and server end. These are disconnects due to a computer crash, cable 
		 * disconnect, or other failure. The keep-alive mechanism built into the TCP socket can
		 * detect these disconnects by essentially sending null data packets (header only) and
		 * waiting for acks.
		 */

        /// <summary>Structure for settings keep-alive bytes</summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct tcp_keepalive
        {
            /// <summary>1 = on, 0 = off</summary>
            public uint onoff;
            /// <summary>TCP inactivity before sending keep-alive packet (ms)</summary>
            public uint keepalivetime;
            /// <summary>Interval to send keep-alive packet if acknowledgement was not received (ms)</summary>
            public uint keepaliveinterval;
        }

        /// <summary>Set up the socket to use TCP keep alive messages</summary>
        protected void SetKeepAlive()
        {
            try
            {
                tcp_keepalive sioKeepAliveVals = new tcp_keepalive();
                sioKeepAliveVals.onoff = (uint)1; // 1 to enable 0 to disable
                sioKeepAliveVals.keepalivetime = this.KeepAliveInactivity;
                sioKeepAliveVals.keepaliveinterval = this.KeepAliveInterval;

                IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(sioKeepAliveVals));
                Marshal.StructureToPtr(sioKeepAliveVals, p, true);
                byte[] inBytes = new byte[Marshal.SizeOf(sioKeepAliveVals)];
                Marshal.Copy(p, inBytes, 0, inBytes.Length);
                Marshal.FreeHGlobal(p);

                byte[] outBytes = BitConverter.GetBytes(0);
                if (IsConnected(this.socket))
                    this.socket.IOControl(IOControlCode.KeepAliveValues, inBytes, outBytes);
                else
                    Close("Currently disconnected. Try to reconnect.");
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("Keep Alive", ex);
            }
        }
        #endregion

        #region Connection Sanity Check
        private void connectedTimerCallback(object sender)
        {
            try
            {
                if (this.state == SocketState.Connected &&
                    (this.socket == null || !this.socket.Connected))
                    this.Close("Connect Timer");
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("ConnectTimer", ex);
                this.Close("Connect Timer Exception");
            }
        }
        #endregion
    }

    public class NetServer : NetBase
    {
        #region Events
        /// <summary>A socket has requested a connection</summary>
        public event EventHandler<NetSockConnectionRequestEventArgs> ConnectionRequested;
        #endregion

        #region Listen
        /// <summary>Listen for incoming connections</summary>
        /// <param name="port">Port to listen on</param>
        public void Listen(int port)
        {
            try
            {
                if (this.socket != null)
                {
                    try
                    {
                        this.socket.Close();
                    }
                    catch { }; // ignore problems with old socket
                }
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);
                this.socket.Bind(ipLocal);
                this.socket.Listen(1);
                this.socket.BeginAccept(new AsyncCallback(this.AcceptCallback), this.socket);
                this.OnChangeState(SocketState.Listening);
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("Listen", ex);
            }
        }

        /// <summary>Callback for BeginAccept</summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket listener = (Socket)ar.AsyncState;
                Socket sock = listener.EndAccept(ar);

                if (this.state == SocketState.Listening)
                {
                    if (this.socket != listener)
                    {
                        this.Close("Async Listen Socket mismatched");
                        return;
                    }

                    if (this.ConnectionRequested != null)
                        this.ConnectionRequested(this, new NetSockConnectionRequestEventArgs(sock));
                }

                if (this.state == SocketState.Listening)
                    this.socket.BeginAccept(new AsyncCallback(this.AcceptCallback), listener);
                else
                {
                    try
                    {
                        listener.Close();
                    }
                    catch (Exception ex)
                    {
                        this.OnErrorReceived("Close Listen Socket", ex);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (SocketException ex)
            {
                this.Close("Listen Socket Exception");
                this.OnErrorReceived("Listen Socket", ex);
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("Listen Socket", ex);
            }
        }
        #endregion

        #region Accept
        /// <summary>Accept the connection request</summary>
        /// <param name="client">Client socket to accept</param>
        public void Accept(Socket client)
        {
            try
            {
                if (this.state != SocketState.Listening)
                    throw new Exception("Cannot accept socket is " + this.state.ToString());

                if (this.socket != null)
                {
                    try
                    {
                        this.socket.Close(); // close listening socket
                    }
                    catch { } // don't care if this fails
                }

                this.socket = client;

                this.socket.ReceiveBufferSize = this.byteBuffer.Length;
                this.socket.SendBufferSize = this.byteBuffer.Length;

                this.SetKeepAlive();

                this.OnChangeState(SocketState.Connected);
                this.OnConnected(this.socket);

                this.Receive();
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("Accept", ex);
            }
        }
        #endregion
    }

    public class NetClient : NetBase
    {
        #region Constructor
        public NetClient() : base() { }
        #endregion

        #region Connect
        /// <summary>Connect to the computer specified by Host and Port</summary>
        public void Connect(IPEndPoint endPoint)
        {
            if (this.state == SocketState.Connected)
                return; // already connecting to something

            try
            {
                if (this.state != SocketState.Closed)
                    throw new Exception("Cannot connect socket is " + this.state.ToString());

                this.OnChangeState(SocketState.Connecting);

                if (this.socket == null)
                    this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                this.socket.BeginConnect(endPoint, new AsyncCallback(this.ConnectCallback), this.socket);
            }
            catch (Exception ex)
            {
                this.OnErrorReceived("Connect", ex);
                this.Close("Connect Exception");
            }
        }

        /// <summary>Callback for BeginConnect</summary>
        /// <param name="ar"></param>
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket sock = (Socket)ar.AsyncState;
                sock.EndConnect(ar);

                if (this.socket != sock)
                {
                    this.Close("Async Connect Socket mismatched");
                    return;
                }

                if (this.state != SocketState.Connecting)
                    throw new Exception("Cannot connect socket is " + this.state.ToString());

                this.socket.ReceiveBufferSize = this.byteBuffer.Length;
                this.socket.SendBufferSize = this.byteBuffer.Length;

                this.SetKeepAlive();

                this.OnChangeState(SocketState.Connected);
                this.OnConnected(this.socket);

                this.Receive();
            }
            catch (Exception ex)
            {
                this.Close("Socket Connect Exception");
                this.OnErrorReceived("Socket Connect", ex);
            }
        }
        #endregion
    }
    #endregion
}

namespace BlueSheep.Engine.Network
{
    public class SocketManager
    {
        #region Fields
        private MessageInformations m_MessageInformations;
        private AccountUC account;
        private NetClient client;
        private bool m_IsChangingServer;
        public SocketState State;
        public bool IsChangingServer
        {
            get { return m_IsChangingServer; }
            set { m_IsChangingServer = value; }
        }
        private List<string> DisconnectReasons = new List<string>() {"Alerte au modo ! Alerte au modo !", "Try Reconnect.", "Wait before next meal.", "Changing server.", "User forced."};

        #region MITM
        private NetServer server;
        #endregion
        #endregion

        #region Constructors
        public SocketManager(AccountUC accountform)
        {
            account = accountform;
            if (account != null)
                account.LatencyFrame = new LatencyFrame(accountform);
            client = new NetClient();
            this.client.Connected += new EventHandler<NetSocketConnectedEventArgs>(client_Connected);
            this.client.DataArrived += new EventHandler<NetSockDataArrivalEventArgs>(client_DataArrived);
            this.client.Disconnected += new EventHandler<NetSocketDisconnectedEventArgs>(client_Disconnected);
            this.client.ErrorReceived += new EventHandler<NetSockErrorReceivedEventArgs>(client_ErrorReceived);
            this.client.StateChanged += new EventHandler<NetSockStateChangedEventArgs>(client_StateChanged);
        }

        /// <summary>
        /// Connect the socket.
        /// </summary>
        public void Connect(ConnectionInformations connectionInformations)
        {
            //if (m_RawSocket != null && !m_RawSocket.Connected)
            //    RawConnect();
            m_MessageInformations = new MessageInformations(account);
            if (m_IsChangingServer)
            {
                client.Close("Changing server.");
                client = new NetClient();
                this.client.Connected += new EventHandler<NetSocketConnectedEventArgs>(client_Connected);
                this.client.DataArrived += new EventHandler<NetSockDataArrivalEventArgs>(client_DataArrived);
                this.client.Disconnected += new EventHandler<NetSocketDisconnectedEventArgs>(client_Disconnected);
                this.client.ErrorReceived += new EventHandler<NetSockErrorReceivedEventArgs>(client_ErrorReceived);
                this.client.StateChanged += new EventHandler<NetSockStateChangedEventArgs>(client_StateChanged);
            }
            try
            {
                client.Connect(new IPEndPoint(IPAddress.Parse(connectionInformations.Address), connectionInformations.Port));
            }
            catch (SocketException sockEx)
            {
                account.Log(new ErrorTextInformation("[Socket Exception] " + sockEx.Message), 0);
                account.TryReconnect(10);
            }
        }

        /// <summary>
        /// Initialize the MITM server.
        /// </summary>
        public void InitMITM()
        {
            server = new NetServer();
            this.server.Connected += new EventHandler<NetSocketConnectedEventArgs>(server_Connected);
            this.server.ConnectionRequested += new EventHandler<NetSockConnectionRequestEventArgs>(server_ConnectionRequested);
            this.server.DataArrived += new EventHandler<NetSockDataArrivalEventArgs>(server_DataArrived);
            this.server.Disconnected += new EventHandler<NetSocketDisconnectedEventArgs>(server_Disconnected);
            this.server.ErrorReceived += new EventHandler<NetSockErrorReceivedEventArgs>(server_ErrorReceived);
            this.server.StateChanged += new EventHandler<NetSockStateChangedEventArgs>(server_StateChanged);
            ListenDofus();
        }

        /// <summary>
        /// Disconnect the socket and set the reason as "User forced".
        /// </summary>
        public void DisconnectFromGUI()
        {
            this.client.Close("User forced.");
        }

        /// <summary>
        /// Disconnect the socket and set the specified reason.
        /// </summary>
        public void Disconnect(string reason)
        {
            this.client.Close(reason);
        }


        /// <summary>
        /// Send the byte array.
        /// </summary>
        public void Send(byte[] content)
        {
            client.Send(content);
        }

        /// <summary>
        /// Serialize and pack the message, and send it.
        /// </summary>
        public void Send(BlueSheep.Engine.Types.Message msg)
        {
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                msg.Serialize(writer);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                account.SocketManager.Send(pack.Writer.Content);
                if (account.DebugMode.Checked)
                    account.Log(new DebugTextInformation("[SND] " + msg.ProtocolID), 0);
            }
        }


        #region Private Methods
        private void client_StateChanged(object sender, NetSockStateChangedEventArgs e)
        {
            switch (e.NewState)
            {
                case SocketState.Closed:
                    account.SetStatus(Enums.Status.Disconnected);
                    break;
                case SocketState.Connected:
                    account.SetStatus(Enums.Status.None);
                    break;
            }
            State = e.NewState;
        }

        private void client_ErrorReceived(object sender, NetSockErrorReceivedEventArgs e)
        {
            if (e.Exception.GetType() == typeof(System.Net.Sockets.SocketException))
            {
                System.Net.Sockets.SocketException s = (System.Net.Sockets.SocketException)e.Exception;
                account.Log(new ErrorTextInformation("Error: " + e.Function + " - " + s.SocketErrorCode.ToString() + "\r\n" + s.ToString()),0);
            }
            else
                account.Log(new ErrorTextInformation("Error: " + e.Function + "\r\n" + e.Exception.ToString()),4);
        }

        private void client_Disconnected(object sender, NetSocketDisconnectedEventArgs e)
        {
            account.Log(new ConnectionTextInformation("Disconnected: " + e.Reason), 2);
            if (!DisconnectReasons.Contains(e.Reason))
                account.TryReconnect(10);
        }

        private void client_DataArrived(object sender, NetSockDataArrivalEventArgs e)
        {
            m_MessageInformations.ParseBuffer(e.Data);
            account.LatencyFrame.UpdateLatency();
        }

        private void client_Connected(object sender, NetSocketConnectedEventArgs e)
        {
            account.Log(new ConnectionTextInformation("Connected: " + e.SourceIP), 0);
            Send(new BlueSheep.Common.Protocol.Messages.BasicPingMessage());
        }

        #region MITM

        #region Public Methods
        public void SendToDofusClient(byte[] p)
        {
            this.server.Send(p);
        }

        public void DisconnectServer(string reason)
        {
            this.server.Close(reason);
        }

        public void ListenDofus()
        {
            account.Log(new ConnectionTextInformation("Listening on port 5555"), 0);
            this.server.Listen(5555);
        }
        #endregion

        #region Private Methods
        private void server_StateChanged(object sender, NetSockStateChangedEventArgs e)
        {
            account.Log(new ConnectionTextInformation("New server state : " + e.NewState), 0);
        }

        private void server_ErrorReceived(object sender, NetSockErrorReceivedEventArgs e)
        {
            if (e.Exception.GetType() == typeof(System.Net.Sockets.SocketException))
            {
                System.Net.Sockets.SocketException s = (System.Net.Sockets.SocketException)e.Exception;
                account.Log(new ErrorTextInformation("Error: " + e.Function + " - " + s.SocketErrorCode.ToString() + "\r\n" + s.ToString()),0);
            }
            else
                account.Log(new ErrorTextInformation("Error: " + e.Function + "\r\n" + e.Exception.ToString()),0);
        }

        private void server_Disconnected(object sender, NetSocketDisconnectedEventArgs e)
        {
            account.Log(new ConnectionTextInformation("Disconnected: " + e.Reason),4);
        }

        private void local_Disconnected(object sender, NetSocketDisconnectedEventArgs e)
        {
            //this.server.Listen(5555);
        }

        private void server_DataArrived(object sender, NetSockDataArrivalEventArgs e)
        {
            client.Send(e.Data);
        }

        private void local_DataArrived(object sender, NetSockDataArrivalEventArgs e)
        {
            this.server.Send(e.Data);
        }

        private void server_ConnectionRequested(object sender, NetSockConnectionRequestEventArgs e)
        {
            account.Log(new ConnectionTextInformation("Connection Requested: " + ((System.Net.IPEndPoint)e.Client.RemoteEndPoint).Address.ToString()),4);
            this.server.Accept(e.Client);
            account.Init();
        }

        private void local_ConnectionRequested(object sender, NetSockConnectionRequestEventArgs e)
        {
            this.server.Accept(e.Client);
            account.Log(new ConnectionTextInformation("Accepted client !"), 0);
            account.Init();
        }

        private void server_Connected(object sender, NetSocketConnectedEventArgs e)
        {
            account.Log(new ConnectionTextInformation("Connected: " + e.SourceIP),4);
        }
        #endregion


        #endregion
        #endregion





    }
}
        #endregion
