using System;

namespace ZincFramework.Binary.Serialization.Converters
{
    public abstract class SimpleValueConverter<T> : BinaryConverter<T>
    {
        public override ConvertStrategy GetConvertStrategy() => ConvertStrategy.SimpleValue;
    }
}