using System;
using System.IO;
using System.Threading.Tasks;
using ZincFramework.Binary;
using ZincFramework.Serialization.Cache;
using ZincFramework.Serialization.Factory;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct OtherValueWriter : ISerializeWrite
            {
                public void Write(object obj, Stream stream, Type type)
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
                                ByteWriter.WriteInt32(memberConfig.OrdinalNumber, stream);
                                WritePropertyPrimitive(obj, memberConfig, stream);
                            }
                            else if (memberConfig.ConfigType == typeof(string))
                            {
                                string str = (memberConfig as PropertyConfig<string>).GetPropertyValue(obj);

                                if (str != null)
                                {
                                    //写入当前字段写入的序号
                                    ByteWriter.WriteInt32(memberConfig.OrdinalNumber, stream);
                                    ByteWriter.WriteString(str, stream);
                                }
                                else
                                {
                                    //如果为空直接写入int的最小值
                                    ByteWriter.WriteInt32(int.MinValue, stream);
                                }
                            }
                            else
                            {
                                WriteOtherValue(obj, memberConfig, stream);
                            }
                        }
                        else
                        {
                            WriteOtherValue(obj, memberConfig, stream);
                        }
                    }
                }

                public async Task WriteAsync(object obj, Stream stream, Type type)
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
                                ByteWriter.WriteInt32(memberConfig.OrdinalNumber, stream);
                                await WritePropertyPrimitiveAsync(obj, memberConfig, stream);
                            }
                            else if (memberConfig.ConfigType == typeof(string))
                            {
                                string str = (memberConfig as PropertyConfig<string>).GetPropertyValue(obj);

                                if (str != null)
                                {
                                    //写入当前字段写入的序号
                                    ByteWriter.WriteInt32(memberConfig.OrdinalNumber, stream);
                                    await ByteWriter.WriteStringAsync(str, stream);
                                }
                                else
                                {
                                    //如果为空直接写入int的最小值
                                    ByteWriter.WriteInt32(int.MinValue, stream);
                                }
                            }
                            else
                            {
                                await WriteOtherValueAsync(obj, memberConfig, stream);
                            }
                        }
                        else
                        {
                            await WriteOtherValueAsync(obj, memberConfig, stream);
                        }
                    }
                }

                private void WritePropertyPrimitive(object serialObj, MemberConfig memberConfig, Stream stream)
                {
                    Type configType = memberConfig.ConfigType;

                    if (configType == typeof(int))
                    {
                        ByteWriter.WriteInt32((memberConfig as PropertyConfig<int>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(float))
                    {
                        ByteWriter.WriteFloat((memberConfig as PropertyConfig<float>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(bool))
                    {
                        ByteWriter.WriteBoolean((memberConfig as PropertyConfig<bool>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(long))
                    {
                        ByteWriter.WriteInt64((memberConfig as PropertyConfig<long>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(double))
                    {
                        ByteWriter.WriteDouble((memberConfig as PropertyConfig<double>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(short))
                    {
                        ByteWriter.WriteInt16((memberConfig as PropertyConfig<short>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(ushort))
                    {
                        ByteWriter.WriteUInt16((memberConfig as PropertyConfig<ushort>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(uint))
                    {
                        ByteWriter.WriteUInt32((memberConfig as PropertyConfig<uint>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(ulong))
                    {
                        ByteWriter.WriteUInt64((memberConfig as PropertyConfig<ulong>).GetPropertyValue(serialObj), stream);
                    }
                }        

                private void WriteOtherValue(object serialObj, MemberConfig memberConfig, Stream stream)
                {
                    var memberObject = memberConfig.GetValue(serialObj);

                    if (memberObject != null)
                    {
                        //写入当前字段写入的序号
                        ByteWriter.WriteInt32(memberConfig.OrdinalNumber, stream);
                        var writer = WriterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                        writer.Write(memberObject, stream, memberConfig.ConfigType);
                    }
                    else
                    {
                        //如果为空直接写入int的最小值
                        ByteWriter.WriteInt32(int.MinValue, stream);
                    }
                }

                private async Task WritePropertyPrimitiveAsync(object serialObj, MemberConfig memberConfig, Stream stream)
                {
                    Type configType = memberConfig.ConfigType;

                    if (configType == typeof(int))
                    {
                        await ByteWriter.WriteInt32Async((memberConfig as PropertyConfig<int>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(float))
                    {
                        await ByteWriter.WriteFloatAsync((memberConfig as PropertyConfig<float>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(bool))
                    {
                        await ByteWriter.WriteBooleanAsync((memberConfig as PropertyConfig<bool>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(long))
                    {
                        await ByteWriter.WriteInt64Async((memberConfig as PropertyConfig<long>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(double))
                    {
                        await ByteWriter.WriteDoubleAsync((memberConfig as PropertyConfig<double>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(short))
                    {
                        await ByteWriter.WriteInt16Async((memberConfig as PropertyConfig<short>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(ushort))
                    {
                        await ByteWriter.WriteUInt16Async((memberConfig as PropertyConfig<ushort>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(uint))
                    {
                        await ByteWriter.WriteUInt32Async((memberConfig as PropertyConfig<uint>).GetPropertyValue(serialObj), stream);
                    }
                    else if (configType == typeof(ulong))
                    {
                        await ByteWriter.WriteUInt64Async((memberConfig as PropertyConfig<ulong>).GetPropertyValue(serialObj), stream);
                    }
                }

                private async Task WriteOtherValueAsync(object serialObj, MemberConfig memberConfig, Stream stream)
                {
                    var memberObject = memberConfig.GetValue(serialObj);

                    if (memberObject != null)
                    {
                        //写入当前字段写入的序号
                        ByteWriter.WriteInt32(memberConfig.OrdinalNumber, stream);
                        var writer = WriterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                        await writer.WriteAsync(memberObject, stream, memberConfig.ConfigType);
                    }
                    else
                    {
                        //如果为空直接写入int的最小值
                        ByteWriter.WriteInt32(int.MinValue, stream);
                    }
                }
            }
        }
    }
}

