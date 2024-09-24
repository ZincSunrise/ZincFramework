namespace ZincFramework.Binary.Serialization.Converters
{
    public class SByteConverter : SimpleValueConverter<sbyte>
    {
        public override sbyte Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadSByte();
        }

        public override void Write(sbyte data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteSByte(data);
        }
    }
}
