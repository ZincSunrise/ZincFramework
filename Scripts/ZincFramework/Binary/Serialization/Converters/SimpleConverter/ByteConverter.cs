namespace ZincFramework.Binary.Serialization.Converters
{
    public class ByteConverter : SimpleValueConverter<byte>
    {
        public override byte Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadByte();
        }

        public override void Write(byte data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteByte(data);
        }
    }
}
