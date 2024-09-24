namespace ZincFramework.Binary.Serialization.Converters
{
    public class Int32Converter : SimpleValueConverter<int>
    {
        public override int Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return serializerOption.IsUsingVarint ? byteReader.ReadVarInt32() : byteReader.ReadInt32();
        }

        public override void Write(int data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            if (serializerOption.IsUsingVarint)
            {
                byteWriter.WriteVarInt32(data);
            }
            else
            {
                byteWriter.WriteInt32(data);
            }
        }
    }
}
