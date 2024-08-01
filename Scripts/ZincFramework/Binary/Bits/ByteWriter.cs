using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;



namespace ZincFramework
{
    namespace Binary
    {   
        public static class ByteWriter
        {
            public static void WriteBytes(ReadOnlySpan<byte> bytes, Stream stream)
            {
                stream.Write(bytes);
            }

            public static void WriteBytes(byte[] bytes, Stream stream)
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            public static void WriteInt16(short value, Stream stream)
            {
                Span<byte> bytes = stackalloc byte[2];
                BitConverter.TryWriteBytes(bytes, value);
                stream.Write(bytes);
            }

            public static void WriteInt32(int value, Stream stream)
            {
                Span<byte> bytes = stackalloc byte[4];
                BitConverter.TryWriteBytes(bytes, value);
                stream.Write(bytes);
            }

            public static void WriteInt64(long value, Stream stream)
            {
                Span<byte> bytes = stackalloc byte[8];
                BitConverter.TryWriteBytes(bytes, value);
                stream.Write(bytes);
            }

            public static void WriteUInt16(ushort value, Stream stream)
            {
                Span<byte> bytes = stackalloc byte[2];
                BitConverter.TryWriteBytes(bytes, value);
                stream.Write(bytes);
            }

            public static void WriteUInt32(uint value, Stream stream)
            {
                Span<byte> bytes = stackalloc byte[4];
                BitConverter.TryWriteBytes(bytes, value);
                stream.Write(bytes);
            }

            public static void WriteUInt64(ulong value, Stream stream)
            {
                Span<byte> bytes = stackalloc byte[8];
                BitConverter.TryWriteBytes(bytes, value);
                stream.Write(bytes);
            }

            public static void WriteBoolean(bool value, Stream stream)
            {
                Span<byte> bytes = stackalloc byte[1];
                BitConverter.TryWriteBytes(bytes, value);
                stream.Write(bytes);
            }

            public static void WriteFloat(float value, Stream stream)
            {
                Span<byte> bytes = stackalloc byte[4];
                BitConverter.TryWriteBytes(bytes, value);
                stream.Write(bytes);
            }

            public static void WriteDouble(double value, Stream stream)
            {
                Span<byte> bytes = stackalloc byte[4];
                BitConverter.TryWriteBytes(bytes, value);
                stream.Write(bytes);
            }

            public static void WriteString(string value, Stream stream)
            {
                ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(value);

                Span<byte> lengthBytes = stackalloc byte[4];
                BitConverter.TryWriteBytes(lengthBytes, bytes.Length);
                stream.Write(lengthBytes);
                stream.Write(bytes);
            }


            public static async Task WriteBytesAsync(byte[] bytes, Stream stream)
            {
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }

            public static async Task WriteInt16Async(short value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value), 0, 2);
            }

            public static async Task WriteInt32Async(int value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value), 0, 4);
            }

            public static async Task WriteInt64Async(long value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value), 0, 8);
            }

            public static async Task WriteUInt16Async(ushort value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value), 0, 2);
            }

            public static async Task WriteUInt32Async(uint value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value), 0, 4);
            }

            public static async Task WriteUInt64Async(ulong value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value), 0, 8);
            }

            public static async Task WriteBooleanAsync(bool value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value), 0, 1);
            }

            public static async Task WriteFloatAsync(float value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value), 0, 4);
            }

            public static async Task WriteDoubleAsync(double value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value), 0, 8);
            }

            public static async Task WriteStringAsync(string value, Stream stream)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                await stream.WriteAsync(BitConverter.GetBytes(bytes.Length), 0, 4);
                await stream.WriteAsync(Encoding.UTF8.GetBytes(value), 0, bytes.Length);
            }
        }
    }
}

