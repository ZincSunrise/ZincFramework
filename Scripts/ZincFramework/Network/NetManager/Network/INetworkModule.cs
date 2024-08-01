using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


namespace ZincFramework
{
    namespace Network
    {
        namespace Module
        {
            public interface INetworkModule
            {
                public static INetworkModule GetNetworkModule(ProtocolType protocolType) => protocolType switch
                {
                    ProtocolType.Tcp => new TcpNetworkModule(),
                    ProtocolType.Udp => new UdpNetWorkModule(),
                    _ => throw new InvalidOperationException("不合法的类型，只支持UDP和TCP"),
                };

                IPEndPoint IPEndPoint { get; }

                byte[] ReceiveBytes { get; }

                bool Connect(out SocketException socketException);

                bool Reconnect(out SocketException socketException);

                void Disconnect();

                Task<bool> SendAsync(byte[] bytes, int offset, int length);

                Task<int> ReceiveAsync();

                void SendQuitMassage();
            }
        }
    }
}

