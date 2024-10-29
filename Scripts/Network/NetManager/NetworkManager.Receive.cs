using System;
using System.Collections.Concurrent;
using UnityEngine;
using ZincFramework.Binary.Serialization;
using ZincFramework.Network.Protocol;
using ZincFramework.Network.BufferHandler;



namespace ZincFramework.Network
{
    public sealed partial class NetworkManager
    {
        private NetBufferHandler _bufferHandler = new NetBufferHandler();

        private ConcurrentQueue<IHandleMessage> _handlerQueue = new ConcurrentQueue<IHandleMessage>();

        public void SetReceiveBuffSize(int bufferSize)
        {
            _bufferHandler.SetBufferSize(bufferSize);
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
                else if (receivelength == -2)
                {
                    LogUtility.LogError("接收到一个并不是来自服务器的信息");
                }
                else
                {
                    _bufferHandler.HandleMassageAsync(_networkModule.ReceiveBytes, receivelength);
                }
            }
        }

        private void HandleReceive(int id, byte[] bytes)
        {
            ByteReader byteReader = new ByteReader(bytes.AsSpan(), SerializerOption.Message.GetReaderOption());
            BaseMessage baseMessage = MessagePool.GetBaseMessage(id);
            baseMessage.Read(ref byteReader, SerializerOption);

            IHandleMessage handle = MessagePool.GetBaseHandler(id, baseMessage);
            _handlerQueue.Enqueue(handle);
        }
    }
}

