using System;
using System.Collections.Generic;


namespace ZincFramework.Binary.Serialization.Factory
{
    public class CustomFactory : ConvertibleFactory
    {
        private readonly Dictionary<Type, BinaryConverter> _converterMaps = new Dictionary<Type, BinaryConverter>();

        internal override BinaryConverter GetConverter(Type customType, SerializerOption serializerOption)
        {
            return _converterMaps[customType];
        }


        internal override BinaryConverter<T> GetConverter<T>(SerializerOption serializerOption)
        {
            return _converterMaps[typeof(T)] as BinaryConverter<T>;
        }

        public void AddConverter(Type customType, BinaryConverter converter) 
        {
            _converterMaps.Add(customType, converter);
        }

        internal override ConvertStrategy GetStrategy() => ConvertStrategy.Custom;

        internal override bool IsSerializable(Type type) => _converterMaps.ContainsKey(type);
    }
}
