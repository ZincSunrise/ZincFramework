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
            public readonly struct ArrayListConverter : ISerializeConvert
            {
                public object Convert(byte[] bytes, ref int nowIndex, Type type)
                {
                    int count = ByteConverter.ToInt16(bytes, ref nowIndex);

                    IList list;
                    if (type.IsArray)
                    {
                        Type elementType = type.GetElementType();
                        list = Array.CreateInstance(elementType, count);
                        
                        ConvertArrayImpl(list, elementType, bytes, ref nowIndex);
                    }
                    else
                    {
                        var listConstructor = EnumerableConverter.GetCapacityConstructorMap(type);
                        list = listConstructor.Invoke(count) as IList;

                        ConvertListImpl(list, type.GenericTypeArguments[0], count, bytes, ref nowIndex);
                    }

                    return list;
                }

                public object Convert(ref ReadOnlySpan<byte> bytes, Type type)
                {
                    int count = ByteConverter.ToInt16(ref bytes);

                    IList list;
                    if (type.IsArray)
                    {
                        Type elementType = type.GetElementType();
                        list = Array.CreateInstance(elementType, count);
                        
                        ConvertArrayImpl(list, elementType, ref bytes);
                    }
                    else
                    {
                        var listConstructor = EnumerableConverter.GetCapacityConstructorMap(type);
                        list = listConstructor.Invoke(count) as IList;

                        ConvertListImpl(list, type.GenericTypeArguments[0], count, ref bytes);
                    }

                    return list;
                }
                
                
                
                private void ConvertArrayImpl(IList array, Type elementType, byte[] bytes, ref int startIndex)
                {
                    if (elementType.IsPrimitive)
                    {
                        switch (array)
                        {
                            case int[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToInt32(bytes, ref startIndex);
                                }
                                break;
                            case float[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToFloat(bytes, ref startIndex);
                                }
                                break;
                            case bool[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToBoolean(bytes, ref startIndex);
                                }
                                break;
                            case long[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToInt64(bytes, ref startIndex);
                                }
                                break;
                            case double[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToDouble(bytes, ref startIndex);
                                }
                                break;
                            case short[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToInt16(bytes, ref startIndex);
                                }
                                break;
                            case ushort[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToUInt16(bytes, ref startIndex);
                                }
                                break;
                            case uint[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToUInt32(bytes, ref startIndex);
                                }
                                break;
                            case ulong[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToUInt64(bytes, ref startIndex);
                                }
                                break;
                            case byte[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = bytes[startIndex++];
                                }
                                break;
                            case sbyte[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = (sbyte)bytes[startIndex++];
                                }
                                break;
                        }
                    }
                    else if (array is string[] value)
                    {
                        for (int i = 0; i < array.Count; i++)
                        {
                            value[i] = ByteConverter.ToString(bytes, ref startIndex);
                        }
                    }
                    else if (array is TimeSpan[] timeArray)
                    {
                        for (int i = 0; i < array.Count; i++)
                        {
                            timeArray[i] = new TimeSpan(ByteConverter.ToInt64(bytes, ref startIndex));
                        }
                    }
                    else if (elementType.IsEnum)
                    {
                        for (int i = 0; i < array.Count; i++)
                        {
                            array[i] = Enum.ToObject(elementType, ByteConverter.ToInt32(bytes, ref startIndex));
                        }
                    }
                    else
                    {
                        ISerializeConvert strategy = ConverterFactory.Shared.CreateBuilder(elementType);
                        for (int i = 0; i < array.Count; i++)
                        {
                            array[i] = strategy.Convert(bytes, ref startIndex, elementType);
                        }
                    }
                }
                
                private void ConvertListImpl(IList list, Type genericType, int count, byte[] bytes, ref int startIndex)
                {
                    if (genericType.IsPrimitive)
                    {
                        switch (list)
                        {
                            case List<int> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToInt32(bytes, ref startIndex));
                                }
                                break;
                            case List<float> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToFloat(bytes, ref startIndex));
                                }
                                break;
                            case List<bool> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToBoolean(bytes, ref startIndex));
                                }
                                break;
                            case List<long> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToInt64(bytes, ref startIndex));
                                }
                                break;
                            case List<double> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToDouble(bytes, ref startIndex));
                                }
                                break;
                            case List<short> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToInt16(bytes, ref startIndex));
                                }
                                break;
                            case List<ushort> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToUInt16(bytes, ref startIndex));
                                }
                                break;
                            case List<uint> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToUInt32(bytes, ref startIndex));
                                }
                                break;
                            case List<ulong> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToUInt64(bytes, ref startIndex));
                                }
                                break;
                            case List<byte> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(bytes[startIndex++]);
                                }
                                break;
                            case List<sbyte> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add((sbyte)bytes[startIndex++]);
                                }
                                break;
                        }
                    }
                    else if (list is List<string> strList)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            strList.Add(ByteConverter.ToString(bytes, ref startIndex));
                        }
                    }
                    else if (list is List<TimeSpan> timeList)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            timeList.Add(new TimeSpan(ByteConverter.ToInt64(bytes, ref startIndex)));
                        }
                    }
                    else if (genericType.IsEnum)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            list.Add(ByteConverter.ToInt32(bytes, ref startIndex));
                        }
                    }
                    else
                    {
                        ISerializeConvert strategy = ConverterFactory.Shared.CreateBuilder(genericType);
                        for (int i = 0; i < count; i++)
                        {
                            list.Add(strategy.Convert(bytes, ref startIndex, genericType));
                        }
                    }
                }
                
                
                private void ConvertArrayImpl(IList array, Type elementType, ref ReadOnlySpan<byte> bytes)
                {
                    if (elementType.IsPrimitive)
                    {
                        switch (array)
                        {
                            case int[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToInt32(ref bytes);
                                }
                                break;
                            case float[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToFloat(ref bytes);
                                }
                                break;
                            case bool[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToBoolean(ref bytes);
                                }
                                break;
                            case long[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToInt64(ref bytes);
                                }
                                break;
                            case double[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToDouble(ref bytes);
                                }
                                break;
                            case short[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToInt16(ref bytes);
                                }
                                break;
                            case ushort[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToUInt16(ref bytes);
                                }
                                break;
                            case uint[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToUInt32(ref bytes);
                                }
                                break;
                            case ulong[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = ByteConverter.ToUInt64(ref bytes);
                                }
                                break;
                            case byte[] value:
                                bytes.CopyTo(value);
                                bytes = bytes[array.Count..];
                                break;
                            case sbyte[] value:
                                for (int i = 0; i < array.Count; i++)
                                {
                                    value[i] = (sbyte)bytes[i];
                                }
                                bytes = bytes[array.Count..];
                                break;
                        }
                    }
                    else if (array is string[] value)
                    {
                        for (int i = 0; i < array.Count; i++)
                        {
                            value[i] = ByteConverter.ToString(ref bytes);
                        }
                    }
                    else if (array is TimeSpan[] timeArray)
                    {
                        for (int i = 0; i < array.Count; i++)
                        {
                            timeArray[i] = new TimeSpan(ByteConverter.ToInt64(ref bytes));
                        }
                    }
                    else if (elementType.IsEnum)
                    {
                        for (int i = 0; i < array.Count; i++)
                        {
                            array[i] = Enum.ToObject(elementType, ByteConverter.ToInt32(ref bytes));
                        }
                    }
                    else
                    {
                        ISerializeConvert strategy = ConverterFactory.Shared.CreateBuilder(elementType);
                        for (int i = 0; i < array.Count; i++)
                        {
                            array[i] = strategy.Convert(ref bytes, elementType);
                        }
                    }
                }
                
                private void ConvertListImpl(IList list, Type genericType, int count, ref ReadOnlySpan<byte> bytes)
                {
                    if (genericType.IsPrimitive)
                    {
                        switch (list)
                        {
                            case List<int> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToInt32(ref bytes));
                                }
                                break;
                            case List<float> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToFloat(ref bytes));
                                }
                                break;
                            case List<bool> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToBoolean(ref bytes));
                                }
                                break;
                            case List<long> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToInt64(ref bytes));
                                }
                                break;
                            case List<double> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToDouble(ref bytes));
                                }
                                break;
                            case List<short> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToInt16(ref bytes));
                                }
                                break;
                            case List<ushort> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToUInt16(ref bytes));
                                }
                                break;
                            case List<uint> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToUInt32(ref bytes));
                                }
                                break;
                            case List<ulong> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(ByteConverter.ToUInt64(ref bytes));
                                }
                                break;
                            case List<byte> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add(bytes[i]);
                                }
                                
                                bytes = bytes[count..];
                                break;
                            case List<sbyte> newList:
                                for (int i = 0; i < count; i++)
                                {
                                    newList.Add((sbyte)bytes[i]);
                                }
                                
                                bytes = bytes[count..];
                                break;
                        }
                    }
                    else if (list is List<string> strList)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            strList.Add(ByteConverter.ToString(ref bytes));
                        }
                    }
                    else if (list is List<TimeSpan> timeList)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            timeList.Add(new TimeSpan(ByteConverter.ToInt64(ref bytes)));
                        }
                    }
                    else if (genericType.IsEnum)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            list.Add(ByteConverter.ToInt32(ref bytes));
                        }
                    }
                    else
                    {
                        ISerializeConvert strategy = ConverterFactory.Shared.CreateBuilder(genericType);
                        for (int i = 0; i < count; i++)
                        {
                            list.Add(strategy.Convert(ref bytes, genericType));
                        }
                    }
                }
            }
        }
    }
}
