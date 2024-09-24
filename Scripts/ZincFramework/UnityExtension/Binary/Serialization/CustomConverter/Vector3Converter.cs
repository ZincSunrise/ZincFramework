using UnityEngine;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class Vector3Converter : BinaryConverter<Vector3>
    {
        public override Vector3 Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            float x = byteReader.ReadSingle();
            float y = byteReader.ReadSingle();
            float z = byteReader.ReadSingle();
            return new Vector3(x, y, z);
        }

        public override void Write(Vector3 data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteSingle(data.x);
            byteWriter.WriteSingle(data.y);
            byteWriter.WriteSingle(data.z);
        }
    }
}