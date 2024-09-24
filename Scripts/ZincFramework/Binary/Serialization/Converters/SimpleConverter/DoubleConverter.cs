namespace ZincFramework.Binary.Serialization.Converters
{
    public class DoubleConverter : SimpleValueConverter<double>
    {
        public override double Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadDouble();
        }

        public override void Write(double data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteDouble(data);
        }
    }
}
