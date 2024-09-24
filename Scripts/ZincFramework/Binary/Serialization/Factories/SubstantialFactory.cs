using System;
using ZincFramework.Binary.Serialization.Converters;


namespace ZincFramework.Binary.Serialization.Factory
{
    public class SubstantialFactory : ConvertibleFactory
    {
        internal override BinaryConverter GetConverter(Type customType, SerializerOption serializerOption)
        {
            Type genericType = typeof(SubstantialConverter<>).MakeGenericType(customType);
            return Activator.CreateInstance(genericType) as BinaryConverter;
        }

        internal override BinaryConverter<T> GetConverter<T>(SerializerOption serializerOption)
        {
            Type genericType = typeof(SubstantialConverter<>).MakeGenericType(typeof(T));
            return Activator.CreateInstance(genericType) as BinaryConverter<T>;
        }

        internal override ConvertStrategy GetStrategy() => ConvertStrategy.Substantial;

        internal override bool IsSerializable(Type type) => type.IsValueType && (typeof(IFormattable).IsAssignableFrom(type) || typeof(ISubstantiable).IsAssignableFrom(type));
    }
}