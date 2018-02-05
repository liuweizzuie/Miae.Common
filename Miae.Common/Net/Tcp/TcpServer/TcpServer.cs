using Miae.Threading.Engine;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Miae.Net.Tcp
{
    /// <summary>
    /// Tcp服务器，负责客户端连接管理与数据管理。
    /// </summary>
    public abstract class TcpServer : BaseSingleThreadEngine
    {
        ITcpListener listener = new AgileTcpListener();
        ITcpClientAgentCollection clients = new TcpClientAgentCollection();

        #region Client Events
        #region event ClientConnectedEvent
        public event TcpClientConnectedEventHandler ClientConnectedEvent;
        protected void RaiseClientConnectedEvent(ITcpClientAgent agent)
        {
            if (this.ClientConnectedEvent != null)
            {
                this.ClientConnectedEvent(this, new TcpClientConnectedEventArgs(agent));
            }
        }
        #endregion

        #region event ClientDisconnectedEvent
        public event TcpClientDisconnectedEventHandler ClientDisconnectedEvent;
        protected void RaiseClientDisconnectedEvent(ITcpClientAgent agent)
        {
            if (this.ClientDisconnectedEvent != null)
            {
                this.ClientDisconnectedEvent(this, new TcpClientDisconnectedArgs(agent));
            }
        }
        #endregion
        #endregion

        public TcpServer() : base(20, 2)
        {
            this.listener.ClientEstablishedEvent += listener_ClientEstablishedEvent;
        }

        public new void Start() { this.listener.Start(); base.Start(); }
        public new void Stop() { this.listener.Stop(); base.Start(); }
        public void ResetListener(string ip, int port) { this.listener.Restart(new AgileEndPoint(ip, port)); }

        protected override bool EngineTask()
        {
            try
            {
                for (int i = this.clients.Count - 1; i >= 0; i--)
                {
                    ITcpClientAgent client = this.clients[i];
                    List<byte> data = new List<byte>();

                    if (!client.CanRead || !client.CanWrite || !client.Connected)
                    {
                        this.clients.Remove(client);
                        client.NotifyDisconnected();
                        client.Close();
                        this.RaiseClientDisconnectedEvent(client);
                        continue;
                    }

                    if (client.DataAvailable)
                    {
                        client.Read();
                    }
                    client.Schedule();

                    try
                    {
                        client.WriteEmptyByte(); //尝试写入一个空字节，以测试连接的可用性。
                    }
                    catch (Exception)
                    {
                        RaiseClientDisconnectedEvent(client);
                        client.NotifyDisconnected();
                        client.Dispose();
                        this.clients.Remove(client);
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.TxtLogger.SingleInstance.LogWithTime(exp.Message);
            }
            return true;
        }

        /// <summary>
        /// 创建自定义的客户端代理。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="remoteEP"></param>
        /// <returns></returns>
        protected abstract ITcpClientAgent CreateClientAgent(TcpClient client);

        private void listener_ClientEstablishedEvent(TcpClient client)
        {
            ITcpClientAgent agent = CreateClientAgent(client);
            this.clients.Add(agent);
            this.RaiseClientConnectedEvent(agent);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.listener.Stop();
            this.clients.Clear();
        }
    }
}
