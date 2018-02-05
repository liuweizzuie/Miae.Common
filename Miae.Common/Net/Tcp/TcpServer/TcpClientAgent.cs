using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Miae.Net.Tcp
{
    /// <summary>
    /// 表示一个远程客户连接，并维护此连接。
    /// <para>用于服务器端。</para>
    /// <para>请重写HandleCache()方法，在其中添加处理缓存数据的逻辑。</para>
    /// </summary>
    public class TcpClientAgent : ITcpClientAgent
    {
        private TcpClient client;

        #region protected IList<byte> Cache
        /// <summary>
        /// 数据缓存。
        /// </summary>
        protected IList<byte> Cache = new List<byte>();
        #endregion 

        #region BytesReceived
        /// <summary>
        /// 记录接收到的字节数。
        /// </summary>
        public long BytesReceived { get; protected set; }
        #endregion

        /// <summary>
        /// 是否可读。
        /// </summary>
        public bool CanRead { get { return this.Stream.CanRead; } }

        /// <summary>
        /// 是否可写。
        /// </summary>
        public bool CanWrite { get { return this.Stream.CanWrite; } }

        public bool DataAvailable { get { return this.Stream.DataAvailable && this.client.Available > 0; } }

        #region public long BytesSend
        /// <summary>
        /// 记录发送的字节数。
        /// </summary>
        public long BytesSent { get; protected set; }
        #endregion

        #region  public ISafeNetworkStream Stream
        /// <summary>
        /// 此客户端的网络数据流。
        /// </summary>
        protected ISafeNetworkStream Stream { get; set; }
        #endregion

        #region  public AgileEndPoint EndPoint
        /// <summary>
        /// 远端IP端口
        /// </summary>
        public AgileEndPoint EndPoint { get; protected set; }
        #endregion

        /// <summary>
        /// 获取一个值，该值指示是否已连接到远程主机。
        /// </summary>
        public bool Connected { get { return this.client.Connected; } }

        #region public TcpClientAgent(ISafeNetworkStream stream, AgileEndPoint endPoint)


        /// <summary>
        /// 使用一个远端客户端构造 TcpClientAgent 的新实例。
        /// </summary>
        /// <param name="client">已连接的远端客户端。</param>
        public TcpClientAgent(TcpClient client)
        {
            this.client = client;
            // 注意，这里假定 client 是已经连接的，但是在单元测试中，client可能是未连接的。
            if (client.Connected)
            {
                this.Stream = new SafeNetworkStream(client.GetStream());
                this.EndPoint = new AgileEndPoint(client.Client.RemoteEndPoint);
            }
        }
        #endregion

        #region public void Read()
        public void Read()
        {
            while (this.Stream.DataAvailable)
            {
                this.Cache.Add((byte)this.Stream.ReadByte());
                this.BytesReceived++;
            }
            HandleCache();
        }
        #endregion

        public void NotifyDisconnected() { OnDisconnected(); }

        #region protected virtual void HandleCache() { }
        /// <summary>
        /// 根据具体的业务要求，处理并尽量清空所有的缓存。
        /// <para>如果缓存中的数据没有攒够一次处理的量，则可以留着下次处理。</para>
        /// </summary>
        protected virtual void HandleCache() { }
        #endregion

        protected virtual void OnDisconnected() { }

        public void WriteEmptyByte()
        {
            this.Stream.Write(new byte[] { }, 0, 0);
            this.Stream.Flush();
        }

        /// <summary>
        /// 供引擎周期性调用。
        /// </summary>
        public void Schedule()
        {
            OnSchedule();
        }

        /// <summary>
        /// 由引擎周期性调用，可以在这里执行一些主动任务。
        /// </summary>
        protected virtual void OnSchedule()
        {

        }

        public void Close()
        {
            this.client.Close();
        }

        #region public void Dispose()
        public void Dispose()
        {
            if (this.Connected)
            {
                this.Stream.Close();
                this.Stream.Dispose();
                this.client.Close();
            }
        }
        #endregion
    }
}
