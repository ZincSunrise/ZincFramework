using System;
using System.Text;
using System.Buffers;
using System.Runtime.InteropServices;


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
            if (WriterOption.IsUsingVariant)
            {
                WriteVarInt16((ushort)value);
            }
            else
            {
                Span<short> span = stackalloc short[1] { value };
                _bufferWriter.Write(MemoryMarshal.AsBytes(span));
            }
        }

        public void WriteUInt16(ushort value)
        {
            if (WriterOption.IsUsingVariant)
            {
                WriteVarInt16(value);
            }
            else
            {
                Span<ushort> span = stackalloc ushort[1] { value };
                _bufferWriter.Write(MemoryMarshal.AsBytes(span));
            }
        }

        public void WriteVarInt16(ushort value)
        {
            while (value >= 0x80)
            {
                WriteByte((byte)(value | 0x80));
                value >>= 7;
            }

            WriteByte((byte)value);
        }

        public void WriteChar(char value)
        {
            if (WriterOption.Encoding == Encoding.Unicode)
            {
                Span<char> span = stackalloc char[1] { value };
                var bytes = MemoryMarshal.AsBytes(span);

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
            Span<bool> span = stackalloc bool[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(span));
        }

        public void WriteInt32(int value)
        {
            if (WriterOption.IsUsingVariant)
            {
                WriteVarInt32((uint)value);
            }
            else
            {
                Span<int> span = stackalloc int[1] { value };
                _bufferWriter.Write(MemoryMarshal.AsBytes(span));
            }
        }

        public void WriteUInt32(uint value)
        {
            if (WriterOption.IsUsingVariant)
            {
                WriteVarInt32(value);
            }
            else
            {
                Span<uint> span = stackalloc uint[1] { value };
                _bufferWriter.Write(MemoryMarshal.AsBytes(span));
            }
        }

        public void WriteVarInt32(uint value)
        {
            while (value >= 0x80)
            {
                WriteByte((byte)(value | 0x80));
                value >>= 7;
            }

            WriteByte((byte)value);
        }

        public void WriteSingle(float value)
        {
            Span<float> span = stackalloc float[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(span));
        }

        public void WriteInt64(long value)
        {
            if (WriterOption.IsUsingVariant)
            {
                WriteVarInt64((ulong)value);
            }
            else
            {
                Span<long> span = stackalloc long[1] { value };
                _bufferWriter.Write(MemoryMarshal.AsBytes(span));
            }
        }

        public void WriteUInt64(ulong value)
        {
            if (WriterOption.IsUsingVariant)
            {
                WriteVarInt64(value);
            }
            else
            {
                Span<ulong> span = stackalloc ulong[1] { value };
                _bufferWriter.Write(MemoryMarshal.AsBytes(span));
            }
        }

        public void WriteVarInt64(ulong value)
        {
            while (value >= 0x80)
            {
                WriteByte((byte)(value | 0x80));
                value >>= 7;
            }

            WriteByte((byte)value);
        }

        public void WriteDouble(double value)
        {
            Span<double> span = stackalloc double[1] { value };
            _bufferWriter.Write(MemoryMarshal.AsBytes(span));
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
