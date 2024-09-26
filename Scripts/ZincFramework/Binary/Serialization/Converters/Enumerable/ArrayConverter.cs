namespace ZincFramework.Binary.Serialization.Converters
{
    public class ArrayConverter<T> : IEnumerableConverter<T[], T>
    {
        public override T[] Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int length = SimpleConverters.Int32Converter.Read(ref byteReader, serializerOption);
            T[] array = new T[length];

            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);

            for (int i = 0; i < length; i++) 
            {
                array[i] = _elementTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption);
            }

            return array;
        }

        public override void Write(T[] data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            SimpleConverters.Int32Converter.Write(data.Length, byteWriter, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);

            for (int i = 0; i < data.Length; i++) 
            {
                _elementTypeInfo.WrapperConverter.Write(data[i], byteWriter, serializerOption);
            }
        }
    }
}
