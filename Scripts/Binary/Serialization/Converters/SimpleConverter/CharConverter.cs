namespace ZincFramework.Binary.Serialization.Converters
{
    public class CharConverter : SimpleValueConverter<char>
    {
        public override char Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadChar();
        }

        public override void Write(char data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteChar(data);
        }
    }
}
