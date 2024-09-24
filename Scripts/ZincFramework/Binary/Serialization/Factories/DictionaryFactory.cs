using System;
using System.Collections;



namespace ZincFramework.Binary.Serialization.Factory
{
    internal class DictionaryFactory : ConvertibleFactory
    {
        internal override BinaryConverter GetConverter(Type type, SerializerOption serializerOption)
        {
            throw new System.NotImplementedException();
        }

        internal override BinaryConverter<T> GetConverter<T>(SerializerOption serializerOption)
        {
            throw new NotImplementedException();
        }

        internal override ConvertStrategy GetStrategy() => ConvertStrategy.Dictionary;

        internal override bool IsSerializable(Type type) => typeof(IDictionary).IsAssignableFrom(type);
    }
}
