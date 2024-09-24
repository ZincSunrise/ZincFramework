using System;

namespace ZincFramework.Binary.Serialization.Converters
{
    public class TimeSpanConverter : SimpleValueConverter<TimeSpan>
    {
        public override TimeSpan Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return byteReader.ReadTimeSpan();
        }

        public override void Write(TimeSpan data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteTimeSpan(data);
        }
    }
}
