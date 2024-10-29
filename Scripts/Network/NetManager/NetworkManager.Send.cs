using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using ZincFramework.Events;
using ZincFramework.Network.Protocol;
using ZincFramework.Binary.Serialization;



namespace ZincFramework.Network
{
    public sealed partial class NetworkManager
    {
        private int _sendOffsetTime = 10;

        private readonly HeartMessage _heartMessage;

        private readonly ConcurrentQueue<Memory<byte>> _sendQueue = new ConcurrentQueue<Memory<byte>>();

        private readonly ByteWriter _writer;

        private readonly PooledBufferWriter _pooledBufferWriter;

        public SerializerOption SerializerOption { get; private set; }


        private readonly ZincEvent _onSend = new ZincEvent();

        public void ChangeSendOffset(int sendOffsetTime) => _sendOffsetTime = sendOffsetTime;

        public void AddSendListener(ZincAction zincAction)
        {
            _onSend.AddListener(zincAction);
        }

        public void RemoveSendListener(ZincAction zincAction)
        {
            _onSend?.RemoveListener(zincAction);
        }

        public void SendMassage(BaseMessage massage)
        {
            massage.Write(_writer, SerializerOption);
            _sendQueue.Enqueue(_pooledBufferWriter.WrittenMemory);
        }

        private async void SendHeartMassageAsync()
        {
            while (IsConnected)
            {
                await Task.Delay(_sendOffsetTime << 10);
                SendMassage(_heartMessage);
            }
        }
    }
}
