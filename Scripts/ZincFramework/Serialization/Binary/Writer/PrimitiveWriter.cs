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
            public readonly struct PrimitiveWriter : ISerializeWrite
            {
                public void Write(object obj, Stream stream, Type type)
                {
                    switch (obj)
                    {
                        case int value:
                            ByteWriter.WriteInt32(value, stream);
                            break;
                        case float value:
                            ByteWriter.WriteFloat(value, stream);
                            break;
                        case bool value:
                            ByteWriter.WriteBoolean(value, stream);
                            break;
                        case double value:
                            ByteWriter.WriteDouble(value, stream);
                            break;
                        case long value:
                            ByteWriter.WriteInt64(value, stream);
                            break;
                        case short value:
                            ByteWriter.WriteInt16(value, stream);
                            break;
                        case ushort value:
                            ByteWriter.WriteUInt16(value, stream);
                            break;
                        case uint value:
                            ByteWriter.WriteUInt32(value, stream);
                            break;
                        case ulong value:
                            ByteWriter.WriteUInt64(value, stream);
                            break;
                        case byte or sbyte:
                            stream.WriteByte((byte)obj);
                            break;
                        default:
                            throw new ArgumentException("你下发的类型不正确！");
                    }
                }

                public async Task WriteAsync(object obj, Stream stream, Type type)
                {
                    switch (obj)
                    {
                        case int value:
                            await ByteWriter.WriteInt32Async(value, stream);
                            break;
                        case float value:
                            await ByteWriter.WriteFloatAsync(value, stream);
                            break;
                        case bool value:
                            await ByteWriter.WriteBooleanAsync(value, stream);
                            break;
                        case double value:
                            await ByteWriter.WriteDoubleAsync(value, stream);
                            break;
                        case long value:
                            await ByteWriter.WriteInt64Async(value, stream);
                            break;
                        case short value:
                            await ByteWriter.WriteInt16Async(value, stream);
                            break;
                        case ushort value:
                            await ByteWriter.WriteUInt16Async(value, stream);
                            break;
                        case uint value:
                            await ByteWriter.WriteUInt32Async(value, stream);
                            break;
                        case ulong value:
                            await ByteWriter.WriteUInt64Async(value, stream);
                            break;
                        case byte or sbyte:
                            stream.WriteByte((byte)obj);
                            break;
                        default:
                            throw new ArgumentException("你下发的类型不正确！");
                    }
                }

            }
        }
    }
}
