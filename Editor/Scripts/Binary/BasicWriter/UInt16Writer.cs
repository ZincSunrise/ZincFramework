namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class UInt16Writer : BasicWriter<ushort>
    {
        public override bool CanWrite(string typeStr) => typeStr == "ushort" || typeStr == "UInt16";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteUInt16(ushort.Parse(str));
        }
    }
}

