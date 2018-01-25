using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Miae.Net
{
    public sealed class SafeNetworkStream : ISafeNetworkStream
    {
        private object lockForRead = new object();
        private object lockForWrite = new object();
        private NetworkStream stream = null;

        public SafeNetworkStream(NetworkStream netStream)
        {
            this.stream = netStream;
        }

        #region ISafeNetworkStream 成员
        public NetworkStream NetworkStream { get { return this.stream; } }

        #region Write ,Read
        public int Read(byte[] buffer, int offset, int size)
        {
            lock (this.lockForRead)
            {
                return this.stream.Read(buffer, offset, size);
            }
        }

        public int ReadByte()
        {
            lock (this.lockForRead)
            {
                return this.stream.ReadByte();
            }
        }

        public void Write(byte[] buffer, int offset, int size)
        {
            lock (this.lockForWrite)
            {
                this.stream.Write(buffer, offset, size);
            }
        }

        public void BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            this.stream.BeginWrite(buffer, offset, size, callback, state);
        }
        #endregion

        public void Flush() { this.stream.Flush(); }
        public void Close() { this.stream.Close(); }
        public bool CanRead { get { return this.stream.CanRead; } }
        public bool CanWrite { get { return this.stream.CanWrite; } }
        public bool DataAvailable { get { return this.stream.DataAvailable; } }
        public override int GetHashCode() { return this.stream.GetHashCode(); }


        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            this.stream.Dispose();
        }

        #endregion
    }
}
