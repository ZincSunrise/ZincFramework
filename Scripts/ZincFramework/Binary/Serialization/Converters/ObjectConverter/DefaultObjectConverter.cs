using ZincFramework.Binary.Serialization.Metadata;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class DefaultObjectConverter<T> : ObjectConverter<T> where T : notnull
    {
        public override T Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            BinaryTypeInfo<T> binaryTypeInfo = serializerOption.GetTypeInfo<T>();

            T data = binaryTypeInfo.CreateInstance();
            binaryTypeInfo.ReadAndSetProperties(data, ref byteReader);
            return data;
        }

        public override void Write(T data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            BinaryTypeInfo<T> binaryTypeInfo = serializerOption.GetTypeInfo<T>();
            binaryTypeInfo.GetProperiesAndWrite(data, byteWriter);

        }
    }
}
