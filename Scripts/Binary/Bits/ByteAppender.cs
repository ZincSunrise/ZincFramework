using System;
using System.Runtime.InteropServices;
using System.Text;




namespace ZincFramework
{
    namespace Binary
    {
        public static class ByteAppender
        {
            public static void AppendInt16(short value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 2);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 2;
            }

            public static void AppendVarInt32(int value, Span<byte> buffer, ref int startIndex)
            {
                uint unsignedValue = (uint)value;
                while (unsignedValue >= 0x80)
                {
                    buffer[startIndex++] = (byte)(unsignedValue | 0x80);
                    unsignedValue >>= 7;
                }

                buffer[startIndex++] = (byte)unsignedValue;
            }

            public static void AppendInt32(int value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 4);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 4;
            }

            public static void AppendVarInt64(long value, Span<byte> buffer, ref int startIndex)
            {
                ulong unsignedValue = (ulong)value;
                while (unsignedValue >= 0x80)
                {
                    buffer[startIndex++] = (byte)(unsignedValue | 0x80);
                    unsignedValue >>= 7;
                }

                buffer[startIndex++] = (byte)unsignedValue;
            }

            public static void AppendInt64(long value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 8);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 8;
            }

            public static void AppendByte(byte value, Span<byte> buffer, ref int startIndex)
            {
                buffer[startIndex++] = value;
            }

            public static void AppendSByte(sbyte value, Span<byte> buffer, ref int startIndex)
            {
                buffer[startIndex++] = (byte)value;
            }

            public static void AppendUInt16(ushort value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 2);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 2;
            }

            public static void AppendUInt32(uint value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 4);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 4;
            }

            public static void AppendUInt64(ulong value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 8);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 8;
            }

            public static void AppendBoolean(bool value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 1);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 1;
            }

            public static void AppendFloat(float value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 4);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 4;
            }

            public static void AppendDouble(double value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 8);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 8;
            }

            public static void AppendString(string value, Span<byte> buffer, ref int startIndex, Encoding encoding = null)
            {
                if (encoding == null || encoding == Encoding.Unicode)
                {
                    AppendStringCast(value, buffer, ref startIndex);
                    return;
                }

                Span<byte> bytes = stackalloc byte[value.Length * 3];
                int length = encoding.GetBytes(value, bytes);

                Span<byte> bufferSpan = buffer.Slice(startIndex, 4 + length);
                BitConverter.TryWriteBytes(bufferSpan, length);
                startIndex += 4;

                bufferSpan = bufferSpan.Slice(4, length);
                bytes[..length].CopyTo(bufferSpan);
                startIndex += length;
            }

            public static void AppendStringCast(string value, Span<byte> buffer, ref int startIndex)
            {
                ReadOnlySpan<byte> bytes = MemoryMarshal.AsBytes<char>(value);

                int length = bytes.Length;
                AppendInt32(length, buffer, ref startIndex);

                buffer = buffer.Slice(startIndex, length);
                bytes.CopyTo(buffer);

                startIndex += length;
            }

            public static void AppendArray<T>(T[] value, Span<byte> buffer, ref int startIndex) where T : struct
            {
                AppendInt16((short)value.Length, buffer, ref startIndex);
                ReadOnlySpan<byte> bytes = MemoryMarshal.Cast<T, byte>(value);

                buffer = buffer.Slice(startIndex, bytes.Length);
                bytes.CopyTo(buffer);

                startIndex += bytes.Length;
            }
        }
    }
}