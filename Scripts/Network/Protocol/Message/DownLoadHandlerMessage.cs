using UnityEngine.Networking;
using ZincFramework.Events;
using ZincFramework.Network.BufferHandler;


namespace ZincFramework
{
    namespace Network
    {
        public class DownLoadHandlerMessage : DownloadHandlerScript
        {
            public event ZincAction completed;

            private byte[] _buffer;
            private int _nowIndex = 0;
            private NetBufferHandler _handler;

            public DownLoadHandlerMessage() : base()
            {
                _handler = new NetBufferHandler();
            }

            public DownLoadHandlerMessage(byte[] bytes) : base(bytes) 
            {
                _handler = new NetBufferHandler();
            }
            protected override void ReceiveContentLengthHeader(ulong contentLength)
            {
                _buffer = new byte[contentLength];
            }

            protected override void CompleteContent()
            {
                _nowIndex = 0;
                _handler.HandleMassageAsync(_buffer, _buffer.Length);

                completed?.Invoke();
            }


            protected override byte[] GetData()
            {
                return _buffer;
            }

            protected override bool ReceiveData(byte[] data, int dataLength)
            {
                data.CopyTo(_buffer, _nowIndex);
                _nowIndex += dataLength;

                return true;
            }
        }
    }
}

