using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ZincFramework.Binary.Serialization;
using ZincFramework.Network.Protocol;



namespace ZincFramework
{
    namespace Network
    {
        namespace Module
        {
            public class TcpNetworkModule : INetworkModule
            {
                public IPEndPoint IPEndPoint => _serverEndPoint;

                public byte[] ReceiveBytes => _receiveBytes;


                private Socket _serverSocket;


                private int _bufferSize = 1024 * 1024;


                private byte[] _receiveBytes;


                private ByteWriter _byteWriter;

                private PooledBufferWriter _pooledBufferWriter;


                private readonly IPEndPoint _serverEndPoint;

                private SerializerOption _serializerOption;

                public TcpNetworkModule(SerializerOption serializerOption)
                {
                    _receiveBytes = new byte[_bufferSize];
                    string address = FrameworkConsole.Instance.SharedData.remoteAddress;
                    short port = FrameworkConsole.Instance.SharedData.remotePort;

                    _byteWriter = ByteWriterPool.GetCachedWriter(SerializerOption.Message, out _pooledBufferWriter);
                    _serverEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
                    _serializerOption = serializerOption;
                }

                public bool Connect(out SocketException socketException)
                {
                    if (_serverSocket != null && _serverSocket.Connected)
                    {
                        socketException = null;
                        return true;
                    }

                    _serverSocket ??= SocketUtility.GetTcpSocket();
                    try
                    {
                        _serverSocket.Connect(_serverEndPoint);
                        socketException = null;
                        return true;
                    }
                    catch (SocketException exception)
                    {
                        socketException = exception;
                        return false;
                    }
                }

                public bool Reconnect(out SocketException socketException)
                {
                    _serverSocket ??= SocketUtility.GetTcpSocket();
                    try
                    {
                        _serverSocket.Connect(_serverEndPoint);
                        socketException = null;
                        return true;
                    }
                    catch (SocketException exception)
                    {
                        socketException = exception;
                        return false;
                    }
                }

                /// <summary>
                /// Ö±½Ó¶ÏÏß
                /// </summary>
                public void Disconnect()
                {
                    _serverSocket.Shutdown(SocketShutdown.Both);
                    _serverSocket.Close();
                    Array.Clear(_receiveBytes, 0, _receiveBytes.Length);
                    _serverSocket.Disconnect(false);
                    _serverSocket = null;
                }


                public async Task<bool> SendAsync(byte[] bytes, int offset, int length)
                {
                    try
                    {
                        await _serverSocket.SendAsync(bytes, SocketFlags.None);
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
                        return await _serverSocket.ReceiveAsync(_receiveBytes, SocketFlags.None);
                    }
                    catch (SocketException exception)
                    {
                        LogUtility.LogError(exception.ErrorCode);
                        LogUtility.LogError(exception.Message);
                        return -1;
                    }
                }

                public void SendQuitMassage()
                {
                    new QuitMessage().Write(_byteWriter, _serializerOption);
                    _serverSocket.Send(_pooledBufferWriter.WrittenMemory.Span);

                    _pooledBufferWriter.Clear();
                }
            }
        }
    }
}

