using System;

namespace ZincFramework.Binary.Serialization.Converters
{
    public class BooleanConverter : SimpleValueConverter<bool>
    {
        public override bool Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadBoolean();
        }

        public override void Write(bool data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteBoolean(data);
        }
    }
}
