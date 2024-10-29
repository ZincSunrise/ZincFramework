
using ZincFramework.Serialization;

namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class EnumWriter : BasicWriter<int>
    {
        public readonly string _typeName;

        public EnumWriter(string name)
        {
            _typeName = name;
        }

        public override bool CanWrite(string typeStr) => typeStr == _typeName;

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt32(int.Parse(str));
        }

        public override void WriteWriteMethod(ref MethodWriter methodWriter)
        {
            if (methodWriter.Current.IsWriteMember)
            {
                methodWriter.WriteInList(SerializeWriteUtility.GetWriteCode(methodWriter.Current.Name));
            }

            methodWriter.WriteSimpleList("byteWriter.WriteInt32((int){0});");
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            if (methodWriter.Current.IsWriteMember)
            {
                methodWriter.WriteInList(SerializeWriteUtility.GetCodeStr);
                methodWriter.ReadSimpleInList($"({_typeName})byteReader.ReadInt32()");
            }
            else
            {
                methodWriter.WriteInList($"{methodWriter.Current.Name} = ({_typeName})byteReader.ReadInt32();");
            }
        }


        public override string GetExcelString(object obj) => ((int)obj).ToString();
    }
}
