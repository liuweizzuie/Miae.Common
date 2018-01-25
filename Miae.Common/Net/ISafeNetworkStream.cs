using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Miae.Net
{
    /// <summary>
    /// INetworkStreamSafe 线程安全的网络流 。保证任一时刻最多只有一个读/写动作发生。
    /// <param>作者：朱伟 sky.zhuwei@163.com</param>
    /// <para>2005.04.22</para>
    /// 点评：防止多线程重入，造成写入的数据交叉混合。
    /// </summary>	
    public interface ISafeNetworkStream : IDisposable
    {
        void Flush();
        void Close();

        void Write(byte[] buffer, int offset, int size);
        void BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state);
        int Read(byte[] buffer, int offset, int size);
        int ReadByte();
        bool DataAvailable { get; }
        NetworkStream NetworkStream { get; }

        bool CanRead { get; }
        bool CanWrite { get; }
    }
}
