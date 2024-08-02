using System;
using ZincFramework.Binary;
using ZincFramework.Serialization.Cache;
using ZincFramework.Serialization.Exceptions;
using ZincFramework.Serialization.Factory;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct OtherValueConverter : ISerializeConvert
            {
                public object Convert(byte[] bytes, ref int nowIndex, Type type)
                {
                    if (!SerializationCachePool.TryGetTypeCache(type, out var typeCache))
                    {
                        typeCache = SerializationCachePool.CreateTypeCache(type, SerializeConfig.Property);
                    }

                    var value = typeCache.CreateInstance();
                    var memberConfigs = typeCache.MemberConfigs;

                    for (int i = 0; i < memberConfigs.Length; i++)
                    {
                        int ordinalNumber = ByteConverter.ToInt32(bytes, ref nowIndex);
                        //对象为空反序列化不进行
                        if (ordinalNumber == int.MinValue)
                        {
                            continue;
                        }
                        
                        var memberConfig = Array.Find(memberConfigs, (field) => field.OrdinalNumber == ordinalNumber) ?? throw new ObsoleteChangedException(ordinalNumber);

                        if (typeCache.SerializeConfig == SerializeConfig.Property || typeCache.SerializeConfig == SerializeConfig.AllProperty)
                        {
                            if (memberConfig.ConfigType.IsPrimitive)
                            {
                                SetPropertyValue(memberConfig, value, bytes, ref nowIndex);
                            }
                            else if (memberConfig.ConfigType == typeof(string))
                            {
                                (memberConfig as PropertyConfig<string>).SetPropertyValue(value, ByteConverter.ToString(bytes, ref nowIndex));
                            }
                            else
                            {
                                var serializeConvert = ConverterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                                memberConfig.SetValue(value, serializeConvert.Convert(bytes, ref nowIndex, memberConfig.ConfigType));
                            }
                        }
                        else
                        {
                            var serializeConvert = ConverterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                            memberConfig.SetValue(value, serializeConvert.Convert(bytes, ref nowIndex, memberConfig.ConfigType));
                        }
                    }
                    return value;
                }

                public object Convert(ref ReadOnlySpan<byte> bytes, Type type)
                {
                    if (!SerializationCachePool.TryGetTypeCache(type, out var typeCache))
                    {
                        typeCache = SerializationCachePool.CreateTypeCache(type, SerializeConfig.Property);
                    }

                    var value = typeCache.CreateInstance();
                    var memberConfigs = typeCache.MemberConfigs;

                    for (int i = 0; i < memberConfigs.Length; i++)
                    {
                        int ordinalNumber = ByteConverter.ToInt32(ref bytes);
                        //对象为空反序列化不进行
                        if (ordinalNumber == int.MinValue)
                        {
                            continue;
                        }

                        var memberConfig = Array.Find(memberConfigs, (field) => field.OrdinalNumber == ordinalNumber) ?? throw new ObsoleteChangedException(ordinalNumber);

                        if (typeCache.SerializeConfig == SerializeConfig.Property || typeCache.SerializeConfig == SerializeConfig.AllProperty)
                        {
                            if (typeCache.CacheType.IsPrimitive) 
                            {
                                SetPropertyValue(memberConfig, value, ref bytes);
                            }
                            else if (typeCache.CacheType == typeof(string))
                            {
                                (memberConfig as PropertyConfig<string>).SetPropertyValue(value, ByteConverter.ToString(ref bytes));
                            }
                            else
                            {
                                var serializeConvert = ConverterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                                memberConfig.SetValue(value, serializeConvert.Convert(ref bytes, memberConfig.ConfigType));
                            }
                        }
                        else
                        {
                            var serializeConvert = ConverterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                            memberConfig.SetValue(value, serializeConvert.Convert(ref bytes, memberConfig.ConfigType));
                        }
                    }
                    return value;
                }

                private void SetPropertyValue(MemberConfig memberConfig, object serialObj, ref ReadOnlySpan<byte> bytes)
                {
                    Type configType = memberConfig.ConfigType;

                    if (configType == typeof(int))
                    {
                        (memberConfig as PropertyConfig<int>).SetPropertyValue(serialObj, ByteConverter.ToInt32(ref bytes));
                        
                    }
                    else if (configType == typeof(float))
                    {
                        (memberConfig as PropertyConfig<float>).SetPropertyValue(serialObj, ByteConverter.ToFloat(ref bytes));    
                    }
                    else if (configType == typeof(bool))
                    {
                        (memberConfig as PropertyConfig<bool>).SetPropertyValue(serialObj, ByteConverter.ToBoolean(ref bytes));
                    }
                    else if (configType == typeof(long))
                    {
                        (memberConfig as PropertyConfig<long>).SetPropertyValue(serialObj, ByteConverter.ToInt64(ref bytes));
                    }
                    else if (configType == typeof(double))
                    {
                        (memberConfig as PropertyConfig<double>).SetPropertyValue(serialObj, ByteConverter.ToDouble(ref bytes));
                    }
                    else if (configType == typeof(short))
                    {
                        (memberConfig as PropertyConfig<short>).SetPropertyValue(serialObj, ByteConverter.ToInt16(ref bytes));
                    }
                    else if (configType == typeof(ushort))
                    {
                        (memberConfig as PropertyConfig<ushort>).SetPropertyValue(serialObj, ByteConverter.ToUInt16(ref bytes));
                    }
                    else if (configType == typeof(uint))
                    {
                        (memberConfig as PropertyConfig<uint>).SetPropertyValue(serialObj, ByteConverter.ToUInt32(ref bytes));
                    }
                    else if (configType == typeof(ulong))
                    {
                        (memberConfig as PropertyConfig<ulong>).SetPropertyValue(serialObj, ByteConverter.ToUInt64(ref bytes));
                    }
                }

                private void SetPropertyValue(MemberConfig memberConfig, object serialObj, byte[] bytes, ref int nowIndex)
                {
                    Type configType = memberConfig.ConfigType;

                    if (configType == typeof(int))
                    {
                        (memberConfig as PropertyConfig<int>).SetPropertyValue(serialObj, ByteConverter.ToInt32(bytes, ref nowIndex));

                    }
                    else if (configType == typeof(float))
                    {
                        (memberConfig as PropertyConfig<float>).SetPropertyValue(serialObj, ByteConverter.ToFloat(bytes, ref nowIndex));
                    }
                    else if (configType == typeof(bool))
                    {
                        (memberConfig as PropertyConfig<bool>).SetPropertyValue(serialObj, ByteConverter.ToBoolean(bytes, ref nowIndex));
                    }
                    else if (configType == typeof(long))
                    {
                        (memberConfig as PropertyConfig<long>).SetPropertyValue(serialObj, ByteConverter.ToInt64(bytes, ref nowIndex));
                    }
                    else if (configType == typeof(double))
                    {
                        (memberConfig as PropertyConfig<double>).SetPropertyValue(serialObj, ByteConverter.ToDouble(bytes, ref nowIndex));
                    }
                    else if (configType == typeof(short))
                    {
                        (memberConfig as PropertyConfig<short>).SetPropertyValue(serialObj, ByteConverter.ToInt16(bytes, ref nowIndex));
                    }
                    else if (configType == typeof(ushort))
                    {
                        (memberConfig as PropertyConfig<ushort>).SetPropertyValue(serialObj, ByteConverter.ToUInt16(bytes, ref nowIndex));
                    }
                    else if (configType == typeof(uint))
                    {
                        (memberConfig as PropertyConfig<uint>).SetPropertyValue(serialObj, ByteConverter.ToUInt32(bytes, ref nowIndex));
                    }
                    else if (configType == typeof(ulong))
                    {
                        (memberConfig as PropertyConfig<ulong>).SetPropertyValue(serialObj, ByteConverter.ToUInt64(bytes, ref nowIndex));
                    }
                }
            }
        }
    }
}

