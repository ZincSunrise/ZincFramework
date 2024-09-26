using System;

namespace ZincFramework.Binary.Serialization.Converters
{
    public class UriConverter : SimpleValueConverter<Uri>
    {
        public override Uri Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            return new Uri(byteReader.ReadString());
        }

        public override void Write(Uri data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteString(data.OriginalString);
        }
    }
}
