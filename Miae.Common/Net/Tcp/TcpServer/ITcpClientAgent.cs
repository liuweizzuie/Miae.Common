using System;

namespace Miae.Net.Tcp
{
    /// <summary>
    /// 表示一个远程客户连接，并维护此连接。
    /// <para>用于服务器端。</para>
    /// </summary>
    public interface ITcpClientAgent : IDisposable
    {
        /// <summary>
        /// 记录接收到的字节数。
        /// </summary>
        long BytesReceived { get; }

        /// <summary>
        /// 记录发送的字节数。
        /// </summary>
        long BytesSent { get; }

        /// <summary>
        /// 远端IP端口
        /// </summary>
        AgileEndPoint EndPoint { get; }

        /// <summary>
        /// 是否可读。
        /// </summary>
        bool CanRead { get; }

        /// <summary>
        /// 是否可写。
        /// </summary>
        bool CanWrite { get; }

        /// <summary>
        /// 当前是否有数据。
        /// </summary>
        bool DataAvailable { get; }

        /// <summary>
        /// 处理数据。
        /// </summary>
        void Read();

        /// <summary>
        /// 向远程客户端写一个空字节数组，以测试连接可用性。
        /// </summary>
        void WriteEmptyByte();

        /// <summary>
        /// 触发下线事件。
        /// </summary>
        void NotifyDisconnected();

        /// <summary>
        /// 获取一个值，该值指示是否已连接到远程主机。
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// 释放此<see cref="ITcpClientAgent"/>实例，并请求关闭基础 TCP 连接。
        /// </summary>
        void Close();
    }
}
