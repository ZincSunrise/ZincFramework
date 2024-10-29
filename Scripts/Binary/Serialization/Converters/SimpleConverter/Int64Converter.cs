namespace ZincFramework.Binary.Serialization.Converters
{
    public class Int64Converter : SimpleValueConverter<long>
    {
        public override long Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadInt64();
        }

        public override void Write(long data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt64(data);
        }
    }
}
