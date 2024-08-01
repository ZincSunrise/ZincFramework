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
            public static class BinarySerializer
            {
                public static byte[] Serialize(object obj, SerializeConfig serializeConfig)
                {
                    return SerializeInternal(obj, obj.GetType(), serializeConfig);
                }

                public static byte[] Serialize(object obj, Type type, SerializeConfig serializeConfig)
                {
                    return SerializeInternal(obj, type, serializeConfig);
                }

                public static object Deserialize(byte[] buffer, Type type, SerializeConfig serializeConfig)
                {
                    return DeserializeInternal(buffer, type, 0, serializeConfig);
                }

                public static object Deserialize(byte[] buffer, Type type, int offset, SerializeConfig serializeConfig)
                {
                    return DeserializeInternal(buffer, type, offset, serializeConfig);
                }

                public static T Deserialize<T>(byte[] buffer, SerializeConfig serializeConfig)
                {
                    return (T)DeserializeInternal(buffer, typeof(T), 0, serializeConfig);
                }

                public static T Deserialize<T>(byte[] buffer, int offset, SerializeConfig serializeConfig)
                {
                    return (T)DeserializeInternal(buffer, typeof(T), offset, serializeConfig);
                }

                public static void Deserialize(ISerializable serializable, Type type, byte[] buffer, SerializeConfig serializeConfig, int offset = 0)
                {
                    DeserializeInternal(serializable, buffer, type, offset, serializeConfig);
                }




                private static void DeserializeInternal(ISerializable serializable, byte[] buffer, Type type, int offset, SerializeConfig serializeConfig)
                {
                    int nowIndex = offset;
                    if (!SerializationCachePool.TryGetTypeCache(type, out var typeCache))
                    {
                        typeCache = SerializationCachePool.CreateTypeCache(type, serializeConfig);
                    }

                    MemberConfig[] memberConfigs = typeCache.MemberConfigs;
                    for (int i = 0; i < memberConfigs.Length; i++)
                    {
                        var ordinalNumber = ByteConverter.ToInt32(buffer, ref nowIndex);

                        if (ordinalNumber == int.MinValue)
                        {
                            continue;
                        }

                        var memberConfig = Array.Find(memberConfigs, (field) => field.OrdinalNumber == ordinalNumber) ?? throw new ObsoleteChangedException(ordinalNumber);
                        var strategy = ConverterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                        memberConfig.SetValue(serializable, strategy.Convert(buffer, ref nowIndex, memberConfig.ConfigType));
                    }
                }

                private static object DeserializeInternal(byte[] buffer, Type type, int offset, SerializeConfig serializeConfig)
                {                   
                    if (!SerializationCachePool.TryGetTypeCache(type, out var typeCache))
                    {
                        typeCache = SerializationCachePool.CreateTypeCache(type, serializeConfig);
                    }
                    
                    int nowIndex = offset;
                    var converter = ConverterFactory.Shared.CreateBuilder(typeCache.CacheType);  
                    return converter.Convert(buffer, ref nowIndex, type);
                }

                
                private static byte[] SerializeInternal(object obj, Type type, SerializeConfig serializeConfig)
                {
                    if (!SerializationCachePool.TryGetTypeCache(type, out var typeCache))
                    {
                        typeCache = SerializationCachePool.CreateTypeCache(type, serializeConfig);
                    }

                    int nowIndex = 0;
                    //�õ�type

                    int length = TypeMeasurer.GetTypeLength(obj, type, serializeConfig);
                    byte[] buffer = new byte[length];

                    ISerializeAppend appender = AppenderFactory.Shared.CreateBuilder(typeCache.CacheType);
                    appender.Append(obj, type, buffer, ref nowIndex);
                    return buffer;
                }
            }
        }
    }
}
