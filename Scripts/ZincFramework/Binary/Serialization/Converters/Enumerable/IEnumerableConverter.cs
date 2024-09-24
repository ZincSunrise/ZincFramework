using System;
using System.Collections;

namespace ZincFramework.Binary.Serialization.Converters
{
    public abstract class IEnumerableConverter<TCollection, TElement> : BinaryConverter<TCollection> where TCollection : IEnumerable
    {
        public override ConvertStrategy GetConvertStrategy() => ConvertStrategy.Enumerable;   
    }
}