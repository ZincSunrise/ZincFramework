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
            public readonly struct HashSetConverter : ISerializeConvert
            {
                public object Convert(byte[] bytes, ref int nowIndex, Type type)
                {
                    int count = ByteConverter.ToInt16(bytes, ref nowIndex);
                    IEnumerable set = EnumerableConverter.GetCapacityConstructorMap(type).Invoke(count);
                    ConvertISetImpl(set, type, count, bytes, ref nowIndex);

                    return set;
                }

                public object Convert(ref ReadOnlySpan<byte> bytes, Type type)
                {
                    int count = ByteConverter.ToInt16(ref bytes);
                    IEnumerable set = EnumerableConverter.GetCapacityConstructorMap(type).Invoke(count);
                    ConvertISetImpl(set, type, count, ref bytes);

                    return set;
                }
                
                
                private void ConvertISetImpl(IEnumerable tempSet, Type type, int count, byte[] bytes, ref int startIndex)
                {
                    Type genericType = type.GenericTypeArguments[0];
                    if (genericType.IsPrimitive)
                    {
                        switch (tempSet)
                        {
                            case ISet<int> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToInt32(bytes, ref startIndex));
                                }
                                break;
                            case ISet<float> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToFloat(bytes, ref startIndex));
                                }
                                break;
                            case ISet<bool> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToBoolean(bytes, ref startIndex));
                                }
                                break;
                            case ISet<long> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToInt64(bytes, ref startIndex));
                                }
                                break;
                            case ISet<double> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToDouble(bytes, ref startIndex));
                                }
                                break;
                            case ISet<short> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToInt16(bytes, ref startIndex));
                                }
                                break;
                            case ISet<ushort> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToUInt16(bytes, ref startIndex));
                                }
                                break;
                            case ISet<uint> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToUInt32(bytes, ref startIndex));
                                }
                                break;
                            case ISet<ulong> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToUInt64(bytes, ref startIndex));
                                }
                                break;
                            case ISet<byte> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(bytes[startIndex++]);
                                }
                                break;
                            case ISet<sbyte> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add((sbyte)bytes[startIndex++]);
                                }
                                break;
                        }
                    }
                    else if (tempSet is ISet<string> set)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            set.Add(ByteConverter.ToString(bytes, ref startIndex));
                        }
                    }
                    else if (tempSet is ISet<TimeSpan> timeSet)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            timeSet.Add(new TimeSpan(ByteConverter.ToInt64(bytes, ref startIndex)));
                        }
                    }
                    else
                    {
                        MethodInfo methodInfo = SerializationCachePool.GetMethodInfo("Add", type ,genericType);
                        object[] objects = new object[1];

                        if (genericType.IsEnum)
                        {
                            for (int i = 0; i < count; i++)
                            {    
                                objects[0] = ByteConverter.ToInt32(bytes, ref startIndex);
                                methodInfo.Invoke(tempSet, objects);
                            }
                        }
                        else
                        {
                            ISerializeConvert convert = ConverterFactory.Shared.CreateBuilder(genericType);
                            for (int i = 0; i < count; i++)
                            {
                                objects[0] = convert.Convert(bytes, ref startIndex, genericType);
                                methodInfo.Invoke(tempSet, objects);
                            }
                        }
                    }
                }
                
                
                private void ConvertISetImpl(IEnumerable tempSet, Type type, int count, ref ReadOnlySpan<byte> bytes)
                {
                    Type genericType = type.GenericTypeArguments[0];
                    if (genericType.IsPrimitive)
                    {
                        switch (tempSet)
                        {
                            case ISet<int> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToInt32(ref bytes));
                                }
                                break;
                            case ISet<float> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToFloat(ref bytes));
                                }
                                break;
                            case ISet<bool> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToBoolean(ref bytes));
                                }
                                break;
                            case ISet<long> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToInt64(ref bytes));
                                }
                                break;
                            case ISet<double> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToDouble(ref bytes));
                                }
                                break;
                            case ISet<short> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToInt16(ref bytes));
                                }
                                break;
                            case ISet<ushort> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToUInt16(ref bytes));
                                }
                                break;
                            case ISet<uint> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToUInt32(ref bytes));
                                }
                                break;
                            case ISet<ulong> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(ByteConverter.ToUInt64(ref bytes));
                                }
                                break;
                            case ISet<byte> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add(bytes[i]);
                                }
                                
                                bytes = bytes[count..];
                                break;
                            case ISet<sbyte> set:
                                for (int i = 0; i < count; i++)
                                {
                                    set.Add((sbyte)bytes[i]);
                                }

                                bytes = bytes[count..];
                                break;
                        }
                    }
                    else if (tempSet is ISet<string> set)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            set.Add(ByteConverter.ToString(ref bytes));
                        }
                    }
                    else if (tempSet is ISet<TimeSpan> timeSet)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            timeSet.Add(new TimeSpan(ByteConverter.ToInt64(ref bytes)));
                        }
                    }
                    else
                    {
                        MethodInfo methodInfo = SerializationCachePool.GetMethodInfo("Add", type ,genericType);
                        object[] objects = new object[1];

                        if (genericType.IsEnum)
                        {
                            for (int i = 0; i < count; i++)
                            {    
                                objects[0] = ByteConverter.ToInt32(ref bytes);
                                methodInfo.Invoke(tempSet, objects);
                            }
                        }
                        else
                        {
                            ISerializeConvert convert = ConverterFactory.Shared.CreateBuilder(genericType);
                            for (int i = 0; i < count; i++)
                            {
                                objects[0] = convert.Convert(ref bytes, genericType);
                                methodInfo.Invoke(tempSet, objects);
                            }
                        }
                    }
                }
            }
        }
    }
}

