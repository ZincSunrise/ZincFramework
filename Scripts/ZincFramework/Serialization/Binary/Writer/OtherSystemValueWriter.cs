using System;
using System.IO;
using System.Threading.Tasks;
using ZincFramework.Binary;

namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct OtherSystemValueWriter : ISerializeWrite
            {
                public void Write(object obj, Stream stream, Type type)
                {
                    switch (obj)
                    {
                        case Enum:
                            ByteWriter.WriteInt32((int)obj, stream);
                            break;
                        case TimeSpan value:
                            ByteWriter.WriteInt64(value.Ticks, stream);
                            break;
                        default:
                            throw new NotSupportedException("不支持该系统定义类");
                    }
                }

                public async Task WriteAsync(object obj, Stream stream, Type type)
                {
                    switch (obj)
                    {
                        case Enum value:
                            await ByteWriter.WriteInt32Async((int)obj, stream);
                            break;
                        case TimeSpan value:
                            await ByteWriter.WriteInt64Async(value.Ticks, stream);
                            break;
                        default:
                            throw new NotSupportedException("不支持该系统定义类");
                    }
                }
            }
        }
    }
}

