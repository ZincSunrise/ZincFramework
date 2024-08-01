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
                        var memberObject = memberConfig.GetValue(obj);
                        
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
                        var memberObject = memberConfig.GetValue(obj);
                        if (memberObject != null)
                        {
                            //写入当前字段写入的序号
                            await ByteWriter.WriteInt32Async(memberConfig.OrdinalNumber, stream);
                            var writer = WriterFactory.Shared.CreateBuilder(memberConfig.ConfigType);
                            await writer.WriteAsync(memberObject, stream, memberConfig.ConfigType);
                        }
                        else
                        {
                            //如果为空直接写入int的最小值
                            await ByteWriter.WriteInt32Async(int.MinValue, stream);
                        }
                    }
                }
            }
        }
    }
}

