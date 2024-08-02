
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ZincFramework.Binary;
using ZincFramework.Serialization.Cache;
using ZincFramework.Serialization.Factory;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct LinkedListConverter : ISerializeConvert
            {
                public object Convert(byte[] bytes, ref int nowIndex, Type type)
                {
                    int count = ByteConverter.ToInt16(bytes, ref nowIndex);
                    ICollection collection = Activator.CreateInstance(type) as ICollection;

                    ConvertLinkedListImpl(collection, type, count, bytes, ref nowIndex);

                    return collection;
                }

                public object Convert(ref ReadOnlySpan<byte> bytes, Type type)
                {
                    int count = ByteConverter.ToInt16(ref bytes);
                    ICollection collection = Activator.CreateInstance(type) as ICollection;

                    ConvertLinkedListImpl(collection, type, count, ref bytes);
                    return collection;
                }
                
                public void ConvertLinkedListImpl(ICollection tempLinkedList, Type type, int count, byte[] buffer, ref int startIndex)
                {
                    Type genericType = type.GenericTypeArguments[0];

                    if (genericType.IsPrimitive)
                    {
                        switch (tempLinkedList)
                        {
                            case LinkedList<int> intLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    intLinkedList.AddLast(ByteConverter.ToInt32(buffer, ref startIndex));
                                }
                                break;
                            case LinkedList<float> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToFloat(buffer, ref startIndex));
                                }
                                break;
                            case LinkedList<bool> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToBoolean(buffer, ref startIndex));
                                }
                                break;
                            case LinkedList<long> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToInt64(buffer, ref startIndex));
                                }
                                break;
                            case LinkedList<double> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToDouble(buffer, ref startIndex));
                                }
                                break;
                            case LinkedList<short> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToInt16(buffer, ref startIndex));
                                }
                                break;
                            case LinkedList<ushort> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToUInt16(buffer, ref startIndex));
                                }
                                break;
                            case LinkedList<uint> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToUInt32(buffer, ref startIndex));
                                }
                                break;
                            case LinkedList<ulong> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToUInt64(buffer, ref startIndex));
                                }
                                break;
                            case LinkedList<byte> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(buffer[startIndex++]);
                                }
                                break;
                            case LinkedList<sbyte> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast((sbyte)buffer[startIndex++]);
                                }
                                break;
                        }
                    }
                    else if (tempLinkedList is LinkedList<string> strLinkedList)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            strLinkedList.AddLast(ByteConverter.ToString(buffer, ref startIndex));
                        }
                    }
                    else if (tempLinkedList is LinkedList<TimeSpan> timeLinkedList)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            timeLinkedList.AddLast(new TimeSpan(ByteConverter.ToInt64(buffer, ref startIndex)));
                        }
                    }
                    else
                    {
                        MethodInfo methodInfo = SerializationCachePool.GetMethodInfo("AddLast", type, genericType);
                        object[] args = new object[1];

                        if (genericType.IsEnum)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                args[0] = ByteConverter.ToInt32(buffer, ref startIndex);
                                methodInfo.Invoke(tempLinkedList, args);
                            }
                        }
                        else
                        {
                            ISerializeConvert serializeConvert = ConverterFactory.Shared.CreateBuilder(genericType);
                            for (int i = 0; i < count; i++)
                            {
                                args[0] = serializeConvert.Convert(buffer, ref startIndex, genericType);
                                methodInfo.Invoke(tempLinkedList, args);
                            }
                        }
                    }
                }
                
                public void ConvertLinkedListImpl(ICollection tempLinkedList, Type type, int count, ref ReadOnlySpan<byte> bytes)
                {
                    Type genericType = type.GenericTypeArguments[0];

                    if (genericType.IsPrimitive)
                    {
                        switch (tempLinkedList)
                        {
                            case LinkedList<int> intLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    intLinkedList.AddLast(ByteConverter.ToInt32(ref bytes));
                                }
                                break;
                            case LinkedList<float> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToFloat(ref bytes));
                                }
                                break;
                            case LinkedList<bool> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToBoolean(ref bytes));
                                }
                                break;
                            case LinkedList<long> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToInt64(ref bytes));
                                }
                                break;
                            case LinkedList<double> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToDouble(ref bytes));
                                }
                                break;
                            case LinkedList<short> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToInt16(ref bytes));
                                }
                                break;
                            case LinkedList<ushort> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToUInt16(ref bytes));
                                }
                                break;
                            case LinkedList<uint> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToUInt32(ref bytes));
                                }
                                break;
                            case LinkedList<ulong> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(ByteConverter.ToUInt64(ref bytes));
                                }
                                break;
                            case LinkedList<byte> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast(bytes[i]);
                                }

                                bytes = bytes[count..];
                                break;
                            case LinkedList<sbyte> newLinkedList:
                                for (int i = 0; i < count; i++)
                                {
                                    newLinkedList.AddLast((sbyte)bytes[i]);
                                }
                                
                                bytes = bytes[count..];
                                break;
                        }
                    }
                    else if (tempLinkedList is LinkedList<string> strLinkedList)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            strLinkedList.AddLast(ByteConverter.ToString(ref bytes));
                        }
                    }
                    else if (tempLinkedList is LinkedList<TimeSpan> timeLinkedList)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            timeLinkedList.AddLast(new TimeSpan(ByteConverter.ToInt64(ref bytes)));
                        }
                    }
                    else
                    {
                        MethodInfo methodInfo = SerializationCachePool.GetMethodInfo("AddLast", type, genericType);
                        object[] args = new object[1];

                        if (genericType.IsEnum)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                args[0] = ByteConverter.ToInt32(ref bytes);
                                methodInfo.Invoke(tempLinkedList, args);
                            }
                        }
                        else
                        {
                            ISerializeConvert serializeConvert = ConverterFactory.Shared.CreateBuilder(genericType);
                            for (int i = 0; i < count; i++)
                            {
                                args[0] = serializeConvert.Convert(ref bytes, genericType);
                                methodInfo.Invoke(tempLinkedList, args);
                            }
                        }
                    }
                }
            }
        }
    }
}

