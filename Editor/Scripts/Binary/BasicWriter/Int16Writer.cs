namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class Int16Writer : BasicWriter<short>
    {
        public override bool CanWrite(string typeStr) => typeStr == "short" || typeStr == "Int16";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt16(short.Parse(str));
        }
    }
}

