namespace ZincFramework.Binary.Serialization.Converters
{
    public class BlittableArrayConverter<T> : ArrayConverter<T> where T : struct
    {
        public override T[] Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadArray<T>();
        }

        public override void Write(T[] data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteArray(data);
        }
    }
}