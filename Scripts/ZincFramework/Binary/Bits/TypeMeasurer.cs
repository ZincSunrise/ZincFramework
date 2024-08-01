using System;
using System.Collections;
using ZincFramework.Serialization;
using ZincFramework.Serialization.Cache;

namespace ZincFramework
{
    namespace Binary
    {
        public static class TypeMeasurer
        {
            public static int GetTypeLength(object obj, Type type, SerializeConfig serializeConfig = SerializeConfig.Property)
            {
                if (!SerializationCachePool.TryGetTypeCache(type, out var typeCache))
                {
                    typeCache = SerializationCachePool.CreateTypeCache(type, serializeConfig);
                }

                MemberConfig[] memberConfigs = typeCache.MemberConfigs;

                MemberConfig memberConfig;
                int length = 0;
                object memberObject;

                for (int i = 0; i < memberConfigs.Length; i++)
                {
                    memberConfig = memberConfigs[i];
                    memberObject = memberConfig.GetValue(obj);

                    length += 4;
                    if (memberObject != null)
                    {
                        length += GetMemberLength(memberObject, memberConfig.ConfigType, serializeConfig);
                    }
                }
                return length;
            }

            public static int GetMemberLength(object memberObject, Type type, SerializeConfig serializeConfig)
            {
                if (type.IsPrimitive)
                {
                    return ByteUtility.GetPrimitiveLength(memberObject);
                }
                else if (memberObject is string str)
                {
                    return ByteUtility.GetStringLength(str);
                }
                if (memberObject is Enum)
                {
                    return 4;
                }
                //对字典进行长度测量
                else if (memberObject is IDictionary dictionary)
                {
                    return GetDictionaryLength(dictionary, type, serializeConfig);
                }
                //对普通数据集合进行长度测量
                else if (memberObject is ICollection collection)
                {
                    return GetCollectionLength(collection, type, serializeConfig);
                }
                //对哈希集合进行长度测量
                else if (type.IsGenericType && memberObject is IEnumerable set)
                {
                    return GetHashSetLength(set, type, serializeConfig);
                }
                else
                {
                    return GetTypeLength(memberObject, type, serializeConfig);
                }
            }

            private static int GetDictionaryLength(IDictionary dictionary, Type dictionaryType, SerializeConfig serializeConfig)
            {
                Type keyType = dictionaryType.GenericTypeArguments[0];
                Type valueType = dictionaryType.GenericTypeArguments[1];
                int length = 2;

                if (keyType.IsPrimitive && valueType.IsPrimitive)
                {
                    return (ByteUtility.GetPrimitiveLength(keyType) + ByteUtility.GetPrimitiveLength(valueType)) * dictionary.Count + length;
                }

                if (!keyType.IsPrimitive && valueType.IsPrimitive)
                {
                    foreach (object key in dictionary.Keys)
                    {
                        length += GetMemberLength(key, keyType, serializeConfig);
                    }

                    return ByteUtility.GetPrimitiveLength(valueType) * dictionary.Count + length;
                }

                if (keyType.IsPrimitive && !valueType.IsPrimitive)
                {
                    foreach (object value in dictionary.Values)
                    {
                        length += GetMemberLength(value, valueType, serializeConfig);
                    }

                    return ByteUtility.GetPrimitiveLength(keyType) * dictionary.Count + length;
                }

                foreach (object key in dictionary.Keys)
                {
                    length += GetMemberLength(dictionary[key], valueType, serializeConfig) + GetMemberLength(key, keyType, serializeConfig);
                }

                return length;
            }

            private static int GetCollectionLength(ICollection collection, Type collectionType, SerializeConfig serializeConfig)
            {
                int length = 2;
                Type genericType = collectionType.IsArray ? collectionType.GetElementType() : collectionType.GenericTypeArguments[0];

                if (genericType.IsPrimitive)
                {
                    return ByteUtility.GetPrimitiveLength(genericType) * collection.Count + length;
                }

                if(collection is IList list)
                {
                    for (int i = 0;i < list.Count; i++)
                    {
                        length += GetMemberLength(list[i], genericType, serializeConfig);
                    }
                }
                else
                {
                    foreach (object obj in collection)
                    {
                        length += GetMemberLength(obj, genericType, serializeConfig);
                    }
                }
                return length;
            }

            private static int GetHashSetLength(IEnumerable set, Type hashSetType, SerializeConfig serializeConfig)
            {
                int length = 2;
                Type genericType = hashSetType.GenericTypeArguments[0];

                foreach (object value in set)
                {
                    length += GetMemberLength(value, genericType, serializeConfig);
                }
                return length;
            }
        }
    }
}
