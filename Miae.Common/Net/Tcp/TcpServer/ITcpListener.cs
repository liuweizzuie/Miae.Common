using System;
using System.Collections.Generic;
using System.Text;

namespace Miae.Net.Tcp
{
    /// <summary>
    /// 侦听器，负责管理连接维护。
    /// </summary>
    public interface ITcpListener
    {
        /// <summary>
        /// 最大允许的连接数。
        /// </summary>
        int MaxConnectionCount { get; set; } //10000

        /// <summary>
        /// 当前正在保持连接的数目。
        /// </summary>
        int ConnectionCount { get; set; }

        /// <summary>
        /// 是否正在侦听。
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// 开始侦听。
        /// </summary>
        void Start();

        /// <summary>
        /// 停止侦听。
        /// </summary>
        void Stop();

        /// <summary>
        /// 使用新的端点重新侦听。
        /// </summary>
        /// <param name="ep"></param>
        void Restart(AgileEndPoint ep);

        /// <summary>
        /// 当有客户端建立连接时发生。
        /// </summary>
        event TcpClientEstablishedEventHandler ClientEstablishedEvent;

        /// <summary>
        /// 当TCP服务器意外停止时发生。
        /// </summary>
        event TcpListenerStopped ListenerStopped;
    }
}
