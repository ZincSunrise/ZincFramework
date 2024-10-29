namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class BooleanWriter : BasicWriter<bool>
    {
        public override bool CanWrite(string typeStr) => typeStr == "bool" || typeStr == "Boolean";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteBoolean(bool.Parse(str));
        }
    }
}
