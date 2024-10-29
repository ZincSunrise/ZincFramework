namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class SByteWriter : BasicWriter<sbyte>
    {
        public override bool CanWrite(string typeStr) => typeStr == "sbyte" || typeStr == "SByte";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteSByte(sbyte.Parse(str));
        }
    }
}


