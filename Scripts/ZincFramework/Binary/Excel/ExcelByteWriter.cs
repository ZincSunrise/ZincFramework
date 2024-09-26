using System;
using System.Buffers;


namespace ZincFramework.Binary.Excel
{
    public struct ExcelByteWriter : IBufferWriter<byte>
    {
        public readonly Memory<byte> OutPut => _buffer.AsMemory(0, _postion);

        private readonly byte[] _buffer;

        private int _postion;

        public ExcelByteWriter(int defaultSize)
        {
            _buffer = ArrayPool<byte>.Shared.Rent(defaultSize);
            _postion = 0;
        }

        public void Advance(int count)
        {
            _postion += count;
        }

        public readonly Memory<byte> GetMemory(int sizeHint = 0) => _buffer.AsMemory(_postion);

        public readonly Span<byte> GetSpan(int sizeHint = 0) => _buffer.AsSpan(_postion);
    }
}