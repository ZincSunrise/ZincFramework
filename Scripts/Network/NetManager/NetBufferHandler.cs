using System;
using ZincFramework.Binary;
using ZincFramework.Network.Events;


namespace ZincFramework
{
    namespace Network
    {
        namespace BufferHandler
        {
            public class NetBufferHandler : IDisposable
            {
                private byte[] _cacheBytes;
                private int _bufferSize = 1024 * 1024;
                private int _cacheNumber = 0;
                private bool _willResize = false;

                public OnReceiveEvent Receive { get; private set; }

                public NetBufferHandler()
                {
                    _cacheBytes = new byte[_bufferSize];
                    Receive = new OnReceiveEvent();
                }

                public void SetBufferSize(int size)
                {
                    _willResize = true;
                    _bufferSize = size;
                }

                public void HandleMassageAsync(byte[] receiveBytes, int receiveNum)
                {
                    while (_cacheBytes.Length != _bufferSize || _willResize)
                    {
                        Array.Resize(ref _cacheBytes, _bufferSize);
                        _willResize = false;
                    }

                    if (_cacheBytes.Length < receiveNum)
                    {
                        Array.Resize(ref _cacheBytes, receiveNum);
                        _bufferSize = receiveNum;
                    }

                    int nowIndex = 0;
                    int serilizeCode;
                    int massageLength;

                    //分包逻辑
                    Buffer.BlockCopy(receiveBytes, 0, _cacheBytes, _cacheNumber, receiveNum);
                    _cacheNumber += receiveNum;

                    while (true)
                    {
                        //处理长度和编号
                        if (_cacheNumber - nowIndex < 8)
                        {
                            break;
                        }

                        serilizeCode = ByteConverter.ToInt32(_cacheBytes, ref nowIndex);
                        massageLength = ByteConverter.ToInt32(_cacheBytes, ref nowIndex);

                        //处理类的反序列化逻辑
                        if (_cacheNumber - nowIndex >= massageLength)
                        {
                            Receive.Invoke(serilizeCode, _cacheBytes);

                            nowIndex += massageLength;
                            if (nowIndex == _cacheNumber)
                            {
                                _cacheNumber = 0;
                                Array.Clear(_cacheBytes, 0, _cacheNumber);
                                break;
                            }
                        }
                        else
                        {
                            nowIndex -= 8;
                            Buffer.BlockCopy(_cacheBytes, nowIndex, _cacheBytes, 0, _cacheNumber - nowIndex);
                            _cacheNumber -= nowIndex;
                            break;
                        }
                    }
                }

                public void Dispose()
                {
                    Array.Clear(_cacheBytes, 0, _cacheBytes.Length);
                }
            }
        }
    }
}

