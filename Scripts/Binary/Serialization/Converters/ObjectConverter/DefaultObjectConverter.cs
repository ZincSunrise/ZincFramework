namespace ZincFramework.Binary.Serialization.Converters
{
    public class DefaultObjectConverter<T> : ObjectConverter<T> where T : notnull
    {
        public override T Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            _binaryTypeInfo ??= serializerOption.GetTypeInfo<T>();

            T data = _binaryTypeInfo.CreateInstance();
            _binaryTypeInfo.ReadAndSetProperties(data, ref byteReader);
            return data;
        }

        public override void Write(T data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            _binaryTypeInfo ??= serializerOption.GetTypeInfo<T>();
            _binaryTypeInfo.GetProperiesAndWrite(data, byteWriter);
        }
    }
}
