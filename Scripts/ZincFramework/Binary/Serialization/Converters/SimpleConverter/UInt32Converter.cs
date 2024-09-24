namespace ZincFramework.Binary.Serialization.Converters
{
    public class UInt32Converter : SimpleValueConverter<uint>
    {
        public override uint Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadUInt32();
        }

        public override void Write(uint data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteUInt32(data);
        }
    }
}
