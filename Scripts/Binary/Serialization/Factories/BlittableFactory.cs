using System;
using ZincFramework.Binary.Serialization.Converters;


namespace ZincFramework.Binary.Serialization.Factory
{
    public class BlittableFactory : ConvertibleFactory
    {
        internal override BinaryConverter GetConverter(Type customType, SerializerOption serializerOption)
        {
            Type genericType = typeof(BlittableConverter<>).MakeGenericType(customType);
            return Activator.CreateInstance(genericType) as BinaryConverter;
        }


        internal override ConvertStrategy GetStrategy() => ConvertStrategy.Blittable;

        internal override bool IsSerializable(Type type) => type.IsValueType && (typeof(IFormattable).IsAssignableFrom(type) || typeof(IBlittable).IsAssignableFrom(type));
    }
}