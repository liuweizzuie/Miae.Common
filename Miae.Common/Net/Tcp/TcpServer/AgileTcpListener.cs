using Miae.Threading.Engine;
using System.Net;
using System.Net.Sockets;

namespace Miae.Net.Tcp
{
    /// <summary>
    /// 侦听器，负责管理连接维护。
    /// </summary>
    public class AgileTcpListener : BaseSingleThreadEngine, ITcpListener
    {
        private TcpListener listener = null;

        #region MaxConnectionCount
        /// <summary>
        /// 本侦听器最大支持的连接数目，默认是10000。
        /// </summary>
        public int MaxConnectionCount { get; set; } //10000
        #endregion

        #region ConnectionCount
        /// <summary>
        /// 当前正在保持连接的数目。
        /// </summary>
        public int ConnectionCount { get; set; }
        #endregion

        #region IsListening
        /// <summary>
        /// 获取当前是否处于侦听状态。
        /// </summary>
        public bool IsListening { get; protected set; }
        #endregion

        #region event ClientEstablishedEvent
        public event TcpClientEstablishedEventHandler ClientEstablishedEvent;
        protected void RaiseTcpClientEstablished(TcpClient client)
        {
            if (this.ClientEstablishedEvent != null)
            {
                ClientEstablishedEvent(client);
            }
        }
        #endregion

        #region ctor
        public AgileTcpListener() : this(8080) { }

        public AgileTcpListener(int port) : this(string.Empty, port) { }

        public AgileTcpListener(string ip, int port)
        {
            base.DetectSpanInMs = 10;
            base.SleepTimeInMs = 2;
            this.MaxConnectionCount = 10000;
            this.listener = string.IsNullOrEmpty(ip) ? new TcpListener(IPAddress.Any, port) : new TcpListener(IPAddress.Parse(ip), port);
        }
        #endregion

        #region Start , Stop
        public new void Start()
        {
            this.StartListener();
            base.Start();
        }

        protected void StartListener()
        {
            this.listener.Start();
            this.IsListening = true;
        }

        public new void Stop()
        {
            base.Stop();
            this.StopListener();
        }

        protected void StopListener()
        {
            this.listener.Stop();
            this.IsListening = false;
        }
        #endregion

        protected override bool EngineTask()
        {
            if (this.ConnectionCount <= this.MaxConnectionCount && !this.IsListening)
            {
                this.StartListener();
            }
            else if (this.ConnectionCount > this.MaxConnectionCount && this.IsListening)
            {
                this.StopListener();
            }

            //Socket sock = this.listener.AcceptSocket();
            // KeepAlive为20秒，检查间隔为2秒。如果拨掉客户端网线，服务器Socket.Receive()会在20秒后抛出异常。
            //int keepAlive = -1744830460; // SIO_KEEPALIVE_VALS
            //byte[] inValue = new byte[] { 1, 0, 0, 0, 0x20, 0x4e, 0, 0, 0xd0, 0x07, 0, 0 }; //True, 20 秒, 2 秒
            //sock.IOControl(keepAlive, inValue, null);
            //NetworkStream stream = new NetworkStream(sock);
            //this.RaiseTcpClientEstablished(stream, new AgileEndPoint(sock.RemoteEndPoint));

            if (this.listener.Pending())
            {
                TcpClient client = this.listener.AcceptTcpClient();

                if (System.Environment.OSVersion.ToString().Contains("Windows"))
                {
                    client.Client.IOControl(IOControlCode.KeepAliveValues, new byte[] { 1, 0, 0, 0, 0x20, 0x4e, 0, 0, 0xd0, 0x07, 0, 0 }, null);
                }

                this.RaiseTcpClientEstablished(client);
            }

            return true;
        }

        public void Restart(AgileEndPoint ep)
        {
            this.Stop();
            this.listener = new TcpListener(IPAddress.Parse(ep.IP), ep.Port);
            this.Start();
        }
    }
}
