using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace ZincFramework
{
    namespace Binary
    {
        public static class BytesWriter
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

            public static void WriteVarInt32(int value, Stream stream)
            {
                uint unsignedValue = (uint)value;
                while (unsignedValue >= 0x80)
                {
                    stream.WriteByte((byte)(unsignedValue | 0x80));
                    unsignedValue >>= 7;
                }

                stream.WriteByte((byte)unsignedValue);
            }

            public static void WriteInt64(long value, Stream stream)
            {
                Span<byte> bytes = stackalloc byte[8];
                BitConverter.TryWriteBytes(bytes, value);
                stream.Write(bytes);
            }

            public static void WriteVarInt64(long value, Stream stream)
            {
                ulong unsignedValue = (ulong)value;
                while (unsignedValue >= 0x80)
                {
                    stream.WriteByte((byte)(unsignedValue | 0x80));
                    unsignedValue >>= 7;
                }

                stream.WriteByte((byte)unsignedValue);
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

            public static void WriteString(string value, Stream stream, Encoding encoding = null)
            {
                ReadOnlySpan<byte> bytes;
                if (encoding == null || encoding == Encoding.Unicode)
                {
                    bytes = MemoryMarshal.AsBytes<char>(value);
                }
                else
                {
                    bytes = encoding.GetBytes(value);
                }

                Span<byte> lengthBytes = stackalloc byte[4];
                BitConverter.TryWriteBytes(lengthBytes, bytes.Length);
                stream.Write(lengthBytes);
                stream.Write(bytes);
            }

            public static void WriteArray<T>(T[] value, Stream stream) where T : struct
            {
                WriteInt16((short)value.Length, stream);
                ReadOnlySpan<byte> bytes = MemoryMarshal.Cast<T, byte>(value);
                stream.Write(bytes);
            }

            public static async Task WriteBytesAsync(byte[] bytes, Stream stream)
            {
                await stream.WriteAsync(bytes.AsMemory(0, 1));
            }

            public static async Task WriteInt16Async(short value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value));
            }

            public static async Task WriteInt32Async(int value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value));
            }

            public static async Task WriteInt64Async(long value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value));
            }

            public static async Task WriteUInt16Async(ushort value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value));
            }

            public static async Task WriteUInt32Async(uint value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value));
            }

            public static async Task WriteUInt64Async(ulong value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value));
            }

            public static async Task WriteBooleanAsync(bool value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value));
            }

            public static async Task WriteFloatAsync(float value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value));
            }

            public static async Task WriteDoubleAsync(double value, Stream stream)
            {
                await stream.WriteAsync(BitConverter.GetBytes(value));
            }

            public static async Task WriteStringAsync(string value, Stream stream, Encoding encoding = null)
            {
                byte[] bytes;
                if (encoding == null || encoding == Encoding.Unicode)
                {
                    bytes = MemoryMarshal.AsBytes<char>(value).ToArray();
                }
                else
                {
                    bytes = encoding.GetBytes(value);
                }

                await WriteInt32Async(bytes.Length, stream);
                await stream.WriteAsync(bytes);
            }

            public static async Task WriteArrayAsync<T>(T[] value, Stream stream) where T : struct
            {
                WriteInt16((short)value.Length, stream);
                ReadOnlyMemory<byte> byteMemory = MemoryMarshal.AsBytes(value.AsSpan()).ToArray();
                await stream.WriteAsync(byteMemory);
            }
        }
    }
}

