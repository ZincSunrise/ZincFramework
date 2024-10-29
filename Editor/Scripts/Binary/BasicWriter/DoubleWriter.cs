namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class DoubleWriter : BasicWriter<double>
    {
        public override bool CanWrite(string typeStr) => typeStr == "double" || typeStr == "Double";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteDouble(double.Parse(str));
        }
    }
}

