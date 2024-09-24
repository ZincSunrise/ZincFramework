namespace ZincFramework.Binary.Serialization.Converters
{
    public class UInt64Converter : SimpleValueConverter<ulong>
    {
        public override ulong Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadUInt64();
        }

        public override void Write(ulong data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteUInt64(data);
        }
    }
}
