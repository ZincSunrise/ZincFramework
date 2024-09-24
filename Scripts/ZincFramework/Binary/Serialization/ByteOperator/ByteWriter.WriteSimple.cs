using System;
using System.Text;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


namespace ZincFramework.Binary.Serialization
{
    public partial class ByteWriter
    {
        public void WriteByte(byte value)
        {
            _bufferWriter.Write(stackalloc[] { value });
        }

        public void WriteSByte(sbyte value)
        {
            _bufferWriter.Write(stackalloc[] { (byte)value });
        }

        public void WriteBytes(Span<byte> value)
        {
            _bufferWriter.Write(value);
        }

        public void WriteInt16(short value)
        {
            Span<short> shorts = stackalloc short[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(shorts));
        }

        public void WriteUInt16(ushort value)
        {
            Span<ushort> shorts = stackalloc ushort[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(shorts));
        }

        public void WriteChar(char value)
        {
            if (WriterOption.Encoding == Encoding.Unicode)
            {
                Span<char> shorts = stackalloc char[1] { value };
                var bytes = MemoryMarshal.AsBytes(shorts);

                WriteByte((byte)bytes.Length);
                _bufferWriter.Write(bytes);
            }
            else
            {
                Span<byte> bytes = stackalloc byte[3];
                int length = WriterOption.Encoding.GetBytes(stackalloc[] { value }, bytes);
                WriteByte((byte)length);

                _bufferWriter.Write(bytes[..length]);
            }
        }

        public void WriteBoolean(bool value)
        {
            Span<bool> shorts = stackalloc bool[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(shorts));
        }

        public void WriteInt32(int value)
        {
            Span<int> shorts = stackalloc int[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(shorts));
        }

        public void WriteVarInt32(int value)
        {
            uint unsignedValue = (uint)value;
            while (unsignedValue >= 0x80)
            {
                WriteByte((byte)(unsignedValue | 0x80));
                unsignedValue >>= 7;
            }

            WriteByte((byte)unsignedValue);
        }

        public void WriteUInt32(uint value)
        {
            Span<uint> shorts = stackalloc uint[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(shorts));
        }

        public void WriteSingle(float value)
        {
            Span<float> shorts = stackalloc float[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(shorts));
        }

        public void WriteInt64(long value)
        {
            Span<long> shorts = stackalloc long[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(shorts));
        }

        public void WriteUInt64(ulong value)
        {
            Span<ulong> shorts = stackalloc ulong[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(shorts));
        }

        public void WriteDouble(double value)
        {
            Span<double> shorts = stackalloc double[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(shorts));
        }

        public void WriteString(string value)
        {
            if (WriterOption.Encoding == Encoding.Unicode)
            {
                var span = MemoryMarshal.AsBytes<char>(value);
                WriteInt32(span.Length);
                _bufferWriter.Write(span);
            }
            else
            {
                Span<byte> bytes = stackalloc byte[value.Length * 3];
                int length = WriterOption.Encoding.GetBytes(value, bytes);
                WriteInt32(length);

                _bufferWriter.Write(bytes[..length]);
            }
        }

        public void WriteDateTime(DateTime value)
        {
            WriteInt64(value.Ticks - TimeUtility.UTCFirstYear.Ticks);
        }

        public void WriteTimeSpan(TimeSpan value)
        {
            WriteInt64(value.Ticks);
        }
    }
}
