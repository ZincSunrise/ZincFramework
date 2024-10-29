namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class UInt64Writer : BasicWriter<ulong>
    {
        public override bool CanWrite(string typeStr) => typeStr == "ulong" || typeStr == "UInt64";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteUInt64(ulong.Parse(str));
        }
    }
}

