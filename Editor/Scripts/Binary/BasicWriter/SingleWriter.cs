namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class SingleWriter : BasicWriter<float>
    {
        public override bool CanWrite(string typeStr) => typeStr == "float" || typeStr == "Single";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteSingle(float.Parse(str));
        }
    }
}

