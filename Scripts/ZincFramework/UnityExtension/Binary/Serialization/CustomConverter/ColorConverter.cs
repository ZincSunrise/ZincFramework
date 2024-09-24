using UnityEngine;

namespace ZincFramework.Binary.Serialization.Converters
{
    public class ColorConverter : BinaryConverter<Color>
    {
        public override Color Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            float r = byteReader.ReadSingle();
            float g = byteReader.ReadSingle();
            float b = byteReader.ReadSingle();
            float a = byteReader.ReadSingle();
            return new Color(r, g, b, a);
        }

        public override void Write(Color data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteSingle(data.r);
            byteWriter.WriteSingle(data.g);
            byteWriter.WriteSingle(data.b);
            byteWriter.WriteSingle(data.a);
        }
    }
}