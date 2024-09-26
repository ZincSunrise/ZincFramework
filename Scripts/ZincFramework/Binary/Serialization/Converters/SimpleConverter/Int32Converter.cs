namespace ZincFramework.Binary.Serialization.Converters
{
    public class Int32Converter : SimpleValueConverter<int>
    {
        public override int Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadInt32();
        }

        public override void Write(int data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt32(data);
        }
    }
}
