namespace ZincFramework.Binary.Serialization.Converters
{
    public class SingleConverter : SimpleValueConverter<float>
    {
        public override float Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadSingle();
        }

        public override void Write(float data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteSingle(data);
        }
    }
}
