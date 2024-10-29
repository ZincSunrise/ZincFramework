namespace ZincFramework.Binary.Serialization.Converters
{
    public class UInt16Converter : SimpleValueConverter<ushort>
    {
        public override ushort Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadUInt16();
        }

        public override void Write(ushort data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteUInt16(data);
        }
    }
}
