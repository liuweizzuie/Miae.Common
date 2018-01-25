using System.Collections.ObjectModel;

namespace Miae.Net.Tcp
{
    /// <summary>
    /// 表示服务器的所有活动连接。
    /// </summary>
    public class TcpClientAgentCollection : Collection<ITcpClientAgent>, ITcpClientAgentCollection
    {

    }
}
