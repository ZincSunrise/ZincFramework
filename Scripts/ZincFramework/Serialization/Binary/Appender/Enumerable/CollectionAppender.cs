
using System;
using System.Collections;
using System.Collections.Generic;
using ZincFramework.Binary;
using ZincFramework.Serialization.Factory;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public struct CollectionAppender : ICollectionAppend<IEnumerable>
            {
                public void Append(IEnumerable enumerable, Type genericType, byte[] buffer, ref int nowIndex)
                {
                    Append(enumerable, genericType, buffer.AsSpan(), ref nowIndex);
                }

                public void Append(IEnumerable enumerable, Type genericType, Span<byte> buffer, ref int nowIndex)
                {
                    if (enumerable is ICollection collection)
                    {
                        ByteAppender.AppendInt16((short)collection.Count, buffer, ref nowIndex);
                        AppendValueImpl(collection, genericType, buffer,ref nowIndex);
                    }
                    else
                    {
                        short count = 0;
                        foreach (var item in enumerable)
                        {
                            count++;
                        }
                        
                        ByteAppender.AppendInt16(count, buffer, ref nowIndex);
                        AppendValueImpl(enumerable, genericType, buffer,ref nowIndex);
                    }
                }
                
                
                private void AppendValueImpl(IEnumerable tempCollection, Type genericType, Span<byte> buffer, ref int nowIndex)
                {
                    if (genericType.IsPrimitive)
                    {
                        switch (tempCollection)
                        {
                            case IEnumerable<int> collection:
                                foreach (var item in collection)
                                {
                                    ByteAppender.AppendInt32(item, buffer, ref nowIndex);
                                }
                                break;
                            case IEnumerable<float> collection:
                                foreach (var item in collection)
                                {
                                    ByteAppender.AppendFloat(item, buffer, ref nowIndex);
                                }
                                break;
                            case IEnumerable<bool> collection:
                                foreach (var item in collection)
                                {
                                    ByteAppender.AppendBoolean(item, buffer, ref nowIndex);
                                }
                                break;
                            case IEnumerable<long> collection:
                                foreach (var item in collection)
                                {
                                    ByteAppender.AppendInt64(item, buffer, ref nowIndex);
                                }
                                break;
                            case IEnumerable<double> collection:
                                foreach (var item in collection)
                                {
                                    ByteAppender.AppendDouble(item, buffer, ref nowIndex);
                                }
                                break;
                            case IEnumerable<short> collection:
                                foreach (var item in collection)
                                {
                                    ByteAppender.AppendInt16(item, buffer, ref nowIndex);
                                }
                                break;
                            case IEnumerable<ushort> collection:
                                foreach (var item in collection)
                                {
                                    ByteAppender.AppendUInt16(item, buffer, ref nowIndex);
                                }
                                break;
                            case IEnumerable<uint> collection:
                                foreach (var item in collection)
                                {
                                    ByteAppender.AppendUInt32(item, buffer, ref nowIndex);
                                }
                                break;
                            case IEnumerable<ulong> collection:
                                foreach (var item in collection)
                                {
                                    ByteAppender.AppendUInt64(item, buffer, ref nowIndex);
                                }
                                break;
                            case IEnumerable<byte> collection:
                                foreach (var item in collection)
                                {
                                    buffer[nowIndex++] = item;
                                }
                                break;
                            case IEnumerable<sbyte> collection:
                                foreach (var item in collection)
                                {
                                    buffer[nowIndex++] = (byte)item;
                                }
                                break;
                        }
                    }
                    else if (tempCollection is IEnumerable<string> collection)
                    {
                        foreach (var item in collection)
                        {
                            ByteAppender.AppendString(item, buffer, ref nowIndex);
                        }
                    }
                    else if (tempCollection is IEnumerable<TimeSpan> timeList)
                    {
                        foreach (var item in timeList)
                        {
                            ByteAppender.AppendInt64(item.Ticks, buffer, ref nowIndex);
                        }
                    }
                    else if (genericType.IsEnum)
                    {
                        foreach (var item in tempCollection)
                        {
                            ByteAppender.AppendInt32((int)item, buffer, ref nowIndex);
                        }
                    }
                    else
                    {
                        ISerializeAppend appender = AppenderFactory.Shared.CreateBuilder(genericType);
                        foreach (var item in tempCollection)
                        {
                            appender.Append(item, genericType, buffer, ref nowIndex);
                        }
                    }
                }

                void ISerializeAppend.Append(object obj, Type type, byte[] buffer, ref int nowIndex)
                {
                    throw new NotImplementedException();
                }

                void ISerializeAppend.Append(object obj, Type type, Span<byte> buffer, ref int nowIndex)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}

