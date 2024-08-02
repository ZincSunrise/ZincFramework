using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            public readonly struct StackConverter : ISerializeConvert
            {
                public object Convert(byte[] bytes, ref int startIndex, Type type)
                {
                    int count = ByteConverter.ToInt16(bytes, ref startIndex);
                    ICollection collection = EnumerableConverter.GetCapacityConstructorMap(type).Invoke(count) as ICollection;

                    ConvertStackImpl(collection, type, count, bytes, ref startIndex);

                    return collection;
                }

                public object Convert(ref ReadOnlySpan<byte> bytes, Type type)
                {
                    int count = ByteConverter.ToInt16(ref bytes);
                    ICollection collection = EnumerableConverter.GetCapacityConstructorMap(type).Invoke(count) as ICollection;

                    ConvertStackImpl(collection, type, count,ref bytes);

                    return collection;
                }

                private void ConvertStackImpl(ICollection tempStack, Type type, int count, byte[] bytes, ref int startIndex)
                {
                    Type genericType = type.GenericTypeArguments[0];


                    if (genericType.IsPrimitive)
                    {
                        switch (tempStack)
                        {
                            case Stack<int> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToInt32(bytes, ref startIndex));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<float> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToFloat(bytes, ref startIndex));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<bool> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToBoolean(bytes, ref startIndex));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<long> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToInt64(bytes, ref startIndex));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<double> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToDouble(bytes, ref startIndex));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<short> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToInt16(bytes, ref startIndex));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<ushort> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToUInt16(bytes, ref startIndex));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<uint> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToUInt32(bytes, ref startIndex));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<ulong> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToUInt64(bytes, ref startIndex));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<byte> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(bytes[startIndex++]);
                                }

                                newStack.Reverse();
                                break;
                            case Stack<sbyte> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push((sbyte)bytes[startIndex++]);
                                }

                                newStack.Reverse();
                                break;
                        }
                    }
                    else if (tempStack is Stack<string> strStack)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            strStack.Push(ByteConverter.ToString(bytes, ref startIndex));
                        }

                        strStack.Reverse();
                    }
                    else if (tempStack is Stack<TimeSpan> timeStack)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            timeStack.Push(new TimeSpan(ByteConverter.ToInt64(bytes, ref startIndex)));
                        }

                        timeStack.Reverse();
                    }
                    else
                    {
                        MethodInfo methodInfo = SerializationCachePool.GetMethodInfo("Push", type, genericType);
                        object[] args = new object[1];
                        Stack stack = new Stack(count);

                        if (genericType.IsEnum)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                stack.Push(ByteConverter.ToInt32(bytes, ref startIndex));
                            }
                        }
                        else
                        {
                            ISerializeConvert serializeConvert = ConverterFactory.Shared.CreateBuilder(genericType);
                            for (int i = 0; i < count; i++)
                            {
                                stack.Push(args[0] = serializeConvert.Convert(bytes, ref startIndex, genericType));
                            }
                        }

                        while (stack.Count > 0)
                        {
                            args[0] = stack.Pop();
                            methodInfo.Invoke(tempStack, args);
                        }
                    }
                }

                private void ConvertStackImpl(ICollection tempStack, Type type, int count, ref ReadOnlySpan<byte> bytes)
                {
                    Type genericType = type.GenericTypeArguments[0];
                    
                    if (genericType.IsPrimitive)
                    {
                        switch (tempStack)
                        {
                            case Stack<int> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToInt32(ref bytes));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<float> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToFloat(ref bytes));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<bool> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToBoolean(ref bytes));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<long> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToInt64(ref bytes));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<double> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToDouble(ref bytes));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<short> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToInt16(ref bytes));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<ushort> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToUInt16(ref bytes));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<uint> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToUInt32(ref bytes));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<ulong> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(ByteConverter.ToUInt64(ref bytes));
                                }

                                newStack.Reverse();
                                break;
                            case Stack<byte> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push(bytes[i]);
                                }

                                bytes = bytes[count..];
                                newStack.Reverse();
                                break;
                            case Stack<sbyte> newStack:
                                for (int i = 0; i < count; i++)
                                {
                                    newStack.Push((sbyte)bytes[i]);
                                }
                                
                                bytes = bytes[count..];
                                newStack.Reverse();
                                break;
                        }
                    }
                    else if (tempStack is Stack<string> strStack)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            strStack.Push(ByteConverter.ToString(ref bytes));
                        }

                        strStack.Reverse();
                    }
                    else if (tempStack is Stack<TimeSpan> timeStack)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            timeStack.Push(new TimeSpan(ByteConverter.ToInt64(ref bytes)));
                        }

                        timeStack.Reverse();
                    }
                    else
                    {
                        MethodInfo methodInfo = SerializationCachePool.GetMethodInfo("Push", type, genericType);
                        object[] args = new object[1];
                        Stack stack = new Stack(count);

                        if (genericType.IsEnum)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                stack.Push(ByteConverter.ToInt32(ref bytes));
                            }
                        }
                        else
                        {
                            ISerializeConvert serializeConvert = ConverterFactory.Shared.CreateBuilder(genericType);
                            for (int i = 0; i < count; i++)
                            {
                                stack.Push(args[0] = serializeConvert.Convert(ref bytes, genericType));
                            }
                        }

                        while (stack.Count > 0)
                        {
                            args[0] = stack.Pop();
                            methodInfo.Invoke(tempStack, args);
                        }
                    }
                }
            }
        }
    }
}

