using System;
using UnityEngine;
using System.Collections;
using ZincFramework.Binary.Serialization;
using ZincFramework.Binary.Serialization.Metadata;



namespace ZincFramework
{
    namespace Binary
    {
        public static class TypeMeasurer
        {
            public static int GetTypeLength(object obj, Type type, SerializerOption serializerOption = null)
            {
                serializerOption ??= SerializerOption.Default;
                BinaryTypeInfo binaryTypeInfo = serializerOption.GetTypeInfo(type);

                var binaryMemberInfos = binaryTypeInfo.MemberInfos;
                int length = 0;

                foreach(var member in binaryMemberInfos.Values)
                {
                    object memberObject = member.GetAction.Invoke(obj);

                    length += 4;
                    if (memberObject != null)
                    {
                        length += GetMemberLength(memberObject, member.MemberType, serializerOption);
                    }
                }

                return length;
            }

            public static int GetMemberLength(object memberObject, Type type, SerializerOption serializerOption)
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
                else if (memberObject is Color)
                {
                    return 16;
                }
                //对字典进行长度测量
                else if (memberObject is IDictionary dictionary)
                {
                    return GetDictionaryLength(dictionary, type, serializerOption);
                }
                //对普通数据集合进行长度测量
                else if (memberObject is ICollection collection)
                {
                    return GetCollectionLength(collection, type, serializerOption);
                }
                //对哈希集合进行长度测量
                else if (type.IsGenericType && memberObject is IEnumerable set)
                {
                    return GetHashSetLength(set, type, serializerOption);
                }
                else
                {
                    return GetTypeLength(memberObject, type, serializerOption);
                }
            }

            private static int GetDictionaryLength(IDictionary dictionary, Type dictionaryType, SerializerOption serializerOption)
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
                        length += GetMemberLength(key, keyType, serializerOption);
                    }

                    return ByteUtility.GetPrimitiveLength(valueType) * dictionary.Count + length;
                }

                if (keyType.IsPrimitive && !valueType.IsPrimitive)
                {
                    foreach (object value in dictionary.Values)
                    {
                        length += GetMemberLength(value, valueType, serializerOption);
                    }

                    return ByteUtility.GetPrimitiveLength(keyType) * dictionary.Count + length;
                }

                foreach (object key in dictionary.Keys)
                {
                    length += GetMemberLength(dictionary[key], valueType, serializerOption) + GetMemberLength(key, keyType, serializerOption);
                }

                return length;
            }

            private static int GetCollectionLength(ICollection collection, Type collectionType, SerializerOption serializerOption)
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
                        length += GetMemberLength(list[i], genericType, serializerOption);
                    }
                }
                else
                {
                    foreach (object obj in collection)
                    {
                        length += GetMemberLength(obj, genericType, serializerOption);
                    }
                }
                return length;
            }

            private static int GetHashSetLength(IEnumerable set, Type hashSetType, SerializerOption serializerOption)
            {
                int length = 2;
                Type genericType = hashSetType.GenericTypeArguments[0];

                foreach (object value in set)
                {
                    length += GetMemberLength(value, genericType, serializerOption);
                }

                return length;
            }
        }
    }
}
