using System;
using System.IO;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;



namespace ZincFramework.Binary.Serialization
{
    public struct BufferReader : IDisposable, IAsyncDisposable
    {
        private byte[] _buffer;

        private int _position;

        public readonly Memory<byte> OutputMemory => _buffer.AsMemory(0, _position);

        public BufferReader(int minSize)
        {
            _buffer = ArrayPool<byte>.Shared.Rent(minSize);
            _position = 0;
        }

        public void ReadFromStream(Stream stream)
        {
            CheckAndResize(stream.Length);
            _position = (int)stream.Length; 
            stream.Read(_buffer, 0, _position);
        }

        public async ValueTask ReadFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            CheckAndResize(stream.Length);
            _position = (int)stream.Length;
            await stream.ReadAsync(_buffer, 0, _position, cancellationToken);
        }

        public void Dispose()
        {
            ArrayPool<byte>.Shared.Return(_buffer);
        }

        public ValueTask DisposeAsync()
        {
            ArrayPool<byte>.Shared.Return(_buffer);
            return new ValueTask();
        }

        private void CheckAndResize(long streamLength)
        {
            if (_buffer.Length < streamLength)
            {
                int targetLength = _buffer.Length;
                do
                {
                    targetLength *= 2;
                } while (targetLength < streamLength);

                ArrayPool<byte>.Shared.Return(_buffer);
                _buffer = ArrayPool<byte>.Shared.Rent(targetLength);
            }
        }
    }
}