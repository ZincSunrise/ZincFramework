using ZincFramework.Binary.Serialization.Metadata;

namespace ZincFramework.Binary.Serialization.Converters
{
    public abstract class ObjectConverter<T> : BinaryConverter<T>
    {
        protected BinaryTypeInfo<T> _binaryTypeInfo;
        public override ConvertStrategy GetConvertStrategy() => ConvertStrategy.Object;
    }
}