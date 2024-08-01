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
                    
                    ISerializeAppend appender;
                    var memberConfigs = typeCache.MemberConfigs;

                    for (int i = 0; i < memberConfigs.Length; i++) 
                    {
                        var memberConfig = memberConfigs[i];
                        
                        var memberObject = memberConfig.GetValue(obj);
                        if (memberObject != null)
                        {
                            //写入当前字段写入的序号
                            ByteAppender.AppendInt32(memberConfig.OrdinalNumber, buffer, ref nowIndex);
                            appender = AppenderFactory.Shared.CreateBuilder(memberConfig.ConfigType);
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
}

