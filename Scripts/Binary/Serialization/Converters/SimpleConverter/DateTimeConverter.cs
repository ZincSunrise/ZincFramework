using System;

namespace ZincFramework.Binary.Serialization.Converters
{
    public class DateTimeConverter : SimpleValueConverter<DateTime>
    {
        public override DateTime Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadDateTime();
        }

        public override void Write(DateTime data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteDateTime(data);
        }
    }
}
