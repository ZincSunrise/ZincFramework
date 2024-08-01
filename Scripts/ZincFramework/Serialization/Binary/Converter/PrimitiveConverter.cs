using System;
using ZincFramework.Binary;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct PrimitiveConverter : ISerializeConvert
            {
                public object Convert(byte[] bytes, ref int nowIndex, Type type) => type switch
                {
                    not null when type == typeof(int) => ByteConverter.ToInt32(bytes, ref nowIndex),
                    not null when type == typeof(float) => ByteConverter.ToFloat(bytes, ref nowIndex),
                    not null when type == typeof(bool) => ByteConverter.ToBoolean(bytes, ref nowIndex),
                    not null when type == typeof(long) => ByteConverter.ToInt64(bytes, ref nowIndex),
                    not null when type == typeof(short) => ByteConverter.ToInt16(bytes, ref nowIndex),
                    not null when type == typeof(double) => ByteConverter.ToDouble(bytes, ref nowIndex),
                    not null when type == typeof(ushort) => ByteConverter.ToUInt16(bytes, ref nowIndex),
                    not null when type == typeof(uint) => ByteConverter.ToUInt32(bytes, ref nowIndex),
                    not null when type == typeof(ulong) => ByteConverter.ToUInt64(bytes, ref nowIndex),
                    not null when type == typeof(byte) => ByteConverter.ToByte(bytes, ref nowIndex),
                    not null when type == typeof(sbyte) => ByteConverter.ToSByte(bytes, ref nowIndex),
                    _ => throw new ArgumentException("不支持该类型" + type?.FullName)
                };

                public object Convert(ref ReadOnlySpan<byte> bytes, Type type)=> type switch
                {
                    not null when type == typeof(int) => ByteConverter.ToInt32(ref bytes),
                    not null when type == typeof(float) => ByteConverter.ToFloat(ref bytes),
                    not null when type == typeof(bool) => ByteConverter.ToBoolean(ref bytes),
                    not null when type == typeof(long) => ByteConverter.ToInt64(ref bytes),
                    not null when type == typeof(short) => ByteConverter.ToInt16(ref bytes),
                    not null when type == typeof(double) => ByteConverter.ToDouble(ref bytes),
                    not null when type == typeof(ushort) => ByteConverter.ToUInt16(ref bytes),
                    not null when type == typeof(uint) => ByteConverter.ToUInt32(ref bytes),
                    not null when type == typeof(ulong) => ByteConverter.ToUInt64(ref bytes),
                    not null when type == typeof(byte) => ByteConverter.ToByte(ref bytes),
                    not null when type == typeof(sbyte) => ByteConverter.ToSByte(ref bytes),
                    _ => throw new ArgumentException("不支持该类型" + type?.FullName)
                };
            }
        }
    }
}
