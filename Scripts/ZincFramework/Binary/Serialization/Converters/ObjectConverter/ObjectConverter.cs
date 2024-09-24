namespace ZincFramework.Binary.Serialization.Converters
{
    public abstract class ObjectConverter<T> : BinaryConverter<T>
    {
        public override ConvertStrategy GetConvertStrategy() => ConvertStrategy.Object;
    }
}