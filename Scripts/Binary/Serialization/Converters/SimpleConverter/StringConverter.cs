namespace ZincFramework.Binary.Serialization.Converters
{
    public class StringConverter : SimpleValueConverter<string>
    {
        public override string Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadString();
        }

        public override void Write(string data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteString(data);
        }
    }
}