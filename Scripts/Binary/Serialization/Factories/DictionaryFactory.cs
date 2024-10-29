using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ZincFramework.Binary.Serialization.Converters;



namespace ZincFramework.Binary.Serialization.Factory
{
    internal class DictionaryFactory : ConvertibleFactory
    {
        internal override BinaryConverter GetConverter(Type type, SerializerOption serializerOption)
        {
            Type genericType;
            Type keyType;
            Type valueType;
            if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                keyType = type.GetGenericArguments()[0];
                valueType = type.GetGenericArguments()[1];

                genericType = typeof(DictionaryConverter< , >).MakeGenericType(keyType, valueType);
            }
            else if(type.GetGenericTypeDefinition() == typeof(ConcurrentDictionary<,>))
            {
                keyType = type.GetGenericArguments()[0];
                valueType = type.GetGenericArguments()[1];

                genericType = typeof(ConcurrentDictionary< , >).MakeGenericType(keyType, valueType);
            }
            else
            {
                keyType = type.GetGenericArguments()[0];
                valueType = type.GetGenericArguments()[1];

                genericType = typeof(IDictionaryConverter< , , >).MakeGenericType(type, keyType, valueType);
            }

            return Activator.CreateInstance(genericType) as BinaryConverter;
        }

        internal override ConvertStrategy GetStrategy() => ConvertStrategy.Dictionary;

        internal override bool IsSerializable(Type type) => typeof(IDictionary).IsAssignableFrom(type);
    }
}
