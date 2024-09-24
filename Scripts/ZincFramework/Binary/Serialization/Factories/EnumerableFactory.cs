using System;
using System.Collections;
using System.Collections.Generic;
using ZincFramework.Binary.Serialization.Converters;
using ZincFramework.Binary.Serialization.MetaModule;



namespace ZincFramework.Binary.Serialization.Factory
{
    public class EnumerableFactory : ConvertibleFactory
    {
        internal override BinaryConverter GetConverter(Type type, SerializerOption serializerOption)
        {
            Type genericType = null;
            Type elementType = null;

            if (type.IsArray)
            {
                if(type.GetArrayRank() > 1)
                {
                    throw new NotSupportedException("不支持二维及以上的数组");
                }

                elementType = type.GetElementType();

                if (DefaultMetaModule.IsSubstantial(elementType))
                {
                    genericType = typeof(SubstantialArrayConverter<>).MakeGenericType(elementType);
                }
                else
                {
                    genericType = typeof(ArrayConverter<>).MakeGenericType(elementType);
                }
            }
            else if(type.GetGenericTypeDefinition() == typeof(List<>))
            {
                elementType = type.GetGenericArguments()[0];
            }

            return Activator.CreateInstance(genericType) as BinaryConverter;
        }

        internal override BinaryConverter<T> GetConverter<T>(SerializerOption serializerOption)
        {
            return GetConverter(typeof(T), serializerOption) as BinaryConverter<T>;
        }

        internal override ConvertStrategy GetStrategy() => ConvertStrategy.Enumerable;

        internal override bool IsSerializable(Type type) => typeof(IEnumerable).IsAssignableFrom(type);
    }
}
