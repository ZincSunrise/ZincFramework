using System;
using System.Collections;
using ZincFramework.Binary.Serialization.Metadata;

namespace ZincFramework.Binary.Serialization.Converters
{
    public abstract class IEnumerableConverter<TEnumerable, TElement> : BinaryConverter<TEnumerable> where TEnumerable : IEnumerable
    {
        public override ConvertStrategy GetConvertStrategy() => ConvertStrategy.Enumerable;

        public override Type ElementType => typeof(TElement);


        protected BinaryTypeInfo<TElement> _elementTypeInfo;

        protected BinaryTypeInfo<TElement> GetElementTypeInfo(SerializerOption serializerOption) => serializerOption.GetTypeInfo<TElement>();
    }
}