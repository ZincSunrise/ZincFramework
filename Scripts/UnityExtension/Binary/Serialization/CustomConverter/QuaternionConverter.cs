using UnityEngine;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class QuaternionConverter : BinaryConverter<Quaternion>
    {
        public override Quaternion Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            float x = byteReader.ReadSingle();
            float y = byteReader.ReadSingle();
            float z = byteReader.ReadSingle();
            float w = byteReader.ReadSingle();
            return new Quaternion(x, y, z, w);
        }

        public override void Write(Quaternion data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteSingle(data.x);
            byteWriter.WriteSingle(data.y);
            byteWriter.WriteSingle(data.z);
            byteWriter.WriteSingle(data.w);
        }
    }
}