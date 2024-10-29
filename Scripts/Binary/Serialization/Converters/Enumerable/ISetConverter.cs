using System.Collections.Generic;



namespace ZincFramework.Binary.Serialization.Converters
{
    public class ISetConverter<TSet, TElement> : IEnumerableConverter<TSet, TElement> where TSet : ISet<TElement>
    {
        public override TSet Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = SimpleConverters.Int32Converter.Read(ref byteReader, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);
            TSet values = serializerOption.GetTypeInfo<TSet>().CreateInstance();

            for (int i = 0; i < count; i++)
            {
                values.Add(_elementTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption));
            }

            return values;
        }

        public override void Write(TSet data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            SimpleConverters.Int32Converter.Write(data.Count, byteWriter, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);
            foreach (var item in data)
            {
                _elementTypeInfo.WrapperConverter.Write(item, byteWriter, serializerOption);
            }
        }
    }
}