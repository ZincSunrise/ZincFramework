using System;
using ZincFramework.Binary;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct PrimitiveAppender : ISerializeAppend
            {
                public void Append(object obj, Type type, byte[] buffer, ref int nowIndex)
                {
                    switch (obj)
                    {
                        case int convertValue:
                            ByteAppender.AppendInt32(convertValue, buffer, ref nowIndex);
                            break;
                        case float convertValue:
                            ByteAppender.AppendFloat(convertValue, buffer, ref nowIndex);
                            break;
                        case bool convertValue:
                            ByteAppender.AppendBoolean(convertValue, buffer, ref nowIndex);
                            break;
                        case double convertValue:
                            ByteAppender.AppendDouble(convertValue, buffer, ref nowIndex);
                            break;
                        case long convertValue:
                            ByteAppender.AppendInt64(convertValue, buffer, ref nowIndex);
                            break;
                        case short convertValue:
                            ByteAppender.AppendInt16(convertValue, buffer, ref nowIndex);
                            break;
                        case ushort convertValue:
                            ByteAppender.AppendUInt16(convertValue, buffer, ref nowIndex);
                            break;
                        case uint convertValue:
                            ByteAppender.AppendUInt32(convertValue, buffer, ref nowIndex);
                            break;
                        case ulong convertValue:
                            ByteAppender.AppendUInt64(convertValue, buffer, ref nowIndex);
                            break;
                        case byte convertValue:
                            ByteAppender.AppendByte(convertValue, buffer, ref nowIndex);
                            break;
                        case sbyte convertValue:
                            ByteAppender.AppendSByte(convertValue, buffer, ref nowIndex);
                            break;
                        default:
                            throw new ArgumentException("你下发的类型不正确！");
                    }
                }

                public void Append(object obj, Type type, Span<byte> buffer, ref int nowIndex)
                {
                    switch (obj)
                    {
                        case int convertValue:
                            ByteAppender.AppendInt32(convertValue, buffer, ref nowIndex);
                            break;
                        case float convertValue:
                            ByteAppender.AppendFloat(convertValue, buffer, ref nowIndex);
                            break;
                        case bool convertValue:
                            ByteAppender.AppendBoolean(convertValue, buffer, ref nowIndex);
                            break;
                        case double convertValue:
                            ByteAppender.AppendDouble(convertValue, buffer, ref nowIndex);
                            break;
                        case long convertValue:
                            ByteAppender.AppendInt64(convertValue, buffer, ref nowIndex);
                            break;
                        case short convertValue:
                            ByteAppender.AppendInt16(convertValue, buffer, ref nowIndex);
                            break;
                        case ushort convertValue:
                            ByteAppender.AppendUInt16(convertValue, buffer, ref nowIndex);
                            break;
                        case uint convertValue:
                            ByteAppender.AppendUInt32(convertValue, buffer, ref nowIndex);
                            break;
                        case ulong convertValue:
                            ByteAppender.AppendUInt64(convertValue, buffer, ref nowIndex);
                            break;
                        case byte convertValue:
                            ByteAppender.AppendByte(convertValue, buffer, ref nowIndex);
                            break;
                        case sbyte convertValue:
                            ByteAppender.AppendSByte(convertValue, buffer, ref nowIndex);
                            break;
                        default:
                            throw new ArgumentException("你下发的类型不正确！");
                    }
                }
            }
        }
    }
}
