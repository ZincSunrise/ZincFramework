using ZincFramework.Serialization;

namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class StringWriter : BasicWriter<string>
    {
        public override bool CanWrite(string typeStr) => typeStr == "string" || typeStr == "String";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteString(str);
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            if (methodWriter.Current.IsWriteMember)
            {
                methodWriter.WriteInList(SerializeWriteUtility.GetCodeStr);
                methodWriter.ReadPrimitiveInList("String");
            }
            else
            {
                methodWriter.WriteInList($"{methodWriter.Current.Name} = byteReader.ReadString();");
            }
        }
    }
}