using System;
using System.Collections.Generic;
using System.Reflection;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Cache
        {
            public static class SerializationCachePool
            {
                public readonly static Assembly SystemAssembly;

                private static readonly Dictionary<Type, TypeCache> _typeCacheMap = new Dictionary<Type, TypeCache>();

                private static readonly Dictionary<int, TypeCache> _serializeCodeMap = new Dictionary<int, TypeCache>();

                private static readonly Dictionary<Type, MethodInfo> _addMethodPool = new Dictionary<Type, MethodInfo>();

                private readonly static Type[] _oneTypeArray = new Type[1];

                static SerializationCachePool()
                {
                    SystemAssembly = typeof(int).Assembly;
                }

                internal static TypeCache GetTypeCache(int serializableCode)
                {
                    if (!_serializeCodeMap.TryGetValue(serializableCode, out TypeCache typeCache))
                    {
                        throw new ArgumentException("不存在这样的类!");
                    }
                    return typeCache;
                }


                internal static bool TryGetTypeCache(Type type, out TypeCache typeCache)
                {
                    return _typeCacheMap.TryGetValue(type, out typeCache);
                }


                internal static TypeCache CreateTypeCache(Type type, SerializeConfig serializeConfig)
                {
                    TypeCache typeCache = new TypeCache(type, serializeConfig);

                    _typeCacheMap.Add(type, typeCache);
                    _serializeCodeMap.Add(typeCache.SerializableCode, typeCache);
                    return typeCache;
                }


                public static MethodInfo GetMethodInfo(string name, Type type)
                {
                    if (!_addMethodPool.TryGetValue(type, out MethodInfo methodInfo))
                    {
                        methodInfo = type.GetMethod(name);
                        _addMethodPool.Add(type, methodInfo);
                    }

                    return methodInfo;
                }

                public static MethodInfo GetMethodInfo(string name, Type type, Type genericType)
                {
                    if (!_addMethodPool.TryGetValue(type, out MethodInfo methodInfo))
                    {
                        _oneTypeArray[0] = genericType;
                        methodInfo = type.GetMethod(name, _oneTypeArray);
                        if (methodInfo == null)
                        {
                            return null;
                        }
                        _addMethodPool.Add(type, methodInfo);
                    }
                    return methodInfo;
                }
            }
        }
    }
}
