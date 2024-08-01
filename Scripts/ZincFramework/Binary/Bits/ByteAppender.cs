using System;
using System.Text;




namespace ZincFramework
{
    namespace Binary
    {
        public static class ByteAppender
        {
            public static void AppendInt16(short value, byte[] buffer, ref int startIndex)
            {
                Span<byte> bytes = new Span<byte>(buffer, startIndex, 2);
                BitConverter.TryWriteBytes(bytes, value);
                startIndex += 2;
            }

            public static void AppendInt32(int value, byte[] buffer, ref int startIndex)
            {
                Span<byte> bytes = new Span<byte>(buffer, startIndex, 4);
                BitConverter.TryWriteBytes(bytes, value);
                startIndex += 4;
            }

            public static void AppendInt64(long value, byte[] buffer, ref int startIndex)
            {
                Span<byte> bytes = new Span<byte>(buffer, startIndex, 8);
                BitConverter.TryWriteBytes(bytes, value);
                startIndex += 8;
            }

            public static void AppendByte(byte value, byte[] buffer, ref int startIndex)
            {
                buffer[startIndex++] = value;
            }

            public static void AppendSByte(sbyte value, byte[] buffer, ref int startIndex)
            {
                buffer[startIndex++] = (byte)value;
            }

            public static void AppendUInt16(ushort value, byte[] buffer, ref int startIndex)
            {
                Span<byte> bytes = new Span<byte>(buffer, startIndex, 2);
                BitConverter.TryWriteBytes(bytes, value);
                startIndex += 2;
            }

            public static void AppendUInt32(uint value, byte[] buffer, ref int startIndex)
            {
                Span<byte> bytes = new Span<byte>(buffer, startIndex, 4);
                BitConverter.TryWriteBytes(bytes, value);
                startIndex += 4;
            }

            public static void AppendUInt64(ulong value, byte[] buffer, ref int startIndex)
            {
                Span<byte> bytes = new Span<byte>(buffer, startIndex, 8);
                BitConverter.TryWriteBytes(bytes, value);
                startIndex += 8;
            }

            public static void AppendBoolean(bool value, byte[] buffer, ref int startIndex)
            {
                Span<byte> bytes = new Span<byte>(buffer, startIndex, 1);
                BitConverter.TryWriteBytes(bytes, value);
                startIndex += 1;
            }

            public static void AppendFloat(float value, byte[] buffer, ref int startIndex)
            {
                Span<byte> bytes = new Span<byte>(buffer, startIndex, 4);
                BitConverter.TryWriteBytes(bytes, value);
                startIndex += 4;
            }

            public static void AppendDouble(double value, byte[] buffer, ref int startIndex)
            {
                Span<byte> bytes = new Span<byte>(buffer, startIndex, 8);
                BitConverter.TryWriteBytes(bytes, value);
                startIndex += 8;
            }

            public static void AppendString(string value, byte[] buffer, ref int startIndex)
            {
                Span<byte> bytes = stackalloc byte[value.Length * 3];
                int length = Encoding.UTF8.GetBytes(value, bytes);

                Span<byte> bufferSpan = new Span<byte>(buffer, startIndex, 4 + length);
                BitConverter.TryWriteBytes(bufferSpan, length);
                startIndex += 4;

                bufferSpan = bufferSpan.Slice(4, length);
                bytes[..length].CopyTo(bufferSpan);
                startIndex += length;
            }


            public static void AppendInt16(short value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 2);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 2;
            }

            public static void AppendInt32(int value, Span<byte> buffer, ref int startIndex)
            {
                buffer = buffer.Slice(startIndex, 4);
                BitConverter.TryWriteBytes(buffer, value);
                startIndex += 4;
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

            public static void AppendString(string value, Span<byte> buffer, ref int startIndex)
            {
                Span<byte> bytes = stackalloc byte[value.Length * 3];
                int length = Encoding.UTF8.GetBytes(value, bytes);

                Span<byte> bufferSpan = buffer.Slice(startIndex, 4 + length);
                BitConverter.TryWriteBytes(bufferSpan, length);
                startIndex += 4;

                bufferSpan = bufferSpan.Slice(4, length);
                bytes[..length].CopyTo(bufferSpan);
                startIndex += length;
            }
        }
    }
}