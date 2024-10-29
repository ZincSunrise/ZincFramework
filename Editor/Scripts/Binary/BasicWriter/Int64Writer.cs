namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class Int64Writer : BasicWriter<long>
    {
        public override bool CanWrite(string typeStr) => typeStr == "long" || typeStr == "Int64";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt64(long.Parse(str));
        }
    }
}
