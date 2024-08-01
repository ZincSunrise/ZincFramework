using System;
using ZincFramework.Binary;

namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct OtherSystemValueAppender : ISerializeAppend
            {
                public void Append(object obj, Type type, byte[] buffer, ref int nowIndex)
                {
                    Append(obj, type, buffer.AsSpan(), ref nowIndex);
                }

                public void Append(object obj, Type type, Span<byte> buffer, ref int nowIndex)
                {
                    switch (obj)
                    {
                        case Enum:
                            ByteAppender.AppendInt32((int)obj, buffer, ref nowIndex);
                            break;
                        case TimeSpan span:
                            ByteAppender.AppendInt64(span.Ticks, buffer, ref nowIndex);
                            break;
                        default:
                            throw new NotSupportedException("不支持该系统类");
                    }
                }
            }
        }
    }
}

