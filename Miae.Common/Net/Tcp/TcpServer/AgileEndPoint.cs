using System.Net;

namespace Miae.Net.Tcp
{
    public sealed class AgileEndPoint
    {
        public string IP { get; set; }
        public int Port { get; set; }

        public AgileEndPoint(string ip, int port) { this.IP = ip; this.Port = port; }
        public AgileEndPoint(EndPoint endPoint) : this(endPoint.ToString()) { }
        public AgileEndPoint(string endPoint) : this(endPoint.Split(':')[0], int.Parse(endPoint.Split(':')[1])) { }
        public AgileEndPoint() : this("127.0.0.1", 8080) { }

        public IPEndPoint IPEndPint { get { return new IPEndPoint(IPAddress.Parse(this.IP), this.Port); } }

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.IP, this.Port);
        }
    }
}
