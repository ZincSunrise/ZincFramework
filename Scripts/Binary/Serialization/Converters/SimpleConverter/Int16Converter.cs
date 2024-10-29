namespace ZincFramework.Binary.Serialization.Converters
{
    public class Int16Converter : SimpleValueConverter<short>
    {
        public override short Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadInt16();
        }

        public override void Write(short data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt16(data);
        }
    }
}
