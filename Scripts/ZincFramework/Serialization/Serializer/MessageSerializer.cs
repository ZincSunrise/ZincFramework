using System;
using UnityEngine;
using ZincFramework.Binary;
using ZincFramework.Serialization.Cache;
using ZincFramework.Network.Protocol;
using ZincFramework.Serialization.Exceptions;
using ZincFramework.Serialization.Factory;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Network
        {
            public class MessageSerializer : MonoBehaviour
            {
                public static byte[] Serialize(BaseMessage baseMessage, SerializeConfig serializeConfig = SerializeConfig.Property)
                {
                    return SerializeInternal(baseMessage, baseMessage.SerializableCode, serializeConfig);
                }

                public static ISerializable Deserialize(byte[] buffer, int offset = 0, SerializeConfig serializeConfig = SerializeConfig.Property)
                {
                    int nowIndex = offset;
                    HeadInfo headInfo = HeadInfo.ConvertHeadInfo(buffer, ref nowIndex);
                    TypeCache typeCache = SerializationCachePool.GetTypeCache(headInfo.SerializeCode);
                    object obj = typeCache.CreateInstance();

                    DeserializeInternal(obj, buffer, nowIndex, typeCache, serializeConfig);
                    return obj as BaseMessage;
                }

                public static T Deserialize<T>(byte[] buffer, int offset = 0, SerializeConfig serializeConfig = SerializeConfig.Property) where T : BaseMessage
                {
                    return Deserialize(buffer, offset, serializeConfig) as T;
                }

                public static void Deserialize(BaseMessage baseMessage, byte[] buffer, int offset = 0, SerializeConfig serializeConfig = SerializeConfig.Property)
                {
                    int nowIndex = offset + HeadInfo.HeadLength;
                    TypeCache typeCache = SerializationCachePool.GetTypeCache(baseMessage.SerializableCode);
                    DeserializeInternal(baseMessage, buffer, nowIndex, typeCache, serializeConfig);
                }



                private static void DeserializeInternal(object obj, byte[] buffer, int offset, TypeCache typeCache, SerializeConfig serializeConfig)
                {
                    int nowIndex = offset;
                    MemberConfig[] memberConfigs = typeCache.MemberConfigs;

     
                    for (int i = 0; i < memberConfigs.Length; i++)
                    {
                        var ordinalNumber = ByteConverter.ToInt32(buffer, ref nowIndex);
                        if (ordinalNumber == int.MinValue)
                        {
                            continue;
                        }

                        var memberConfig = Array.Find(memberConfigs, (field) => field.OrdinalNumber == ordinalNumber) ?? throw new ObsoleteChangedException(ordinalNumber);
                        var converter = ConverterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                        memberConfig.SetValue(obj, converter.Convert(buffer, ref nowIndex, memberConfig.ConfigType));
                    }
                }

                private static byte[] SerializeInternal(object obj, int serializabelCode, SerializeConfig serializeConfig)
                {
                    TypeCache typeCache = SerializationCachePool.GetTypeCache(serializabelCode);
                    int nowIndex = 0;

                    //µÃµ½type
                    Span<byte> bytes = stackalloc byte[HeadInfo.HeadLength];
                    HeadInfo headInfo = new HeadInfo(typeCache.SerializableCode, TypeMeasurer.GetTypeLength(obj, typeCache.CacheType, serializeConfig));

                    headInfo.AppendHeadInfo(bytes, ref nowIndex);
                    int length = headInfo.Length;

                    byte[] buffer = new byte[length + HeadInfo.HeadLength];
                    bytes.CopyTo(buffer);

                    var appender = AppenderFactory.Shared.CreateBuilder(E_Builder_Type.OtherValue);
                    appender.Append(obj, typeCache.CacheType, buffer, ref nowIndex);
                    return buffer;
                }
            }
        }
    }
}

