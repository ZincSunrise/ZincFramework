namespace ZincFramework.Binary.Serialization.Converters
{
    public class ArrayConverter<T> : IEnumerableConverter<T[], T>
    {
        public override T[] Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int length = SimpleConverters.Int32Converter.Convert(ref byteReader, serializerOption);
            T[] array = new T[length];

            BinaryConverter<T> binaryConverter = serializerOption.GetTypeInfo<T>().WrapperConverter;
            for (int i = 0; i < length; i++) 
            {
                array[i] = binaryConverter.Convert(ref byteReader, serializerOption);
            }

            return array;
        }

        public override void Write(T[] data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            SimpleConverters.Int32Converter.Write(data.Length, byteWriter, serializerOption);
            BinaryConverter<T> binaryConverter = serializerOption.GetTypeInfo<T>().WrapperConverter;

            for (int i = 0; i < data.Length; i++) 
            {
                binaryConverter.Write(data[i], byteWriter, serializerOption);
            }
        }
    }
}
