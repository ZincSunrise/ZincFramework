namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class UInt32Writer : BasicWriter<uint>
    {
        public override bool CanWrite(string typeStr) => typeStr == "uint" || typeStr == "UInt16";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteUInt32(uint.Parse(str));
        }
    }
}

