namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class ByteWriter : BasicWriter<byte>
    {
        public override bool CanWrite(string typeStr) => typeStr == "byte" || typeStr == "Byte";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteByte(byte.Parse(str));
        }
    }
}