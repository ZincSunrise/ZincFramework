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
            public readonly struct ArrayListAppender : ICollectionAppend<IList>
            {
                public void Append(IList list, Type genericType, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendInt16((short)list.Count, buffer, ref nowIndex);
                    AppendValueImpl(list, genericType, buffer, ref nowIndex);
                }

                public void Append(IList list, Type genericType, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendInt16((short)list.Count, buffer, ref nowIndex);
                    AppendValueImpl(list, genericType, buffer, ref nowIndex);
                }
                
                
                private void AppendValueImpl(IList tempList, Type genericType, Span<byte> buffer, ref int startIndex)
                {
                    if (genericType.IsPrimitive)
                    {
                        switch (tempList)
                        {
                            case IList<int> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteAppender.AppendInt32(list[i], buffer, ref startIndex);
                                }
                                break;
                            case IList<float> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteAppender.AppendFloat(list[i], buffer, ref startIndex);
                                }
                                break;
                            case IList<bool> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteAppender.AppendBoolean(list[i], buffer, ref startIndex);
                                }
                                break;
                            case IList<long> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteAppender.AppendInt64(list[i], buffer, ref startIndex);
                                }
                                break;
                            case IList<double> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteAppender.AppendDouble(list[i], buffer, ref startIndex);
                                }
                                break;
                            case IList<short> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteAppender.AppendInt16(list[i], buffer, ref startIndex);
                                }
                                break;
                            case IList<ushort> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteAppender.AppendUInt16(list[i], buffer, ref startIndex);
                                }
                                break;
                            case IList<uint> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteAppender.AppendUInt32(list[i], buffer, ref startIndex);
                                }
                                break;
                            case IList<ulong> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteAppender.AppendUInt64(list[i], buffer, ref startIndex);
                                }
                                break;
                            case IList<byte> list:                              
                                for (int i = 0; i < list.Count; i++)
                                {
                                    buffer[startIndex++] = list[i];
                                }
                                break;
                            case IList<sbyte> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    buffer[startIndex++] = (byte)list[i];
                                }
                                break;
                        }
                    }
                    else if (tempList is IList<string> list)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            ByteAppender.AppendString(list[i], buffer, ref startIndex);
                        }
                    }
                    else if (tempList is IList<TimeSpan> timeList)
                    {
                        for (int i = 0; i < timeList.Count; i++)
                        {
                            ByteAppender.AppendInt64(timeList[i].Ticks, buffer, ref startIndex);
                        }
                    }
                    else if (genericType.IsEnum)
                    {
                        for (int i = 0; i < tempList.Count; i++)
                        {
                            ByteAppender.AppendInt32((int)(tempList[i] ?? 0), buffer, ref startIndex);
                        }
                    }
                    else
                    {
                        ISerializeAppend appender = AppenderFactory.Shared.CreateBuilder(genericType);

                        for (int i = 0; i < tempList.Count; i++)
                        {
                            appender.Append(tempList[i], genericType, buffer, ref startIndex);
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
