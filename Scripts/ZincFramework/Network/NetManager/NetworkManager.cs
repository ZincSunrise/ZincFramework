using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading.Tasks;
using ZincFramework.Events;
using ZincFramework.Network.Protocol;
using ZincFramework.Network.BufferHandler;
using ZincFramework.Network.Module;
using ZincFramework.Binary.Serialization;


namespace ZincFramework
{
    namespace Network
    {
        public sealed class NetworkManager : BaseSafeSingleton<NetworkManager>
        {
            public bool IsReconnecting { get; private set; }
            public bool IsConnected { get; private set; }

            private ConcurrentQueue<Memory<byte>> _sendQueue = new ConcurrentQueue<Memory<byte>>();

            private int _sendOffsetTime = 10;

            private readonly HeartMessage _heartMessage;
            private NetBufferHandler _bufferHandler = new NetBufferHandler();

            private ConcurrentQueue<IHandleMessage> _handlerQueue = new ConcurrentQueue<IHandleMessage>();

            private readonly INetworkModule _networkModule;
            private readonly ProtocolType _protocolType;

            private readonly ZincEvent _onSend = new ZincEvent();

            private ByteWriter _writer;
            private PooledBufferWriter _pooledBufferWriter;
            private NetworkManager() 
            {
                _writer = ByteWriterPool.GetCachedWriter(SerializerOption.Message, out _pooledBufferWriter);
                _heartMessage = new HeartMessage();
 
                (_sendOffsetTime, _protocolType) = FrameworkConsole.Instance.SharedData;

                _networkModule = INetworkModule.GetNetworkModule(_protocolType);

                MonoManager.Instance.AddOnApplicationQuitListener(DisconnectOnQuit);

                MonoManager.Instance.AddUpdateListener(HandleSend);
                MonoManager.Instance.AddUpdateListener(HandleMessage);

                _bufferHandler.Receive.AddListener(HandleReceive);
            }


            public void Connect()
            {
                if (IsConnected = _networkModule.Connect(out SocketException socketException))
                {
                    SendHeartMassageAsync();
                    ReceiveAsync();
                }
                else
                {
                    if (socketException.ErrorCode == 10061) 
                    {
                        LogUtility.LogError("�������ܾ�����");
                        EventCenter.Boardcast<int>(EventType.ServerReject, socketException.ErrorCode);
                    }
                    else
                    {
                        LogUtility.LogError("����������ʧ��");
                        EventCenter.Boardcast<int>(EventType.ConnectFailed, socketException.ErrorCode);
                    }
                }
            }

            public void Disconnect(bool isSelf)
            {
                IsConnected = false;
                _onSend.RemoveAllListeners();

                _sendQueue.Clear();
                _handlerQueue.Clear();

                _bufferHandler.Dispose();
                MonoManager.Instance.RemoveUpdateListener(HandleMessage);
                MonoManager.Instance.RemoveUpdateListener(HandleSend);

                MonoManager.Instance.RemoveOnApplicationQuitListener(DisconnectOnQuit);

                _networkModule.SendQuitMassage();
                _networkModule.Disconnect();
                if (!isSelf)
                {
                    EventCenter.Boardcast(EventType.IsReconnecting);
                    Reconnect();
                }
            }

            public void SetReceiveBuffSize(int bufferSize)
            {
                _bufferHandler.SetBufferSize(bufferSize);
            }

            public void AddSendListener(ZincAction zincAction)
            {
                _onSend.AddListener(zincAction);
            }

            public void RemoveSendListener(ZincAction zincAction)
            {
                _onSend?.RemoveListener(zincAction);
            }


            private async void ReceiveAsync()
            {
                while (IsConnected)
                {
                    int receivelength = await _networkModule.ReceiveAsync();
                    if (receivelength == -1)
                    {
                        Disconnect(true);
                    }
                    else if(receivelength == -2)
                    {
                        LogUtility.LogError("���յ�һ�����������Է���������Ϣ");
                    }
                    else
                    {
                        _bufferHandler.HandleMassageAsync(_networkModule.ReceiveBytes, receivelength);
                    }
                }
            }

            private void Reconnect()
            {
                IsReconnecting = true;
                if (IsConnected = _networkModule.Reconnect(out SocketException socketException))
                {
                    SendHeartMassageAsync();
                    ReceiveAsync();
                    EventCenter.Boardcast(EventType.Reconnected);
                    IsReconnecting = false;
                }
                else
                {
                    if (socketException.ErrorCode == 10061)
                    {
                        LogUtility.LogError("�������ܾ�����");
                        EventCenter.Boardcast<int>(EventType.ServerReject, socketException.ErrorCode);
                    }
                    else
                    {
                        LogUtility.LogError("����������ʧ��");
                        EventCenter.Boardcast<int>(EventType.ConnectFailed, socketException.ErrorCode);
                    }
                }
            }

            private void DisconnectOnQuit()
            {
                Disconnect(true);
            }

            public void SendMassage(BaseMessage massage)
            {
                massage.Write(_writer);
                _sendQueue.Enqueue(_pooledBufferWriter.WrittenMemory);
            }

            private void HandleReceive(int id, byte[] bytes)
            {
                ByteReader byteReader = new ByteReader(bytes.AsSpan(), SerializerOption.Message.GetReaderOption());
                BaseMessage baseMessage = MessagePool.GetBaseMessage(id);
                baseMessage.Read(ref byteReader);

                IHandleMessage handle = MessagePool.GetBaseHandler(id, baseMessage);
                _handlerQueue.Enqueue(handle);
            }

            private async void SendHeartMassageAsync()
            {
                while (IsConnected)
                {
                    await Task.Delay(_sendOffsetTime << 10);
                    SendMassage(_heartMessage);
                }
            }

            private void HandleMessage()
            {
                while (_handlerQueue.Count > 0)
                {
                    if (_handlerQueue.TryDequeue(out IHandleMessage handler))
                    {
                        handler.HandleMessage();
                    }
                }
            }

            private async void HandleSend()
            {
                int nowIndex = 0;
                int totalLength = 0;
                foreach (var item in _sendQueue)
                {
                    totalLength += item.Length;
                }

                byte[] sendBuffer = new byte[totalLength];

                while (_sendQueue.Count > 0)
                {
                    if (_sendQueue.TryDequeue(out Memory<byte> item))
                    {
                        item.CopyTo(sendBuffer.AsMemory(nowIndex));
                        nowIndex += item.Length;
                    }
                    else
                    {
                        LogUtility.LogError("���Ӵ��������Ƿ����߳�����");
                    }
                }

                if (nowIndex > 0)
                {
                    if (!await _networkModule.SendAsync(sendBuffer, 0, nowIndex))
                    {
                        LogUtility.LogError("����ʧ�ܣ����ڶ�������������");
                        Disconnect(true);
                    }
                }
            }

            /// <summary>
            /// ���Ժ�����һ�㲻����ʹ��
            /// </summary>
#if UNITY_EDITOR
            public void SendMassageTest(byte[] massages)
            {
                _sendQueue.Enqueue(massages);
            }
#endif
        }
    }
}

