using System.Collections.Generic;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class HashConverter<T> : ISetConverter<HashSet<T>, T>
    {
        public override HashSet<T> Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = SimpleConverters.Int32Converter.Convert(ref byteReader, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);
            HashSet<T> values = new HashSet<T>();

            for (int i = 0; i < count; i++)
            {
                values.Add(_elementTypeInfo.WrapperConverter.Convert(ref byteReader, serializerOption));
            }

            return values;
        }

        public override void Write(HashSet<T> data, ByteWriter byteWriter, SerializerOption serializerOption)
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