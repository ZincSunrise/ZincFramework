using System;
using ZincFramework.Binary;
using ZincFramework.Serialization.Cache;
using ZincFramework.Serialization.Factory;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct OtherValueAppender : ISerializeAppend
            {
                public void Append(object obj, Type type, byte[] buffer, ref int nowIndex)
                {
                    Append(obj, type, buffer.AsSpan(), ref nowIndex);
                }

                public void Append(object obj, Type type, Span<byte> buffer, ref int nowIndex)
                {
                    if (!SerializationCachePool.TryGetTypeCache(type, out var typeCache))
                    {
                        typeCache = SerializationCachePool.CreateTypeCache(type, SerializeConfig.Property);
                    }
                    
                    var memberConfigs = typeCache.MemberConfigs;

                    for (int i = 0; i < memberConfigs.Length; i++) 
                    {
                        var memberConfig = memberConfigs[i];

                        if (typeCache.SerializeConfig == SerializeConfig.Property || typeCache.SerializeConfig == SerializeConfig.AllProperty)
                        {
                            if (memberConfig.ConfigType.IsPrimitive)
                            {
                                ByteAppender.AppendInt32(memberConfig.OrdinalNumber, buffer, ref nowIndex);
                                AppendPropertyPrimitive(obj, memberConfig, buffer, ref nowIndex);
                            }
                            else if (memberConfig.ConfigType == typeof(string))
                            {
                                string str = (memberConfig as PropertyConfig<string>).GetPropertyValue(obj);
                                if (str != null)
                                {
                                    //写入当前字段写入的序号
                                    ByteAppender.AppendInt32(memberConfig.OrdinalNumber, buffer, ref nowIndex);
                                    ByteAppender.AppendString(str, buffer, ref nowIndex);
                                }
                                else
                                {
                                    //如果为空直接写入int的最小值
                                    ByteAppender.AppendInt32(int.MinValue, buffer, ref nowIndex);
                                }
                            }
                            else
                            {
                                AppendOtherValue(obj, memberConfig, buffer, ref nowIndex);
                            }
                        }
                        else 
                        {
                            AppendOtherValue(obj, memberConfig, buffer, ref nowIndex);
                        }
                    }
                }


                private void AppendPropertyPrimitive(object serialObj, MemberConfig memberConfig, Span<byte> buffer, ref int nowIndex)
                {
                    Type configType = memberConfig.ConfigType;

                    if (configType == typeof(int))
                    {
                        ByteAppender.AppendInt32((memberConfig as PropertyConfig<int>).GetPropertyValue(serialObj), buffer, ref nowIndex);
                    }
                    else if (configType == typeof(float))
                    {
                        ByteAppender.AppendFloat((memberConfig as PropertyConfig<float>).GetPropertyValue(serialObj), buffer, ref nowIndex);
                    }
                    else if (configType == typeof(bool))
                    {
                        ByteAppender.AppendBoolean((memberConfig as PropertyConfig<bool>).GetPropertyValue(serialObj), buffer, ref nowIndex);
                    }
                    else if (configType == typeof(long))
                    {
                        ByteAppender.AppendInt64((memberConfig as PropertyConfig<long>).GetPropertyValue(serialObj), buffer, ref nowIndex);
                    }
                    else if (configType == typeof(double))
                    {
                        ByteAppender.AppendDouble((memberConfig as PropertyConfig<double>).GetPropertyValue(serialObj), buffer, ref nowIndex);
                    }
                    else if (configType == typeof(short))
                    {
                        ByteAppender.AppendInt16((memberConfig as PropertyConfig<short>).GetPropertyValue(serialObj), buffer, ref nowIndex);
                    }
                    else if (configType == typeof(ushort))
                    {
                        ByteAppender.AppendUInt16((memberConfig as PropertyConfig<ushort>).GetPropertyValue(serialObj), buffer, ref nowIndex);
                    }
                    else if (configType == typeof(uint))
                    {
                        ByteAppender.AppendUInt32((memberConfig as PropertyConfig<uint>).GetPropertyValue(serialObj), buffer, ref nowIndex);
                    }
                    else if (configType == typeof(ulong))
                    {
                        ByteAppender.AppendUInt64((memberConfig as PropertyConfig<ulong>).GetPropertyValue(serialObj), buffer, ref nowIndex);
                    }
                }

                private void AppendOtherValue(object serialObj, MemberConfig memberConfig, Span<byte> buffer, ref int nowIndex)
                {
                    var memberObject = memberConfig.GetValue(serialObj);

                    if (memberObject != null)
                    {
                        //写入当前字段写入的序号
                        ByteAppender.AppendInt32(memberConfig.OrdinalNumber, buffer, ref nowIndex);
                        var appender = AppenderFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                        appender.Append(memberObject, memberConfig.ConfigType, buffer, ref nowIndex);
                    }
                    else
                    {
                        //如果为空直接写入int的最小值
                        ByteAppender.AppendInt32(int.MinValue, buffer, ref nowIndex);
                    }
                }
            }
        }
    }
}

