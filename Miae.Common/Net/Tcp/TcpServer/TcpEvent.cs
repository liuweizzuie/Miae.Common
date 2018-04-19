using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Miae.Net.Tcp
{
    /// <summary>
    /// 表示将处理Tcp客户端连接建立的方法。
    /// <para>为<see cref="AgileTcpListener"/>提供。</para>
    /// </summary>
    /// <param name="stream">建立的网络数据流。</param>
    /// <param name="romoteEP">远端终点。</param>
    public delegate void TcpClientEstablishedEventHandler(TcpClient client);

    /// <summary>
    /// 表示将处理客户端上线事件的方法。
    /// <para>用于<see cref="TcpServer"/>。</para>
    /// </summary>
    /// <param name="sender">触发此事件的对象。 </param>
    /// <param name="e">此事件所带的参数，包含一个<see cref="IClientAgent"/>对象。 </param>
    public delegate void TcpClientConnectedEventHandler(object sender, TcpClientConnectedEventArgs e);

    /// <summary>
    /// 表示将处理客户端掉线事件的方法。
    /// </summary>
    /// <param name="sender">触发此事件的对象。</param>
    /// <param name="e">此事件所带的参数，包含一个<see cref="ClientAgent"对象/>。</param>
    public delegate void TcpClientDisconnectedEventHandler(object sender, TcpClientDisconnectedArgs e);

    /// <summary>
    /// 当TCPListener意外停止时发生。
    /// </summary>
    /// <param name="sender">触发此事件的对象，由  <see cref="AgileTcpListener"/> 触发</param>
    /// <param name="e">包含了异常信息的参数。</param>
    public delegate void TcpListenerStopped(object sender, ExceptionEventArgs e);

    /// <summary>
    /// 表示一次客户端上线或掉线事件。
    /// </summary>
    public class RemoteClientEventArgs : EventArgs
    {
        /// <summary>
        /// 此事件的客户端代理。
        /// </summary>
        public ITcpClientAgent ClientAgent
        {
            get;
            protected set;
        }

        /// <summary>
        /// 从<see cref="ClientAgent"/>获取RemoteClientEventArgs的新实例。
        /// </summary>
        /// <param name="clientAgent">此事件的客户端代理。</param>
        public RemoteClientEventArgs(ITcpClientAgent clientAgent)
        {
            this.ClientAgent = clientAgent;
        }
    }

    /// <summary>
    /// 表示一次客户端上线事件。
    /// </summary>
    public class TcpClientConnectedEventArgs : RemoteClientEventArgs
    {
        /// <summary>
        /// 从<see cref="TcpClientAgent"/>获取TcpClientConnectedEventArgs的新实例。
        /// </summary>
        /// <param name="clientAgent">此事件的客户端代理。</param>
        public TcpClientConnectedEventArgs(ITcpClientAgent clientAgent) : base(clientAgent) { }
    }

    /// <summary>
    /// 表示一次客户端掉线事件。
    /// </summary>
    public class TcpClientDisconnectedArgs : RemoteClientEventArgs
    {
        /// <summary>
        /// 从<see cref="TcpClientAgent"/>获取TcpClientConnectedEventArgs的新实例。
        /// </summary>
        /// <param name="clientAgent">此事件的客户端代理。</param>
        public TcpClientDisconnectedArgs(ITcpClientAgent clientAgent) : base(clientAgent) { }
    }
}
