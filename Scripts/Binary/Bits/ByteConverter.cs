using System;
using System.Runtime.InteropServices;
using System.Text;


namespace ZincFramework
{
    namespace Binary
    {
        public static class ByteConverter
        {
            public static short ToInt16(byte[] bytes, ref int startIndex)
            {
                short value = BitConverter.ToInt16(bytes, startIndex);
                startIndex += 2;
                return value;
            }

            public static int ToInt32(byte[] bytes, ref int startIndex)
            {
                int value = BitConverter.ToInt32(bytes, startIndex);
                startIndex += 4;
                return value;
            }

            public static int ToVarInt32(byte[] bytes, ref int startIndex)
            {
                int value = 0;
                int shift = 0;

                while (true)
                {
                    value |= (bytes[startIndex] & 0x7F) << shift;

                    if ((bytes[startIndex++] & 0x80) == 0)
                    {
                        break;
                    }

                    shift += 7;
                }
                return value;
            }

            public static long ToInt64(byte[] bytes, ref int startIndex)
            {
                long value = BitConverter.ToInt64(bytes, startIndex);
                startIndex += 8;
                return value;
            }

            public static long ToVarInt64(byte[] bytes, ref int startIndex)
            {
                long value = 0;
                int shift = 0;

                while (true)
                {
                    value |= ((uint)bytes[startIndex] & 0x7F) << shift;

                    if ((bytes[startIndex++] & 0x80) == 0)
                    {
                        break;
                    }

                    shift += 7;
                }
                return value;
            }

            public static ushort ToUInt16(byte[] bytes, ref int startIndex)
            {
                ushort value = BitConverter.ToUInt16(bytes, startIndex);
                startIndex += 2;
                return value;
            }

            public static uint ToUInt32(byte[] bytes, ref int startIndex)
            {
                uint value = BitConverter.ToUInt32(bytes, startIndex);
                startIndex += 4;
                return value;
            }

            public static ulong ToUInt64(byte[] bytes, ref int startIndex)
            {
                ulong value = BitConverter.ToUInt64(bytes, startIndex);
                startIndex += 8;
                return value;
            }

            public static float ToFloat(byte[] bytes, ref int startIndex)
            {
                float value = BitConverter.ToSingle(bytes, startIndex);
                startIndex += 4;
                return value;
            }

            public static double ToDouble(byte[] bytes, ref int startIndex)
            {
                double value = BitConverter.ToDouble(bytes, startIndex);
                startIndex += 8;
                return value;
            }

            public static byte ToByte(byte[] bytes, ref int startIndex)
            {
                return bytes[startIndex++];
            }

            public static sbyte ToSByte(byte[] bytes, ref int startIndex)
            {
                return (sbyte)bytes[startIndex++];
            }

            public static bool ToBoolean(byte[] bytes, ref int startIndex)
            {
                bool value = BitConverter.ToBoolean(bytes, startIndex);
                startIndex += 1;
                return value;
            }

            /// <summary>
            /// 已经自动的将字符串长度读取出来了，无需多读取
            /// </summary>

            public static string ToString(byte[] bytes, ref int startIndex, Encoding encoding = null)
            {
                if (encoding == null || encoding == Encoding.Unicode)
                {
                    return ToStringCast(bytes, ref startIndex);
                }

                int length = ToInt32(bytes, ref startIndex);
                string str = Encoding.UTF8.GetString(bytes, startIndex, length);

                startIndex += length;
                return str;
            }

            public static string ToStringCast(byte[] bytes, ref int startIndex)
            {
                int length = ToInt32(bytes, ref startIndex);

                ReadOnlySpan<char> chars = MemoryMarshal.Cast<byte, char>(bytes.AsSpan().Slice(startIndex, length));
                startIndex += length;

                return new string(chars);
            }

            /// <summary>
            /// 注意，只可用于不含引用类型和不含含有引用类型结构体的结构体类型,例如基础类型
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="bytes"></param>
            /// <param name="size"></param>
            /// <param name="startIndex"></param>
            /// <returns></returns>
            public static T[] ToArray<T>(byte[] bytes, int size, ref int startIndex) where T : struct
            {
                short count = ToInt16(bytes, ref startIndex);
                size *= count;

                int nowIndex = startIndex;
                startIndex += size;
                return MemoryMarshal.Cast<byte, T>(bytes.AsSpan()[nowIndex..startIndex]).ToArray();
            }


            public static void SkipToString(byte[] bytes, ref int startIndex)
            {
                int length = ToInt32(bytes, ref startIndex);
                startIndex += length;
            }

            public static short ToInt16(ref ReadOnlySpan<byte> bytes)
            {
                short value = BitConverter.ToInt16(bytes);
                bytes = bytes[2..];
                return value;
            }

            public static int ToInt32(ref ReadOnlySpan<byte> bytes)
            {
                int value = BitConverter.ToInt32(bytes);
                bytes = bytes[4..];
                return value;
            }

            public static long ToInt64(ref ReadOnlySpan<byte> bytes)
            {
                long value = BitConverter.ToInt64(bytes);
                bytes = bytes[8..];
                return value;
            }

            public static ushort ToUInt16(ref ReadOnlySpan<byte> bytes)
            {
                ushort value = BitConverter.ToUInt16(bytes);
                bytes = bytes[2..];
                return value;
            }

            public static uint ToUInt32(ref ReadOnlySpan<byte> bytes)
            {
                uint value = BitConverter.ToUInt32(bytes);
                bytes = bytes[4..];
                return value;
            }

            public static ulong ToUInt64(ref ReadOnlySpan<byte> bytes)
            {
                ulong value = BitConverter.ToUInt64(bytes);
                bytes = bytes[8..];
                return value;
            }

            public static float ToFloat(ref ReadOnlySpan<byte> bytes)
            {
                float value = BitConverter.ToSingle(bytes);
                bytes = bytes[4..];
                return value;
            }

            public static double ToDouble(ref ReadOnlySpan<byte> bytes)
            {
                double value = BitConverter.ToDouble(bytes);
                bytes = bytes[8..];
                return value;
            }

            public static byte ToByte(ref ReadOnlySpan<byte> bytes)
            {
                byte value = bytes[0];
                bytes = bytes[1..];
                return value;
            }

            public static sbyte ToSByte(ref ReadOnlySpan<byte> bytes)
            {
                sbyte value = (sbyte)bytes[0];
                bytes = bytes[1..];
                return value;
            }

            public static bool ToBoolean(ref ReadOnlySpan<byte> bytes)
            {
                bool value = BitConverter.ToBoolean(bytes);
                bytes = bytes[1..];
                return value;
            }

            /// <summary>
            /// 已经自动的将字符串长度读取出来了，无需多读取
            /// </summary>
            /// <param name="bytes"></param>
            /// <param name="startIndex"></param>
            /// <returns></returns>

            public static string ToString(ref ReadOnlySpan<byte> bytes, Encoding encoding = null)
            {
                if (encoding == null || encoding == Encoding.Unicode)
                {
                    return ToStringCast(ref bytes);
                }

                int length = ToInt32(ref bytes);
                string str = Encoding.UTF8.GetString(bytes[..length]);

                bytes = bytes[length..];
                return str;
            }

            public static string ToStringCast(ref ReadOnlySpan<byte> bytes)
            {
                int length = ToInt32(ref bytes);
                ReadOnlySpan<char> chars = MemoryMarshal.Cast<byte, char>(bytes[..length]);
                bytes = bytes[length..];

                return new string(chars);
            }


            /// <summary>
            /// 注意，只可用于不含引用类型和不含含有引用类型结构体的结构体类型,例如基础类型
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="bytes"></param>
            /// <param name="size"></param>
            /// <returns></returns>
            public static T[] ToArray<T>(ref ReadOnlySpan<byte> bytes, int size) where T : struct
            {
                short count = ToInt16(ref bytes);
                size *= count;

                ReadOnlySpan<T> result = MemoryMarshal.Cast<byte, T>(bytes[..size]);

                bytes = bytes[size..];
                return result.ToArray();
            }
        }
    }
}

