using System;
using ZincFramework.Binary;

namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct OtherSystemValueConverter : ISerializeConvert
            {
                public object Convert(byte[] bytes, ref int nowIndex, Type type)
                {
                    if (type.IsEnum)
                    {
                        return ByteConverter.ToInt32(bytes, ref nowIndex);
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        return new TimeSpan(ByteConverter.ToInt64(bytes, ref nowIndex));
                    }
                    else
                    {
                        throw new NotSupportedException("不支持该系统定义类");
                    }
                }

                public object Convert(ref ReadOnlySpan<byte> bytes, Type type)
                {
                    if (type.IsEnum)
                    {
                        return ByteConverter.ToInt32(ref bytes);
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        return new TimeSpan(ByteConverter.ToInt64(ref bytes));
                    }
                    else
                    {
                        throw new NotSupportedException("不支持该系统定义类");
                    }
                }
            }
        }
    }
}

