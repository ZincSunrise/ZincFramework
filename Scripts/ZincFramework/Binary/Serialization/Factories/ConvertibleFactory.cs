using System;

namespace ZincFramework.Binary.Serialization.Factory
{
    public abstract class ConvertibleFactory
    {
        internal virtual ConvertStrategy GetStrategy() => ConvertStrategy.None;

        internal abstract BinaryConverter GetConverter(Type customType, SerializerOption serializerOption);

        internal abstract BinaryConverter<T> GetConverter<T>(SerializerOption serializerOption);

        internal abstract bool IsSerializable(Type type); 
    }
}
