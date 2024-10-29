namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class CharWriter : BasicWriter<char>
    {
        public override bool CanWrite(string typeStr) => typeStr == "char" || typeStr == "Char";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteChar(char.Parse(str));
        }
    }
}
