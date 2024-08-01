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
                        
                        ISerializeConvert serializeConvert = ConverterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                        memberConfig.SetValue(value, serializeConvert.Convert(bytes, ref nowIndex, memberConfig.ConfigType));
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
                        
                        ISerializeConvert serializeConvert = ConverterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                        memberConfig.SetValue(value, serializeConvert.Convert(ref bytes, memberConfig.ConfigType));
                    }
                    return value;
                }
            }
        }
    }
}

