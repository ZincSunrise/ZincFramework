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
            public readonly struct QueueConverter : ISerializeConvert
            {
                public object Convert(byte[] bytes, ref int startIndex, Type type)
                {
                    int count = ByteConverter.ToInt16(bytes, ref startIndex);
                    ICollection collection = EnumerableConverter.GetCapacityConstructorMap(type).Invoke(count) as ICollection;
                    ConvertQueueImpl(collection, type, count, bytes, ref startIndex);

                    return collection;
                }

                public object Convert(ref ReadOnlySpan<byte> bytes, Type type)
                {
                    int count = ByteConverter.ToInt16(ref bytes);
                    ICollection collection = EnumerableConverter.GetCapacityConstructorMap(type).Invoke(count) as ICollection;
                    ConvertQueueImpl(collection, type, count, ref bytes);

                    return collection;
                }

                private void ConvertQueueImpl(ICollection tempQueue, Type type, int count, byte[] bytes, ref int startIndex)
                {
                    Type genericType = type.GenericTypeArguments[0];

                    if (genericType.IsPrimitive)
                    {
                        switch (tempQueue)
                        {
                            case Queue<int> intQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    intQueue.Enqueue(ByteConverter.ToInt32(bytes, ref startIndex));
                                }
                                break;
                            case Queue<float> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToFloat(bytes, ref startIndex));
                                }
                                break;
                            case Queue<bool> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToBoolean(bytes, ref startIndex));
                                }
                                break;
                            case Queue<long> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToInt64(bytes, ref startIndex));
                                }
                                break;
                            case Queue<double> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToDouble(bytes, ref startIndex));
                                }
                                break;
                            case Queue<short> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToInt16(bytes, ref startIndex));
                                }
                                break;
                            case Queue<ushort> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToUInt16(bytes, ref startIndex));
                                }
                                break;
                            case Queue<uint> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToUInt32(bytes, ref startIndex));
                                }
                                break;
                            case Queue<ulong> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToUInt64(bytes, ref startIndex));
                                }
                                break;
                            case Queue<byte> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(bytes[startIndex++]);
                                }
                                break;
                            case Queue<sbyte> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue((sbyte)bytes[startIndex++]);
                                }
                                break;
                        }
                    }
                    else if (tempQueue is Queue<string> strQueue)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            strQueue.Enqueue(ByteConverter.ToString(bytes, ref startIndex));
                        }
                    }
                    else if (tempQueue is Queue<TimeSpan> timeQueue)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            timeQueue.Enqueue(new TimeSpan(ByteConverter.ToInt64(bytes, ref startIndex)));
                        }
                    }
                    else
                    {    
                        MethodInfo methodInfo = SerializationCachePool.GetMethodInfo("Enqueue", type, genericType);
                        object[] args = new object[1];

                        if (genericType.IsEnum)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                args[0] = ByteConverter.ToInt32(bytes, ref startIndex);
                                methodInfo.Invoke(tempQueue, args);
                            }
                        }
                        else
                        {
                            ISerializeConvert serializeConvert = ConverterFactory.Shared.CreateBuilder(genericType);
                            for (int i = 0; i < count; i++)
                            {
                                args[0] = serializeConvert.Convert(bytes, ref startIndex, genericType);
                                methodInfo.Invoke(tempQueue, args);
                            }
                        }
                    }
                }
                
                
                 private void ConvertQueueImpl(ICollection tempQueue, Type type, int count, ref ReadOnlySpan<byte> bytes)
                {
                    Type genericType = type.GenericTypeArguments[0];

                    if (genericType.IsPrimitive)
                    {
                        switch (tempQueue)
                        {
                            case Queue<int> intQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    intQueue.Enqueue(ByteConverter.ToInt32(ref bytes));
                                }
                                break;
                            case Queue<float> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToFloat(ref bytes));
                                }
                                break;
                            case Queue<bool> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToBoolean(ref bytes));
                                }
                                break;
                            case Queue<long> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToInt64(ref bytes));
                                }
                                break;
                            case Queue<double> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToDouble(ref bytes));
                                }
                                break;
                            case Queue<short> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToInt16(ref bytes));
                                }
                                break;
                            case Queue<ushort> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToUInt16(ref bytes));
                                }
                                break;
                            case Queue<uint> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToUInt32(ref bytes));
                                }
                                break;
                            case Queue<ulong> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(ByteConverter.ToUInt64(ref bytes));
                                }
                                break;
                            case Queue<byte> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue(bytes[i]);
                                }

                                bytes = bytes[count..];
                                break;
                            case Queue<sbyte> newQueue:
                                for (int i = 0; i < count; i++)
                                {
                                    newQueue.Enqueue((sbyte)bytes[i]);
                                }
                                
                                bytes = bytes[count..];
                                break;
                        }
                    }
                    else if (tempQueue is Queue<string> strQueue)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            strQueue.Enqueue(ByteConverter.ToString(ref bytes));
                        }
                    }
                    else if (tempQueue is Queue<TimeSpan> timeQueue)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            timeQueue.Enqueue(new TimeSpan(ByteConverter.ToInt64(ref bytes)));
                        }
                    }
                    else
                    {    
                        MethodInfo methodInfo = SerializationCachePool.GetMethodInfo("Enqueue", type, genericType);
                        object[] args = new object[1];

                        if (genericType.IsEnum)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                args[0] = ByteConverter.ToInt32(ref bytes);
                                methodInfo.Invoke(tempQueue, args);
                            }
                        }
                        else
                        {
                            ISerializeConvert serializeConvert = ConverterFactory.Shared.CreateBuilder(genericType);
                            for (int i = 0; i < count; i++)
                            {
                                args[0] = serializeConvert.Convert(ref bytes, genericType);
                                methodInfo.Invoke(tempQueue, args);
                            }
                        }
                    }
                }
            }
        }
    }
}

