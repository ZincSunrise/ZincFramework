namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class Int32Writer : BasicWriter<int>
    {
        public override bool CanWrite(string typeStr) => typeStr == "int" || typeStr == "Int32";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt32(int.Parse(str));
        }
    }
}

