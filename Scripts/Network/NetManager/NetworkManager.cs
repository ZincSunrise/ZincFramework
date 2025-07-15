using System.Net.Sockets;
using ZincFramework.Binary.Serialization;
using ZincFramework.Events;
using ZincFramework.Loop;
using ZincFramework.Network.Module;
using ZincFramework.Network.Protocol;


namespace ZincFramework.Network
{
        public sealed partial class NetworkManager : BaseSafeSingleton<NetworkManager>
        {
            public bool IsReconnecting { get; private set; }
            public bool IsConnected { get; private set; }

            private readonly INetworkModule _networkModule;
            private readonly ProtocolType _protocolType;

            #region Mono观察者
            private HandleSendObserver _handleSendObserver = new HandleSendObserver();

            private HandleMessageObserver _messageObserver = new HandleMessageObserver();
            #endregion

            private NetworkManager() 
            {
                SerializerOption = SerializerOption.Message;
                _writer = ByteWriterPool.GetCachedWriter(SerializerOption.Message, out _pooledBufferWriter);
                _heartMessage = new HeartMessage();
 
                (_sendOffsetTime, _protocolType) = FrameworkConsole.Instance.SharedData;

                _networkModule = INetworkModule.GetNetworkModule(_protocolType);
                ZincLoopSystem.AddUpdateListener(_handleSendObserver);
                ZincLoopSystem.AddUpdateListener(_messageObserver);

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
                        LogUtility.LogError("服务器拒绝连接");
                        EventCenter.Boardcast<int>(EventType.ServerReject, socketException.ErrorCode);
                    }
                    else
                    {
                        LogUtility.LogError("服务器连接失败");
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

                ZincLoopSystem.RemoveUpdateListener(_messageObserver);
                ZincLoopSystem.RemoveUpdateListener(_handleSendObserver);

                _networkModule.SendQuitMassage();
                _networkModule.Disconnect();
                if (!isSelf)
                {
                    EventCenter.Boardcast(EventType.IsReconnecting);
                    Reconnect();
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
                        LogUtility.LogError("服务器拒绝连接");
                        EventCenter.Boardcast<int>(EventType.ServerReject, socketException.ErrorCode);
                    }
                    else
                    {
                        LogUtility.LogError("服务器连接失败");
                        EventCenter.Boardcast<int>(EventType.ConnectFailed, socketException.ErrorCode);
                    }
                }
            }

            /// <summary>
            /// 测试函数，一般不建议使用
            /// </summary>
#if UNITY_EDITOR
            public void SendMassageTest(byte[] massages)
            {
                _sendQueue.Enqueue(massages);
            }
#endif
        }
}

