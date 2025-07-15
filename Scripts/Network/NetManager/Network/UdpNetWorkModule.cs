using System.Net.Sockets;
using System.Net;
using ZincFramework.Network.Protocol;
using System.Threading.Tasks;
using ZincFramework.Binary.Serialization;
using System;
using System.Runtime.InteropServices;


namespace ZincFramework.Network.Module
{
    public class UdpNetWorkModule : INetworkModule
    {
        public IPEndPoint IPEndPoint => _serverEndpoint;

        private IPEndPoint _serverEndpoint;

        public byte[] ReceiveBytes => _receiveBytes;


        private byte[] _receiveBytes;


        private int _buffeSize = 1024 * 1024;


        private Socket _localSocket;

        private ByteWriter _localWriter;
        private PooledBufferWriter _poolBufferWriter;
        private SerializerOption _serializerOption;

        public UdpNetWorkModule(SerializerOption serializerOption)
        {
            _serializerOption = serializerOption;
            _localWriter = ByteWriterPool.GetCachedWriter(SerializerOption.Message, out _poolBufferWriter);
            _receiveBytes = new byte[_buffeSize];
            string address = FrameworkConsole.Instance.SharedData.remoteAddress;
            short port = FrameworkConsole.Instance.SharedData.remotePort;

            _serverEndpoint = new IPEndPoint(IPAddress.Parse(address), port);
        }


        public bool Connect(out SocketException socketException)
        {
            _localSocket ??= SocketUtility.GetUdpSocket();
            string address = FrameworkConsole.Instance.SharedData.localAddress;
            short port = FrameworkConsole.Instance.SharedData.localPort;
            _localSocket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            socketException = null;
            return true;
        }

        public bool Reconnect(out SocketException socketException)
        {
            return Connect(out socketException);
        }

        public void Disconnect()
        {
            _localSocket.Disconnect(false);
            _localSocket.Dispose();
            _localSocket = null;
        }

        public async Task<bool> SendAsync(byte[] bytes, int offset, int length)
        {
            try
            {
                await _localSocket.SendToAsync(bytes, SocketFlags.None, _serverEndpoint);
                return true;
            }
            catch (SocketException exception)
            {
                LogUtility.LogError(exception.ErrorCode);
                LogUtility.LogError(exception.Message);
                return false;
            }
        }

        public async Task<int> ReceiveAsync()
        {
            try
            {
                SocketReceiveFromResult result = await _localSocket.ReceiveFromAsync(_receiveBytes, SocketFlags.None, _serverEndpoint);
                if (!_serverEndpoint.Equals(result.RemoteEndPoint))
                {
                    LogUtility.Log("接收到一个非服务器的消息");
                    return -2;
                }
                return result.ReceivedBytes;
            }
            catch (SocketException ex)
            {
                LogUtility.LogError("错误为" + ex.ErrorCode);
                LogUtility.LogError("错误为" + ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 直接断线
        /// </summary>
        public void SendQuitMassage()
        {
            new QuitMessage().Write(_localWriter, _serializerOption);
            if (MemoryMarshal.TryGetArray<byte>(_poolBufferWriter.WrittenMemory, out var segment))
            {
                _localSocket.SendTo(segment.Array, 0, 8, SocketFlags.None, _serverEndpoint);
            }

            _poolBufferWriter.Clear();
        }
    }
}

