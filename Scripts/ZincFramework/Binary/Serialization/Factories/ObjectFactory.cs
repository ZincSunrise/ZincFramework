using System;
using System.Collections;
using ZincFramework.Binary.Serialization.Converters;



namespace ZincFramework.Binary.Serialization.Factory
{
    public class ObjectFactory : ConvertibleFactory
    {
        internal override BinaryConverter GetConverter(Type customType, SerializerOption serializerOption)
        {
            Type converterType = typeof(DefaultObjectConverter<>).MakeGenericType(customType);
            BinaryConverter binaryConverter = Activator.CreateInstance(converterType) as BinaryConverter;
            return binaryConverter;
        }

        internal override BinaryConverter<T> GetConverter<T>(SerializerOption serializerOption)
        {
            return new DefaultObjectConverter<T>();
        }

        internal override ConvertStrategy GetStrategy() => ConvertStrategy.Object;

        internal override bool IsSerializable(Type type) => !typeof(IEnumerable).IsAssignableFrom(type);
    }
}
