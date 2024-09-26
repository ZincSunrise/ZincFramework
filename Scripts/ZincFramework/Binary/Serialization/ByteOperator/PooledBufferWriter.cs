using System;
using System.IO;
using System.Buffers;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using ZincFramework.DataPool;



namespace ZincFramework.Binary.Serialization
{
    public class PooledBufferWriter : PipeWriter, IDisposable, IResumable
    {
        public Memory<byte> WrittenMemory => _buffer.AsMemory(0, _position);

        public int FreeCapacity => _buffer.Length - _position;

        private readonly Stream _stream;

        private byte[] _buffer;

        private int _position;

        private const int _minCapacity = 256;

        private const int _maxCapacity = int.MaxValue;

        public PooledBufferWriter(int sizeHint)
        {
            _buffer = ArrayPool<byte>.Shared.Rent(sizeHint);
            _position = 0;
            _stream = null;
        }

        public PooledBufferWriter(int sizeHint, Stream stream)
        {
            _buffer = ArrayPool<byte>.Shared.Rent(sizeHint);
            _position = 0;
            _stream = stream;
        }

        public void OnReturn()
        {
            Clear();
            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = null;
        }

        public void OnRent()
        {

        }

        public override void Advance(int count)
        {
            Debug.Assert(count > 0);
            Debug.Assert(count < _maxCapacity);
            _position += count;
        }

        public void Clear()
        {
            Debug.Assert(_buffer != null);
            _position = 0;
            _buffer.AsSpan(0, _position).Clear();
        }

        public void Dispose()
        {
            Clear();
            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = null;
        }

        public override Memory<byte> GetMemory(int sizeHint = _minCapacity)
        {
            CheckAndResizeBuffer(sizeHint);
            return _buffer.AsMemory(_position);
        }

        public override Span<byte> GetSpan(int sizeHint = _minCapacity)
        {
            CheckAndResizeBuffer(sizeHint);
            return _buffer.AsSpan(_position);
        }

        public override void Complete(Exception exception = null) => throw new NotImplementedException();

        public override void CancelPendingFlush() => throw new NotImplementedException();

        public void WriteInStream(Stream stream)
        {
            stream.Write(WrittenMemory.Span);
        }

        public async ValueTask WriteInStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            await stream.WriteAsync(WrittenMemory, cancellationToken);
        }

        public override async ValueTask<FlushResult> FlushAsync(CancellationToken cancellationToken = default)
        {
            await _stream.WriteAsync(WrittenMemory, cancellationToken);
            return new FlushResult(isCanceled: false, isCompleted: false);
        }

        private void CheckAndResizeBuffer(int sizeHint)
        {
            Debug.Assert(_buffer != null);
            Debug.Assert(sizeHint > 0);

            int currentLength = _buffer.Length;
            int availableSpace = currentLength - _position;

            if (_position >= _maxCapacity / 2)
            {
                sizeHint = (int)MathF.Max(sizeHint, _maxCapacity - currentLength);
            }

            if (sizeHint > availableSpace)
            {
                int growBy = (int)MathF.Max(sizeHint, currentLength);

                int newSize = currentLength + growBy;

                if ((uint)newSize > _maxCapacity)
                {
                    newSize = currentLength + sizeHint;
                    if ((uint)newSize > _maxCapacity)
                    {
                        throw new OutOfMemoryException($"借用数组长度过大{(uint)newSize}");
                    }
                }

                byte[] oldBuffer = _buffer;

                _buffer = ArrayPool<byte>.Shared.Rent(newSize);

                Span<byte> oldBufferAsSpan = oldBuffer.AsSpan(0, _position);
                oldBufferAsSpan.CopyTo(_buffer);
                oldBufferAsSpan.Clear();
                ArrayPool<byte>.Shared.Return(oldBuffer);
            }
        }
    }
}