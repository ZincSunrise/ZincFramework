using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ZincFramework.Binary.Serialization.Metadata;

namespace ZincFramework.Binary.Serialization
{
    public partial class SerializerOption
    {
        private sealed class SerializeContext
        {
            public int Hashcode { get; set; }

            public SerializerOption SerializerOption { get; set; }


            private readonly ConcurrentDictionary<Type, BinaryTypeInfo> _cacheMap = new ConcurrentDictionary<Type, BinaryTypeInfo>();

            public SerializeContext(SerializerOption serializerOption)
            {
                SerializerOption = serializerOption;
                Hashcode = serializerOption.GetHashCode();
            }

            public bool TryGetCacheInfo(Type type, out BinaryTypeInfo binaryTypeInfo)
            {
                return _cacheMap.TryGetValue(type, out binaryTypeInfo);
            }

            public BinaryTypeInfo GetOrAddCacheInfo(Type type, SerializerOption serializerOption)
            {
                if (!_cacheMap.TryGetValue(type, out var typeInfo))
                {
                    typeInfo = serializerOption.CreateTypeInfo(type);
                    _cacheMap.TryAdd(type, typeInfo);
                }

                return typeInfo;
            }

            public BinaryTypeInfo GetOrAddCacheInfo<T>(SerializerOption serializerOption)
            {
                Type type = typeof(T);
                if(!_cacheMap.TryGetValue(type, out var typeInfo))
                {
                    typeInfo = serializerOption.CreateTypeInfo<T>();
                    _cacheMap.TryAdd(type, typeInfo);
                }

                return typeInfo;
            }

            public BinaryTypeInfo GetOrAddCacheInfo<T>(Func<T> factory, SerializerOption serializerOption)
            {
                Type type = typeof(T);
                if (!_cacheMap.TryGetValue(typeof(T), out var typeInfo))
                {
                    typeInfo = serializerOption.CreateTypeInfo<T>(factory);
                    _cacheMap.TryAdd(type, typeInfo);
                }

                return typeInfo;
            }
        }

        private static class ContextPool
        {
            private readonly static List<SerializeContext> _cacheInfos = new List<SerializeContext>();

            public static SerializeContext GetOrAddCache(SerializerOption serializerOption)
            {
                for (int i = 0; i < _cacheInfos.Count; i++)
                {
                    if (_cacheInfos[i].Hashcode == serializerOption.GetHashCode() && Equals(_cacheInfos[i].SerializerOption, serializerOption))
                    {
                        return _cacheInfos[i];
                    }
                }

                var info = new SerializeContext(serializerOption);

                _cacheInfos.Add(info);
                return info;
            }
        }

        public SerializerOption()
        {
            _serializeCacheInfo = ContextPool.GetOrAddCache(this);
        }


        private readonly SerializeContext _serializeCacheInfo;
    }
}


