using System;
using System.Collections;
using System.Reflection;
using ZincFramework.Binary;
using ZincFramework.Serialization.Binary;
using ZincFramework.Serialization.Cache;
using ZincFramework.Serialization.Factory;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Excel
        {
            public static class ExcelDeserializer
            {
                public static object Deserialize(Type type, byte[] bytes)
                {
                    int nowIndex = 0;
                    object container = Activator.CreateInstance(type);
                    string typeDicName = TextUtility.LowerFirstString(type.Name);

                    typeDicName = typeDicName.Replace("Data", "Infos");
                    FieldInfo fieldInfo = type.GetField(typeDicName);
                    IDictionary dictionary = fieldInfo.GetValue(container) as IDictionary;
                    short count = ByteConverter.ToInt16(bytes, ref nowIndex);

                    Type keyType = fieldInfo.FieldType.GenericTypeArguments[0];
                    Type valueType = fieldInfo.FieldType.GenericTypeArguments[1];

                    if (!SerializationCachePool.TryGetTypeCache(type, out var typeCache))
                    {
                        typeCache = SerializationCachePool.CreateTypeCache(type, SerializeConfig.Property);
                    }

                    ISerializeConvert valueStrategy = ConverterFactory.Shared.CreateBuilder(keyType);
                    IConvert convert;

                    object key;

                    for (int i = 0;i < count; i++)
                    {
                        convert = typeCache.CreateInstance() as IConvert;

                        key = valueStrategy.Convert(bytes, ref nowIndex, keyType);
                        convert.Convert(bytes, ref nowIndex);
                        dictionary.Add(key, convert);
                    }
                    return container;
                }

                public static T Deserialize<T>(byte[] bytes)
                {
                    return (T)Deserialize(typeof(T), bytes);
                }
            }
        }
    }
}

