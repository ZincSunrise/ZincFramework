using ZincFramework.Serialization;



namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public abstract class BasicWriter<T> : StrProfileWriter<T>
    {
        public override void WriteWriteMethod(ref MethodWriter methodWriter)
        {
            if (methodWriter.Current.IsWriteMember)
            {
                methodWriter.WriteInList(SerializeWriteUtility.GetWriteCode(methodWriter.Current.Name));
            }

            methodWriter.WritePrimitiveInList(typeof(T).Name);
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            if (methodWriter.Current.IsWriteMember)
            {
                methodWriter.WriteInList(SerializeWriteUtility.GetCodeStr);
            }

            methodWriter.ReadPrimitiveInList(typeof(T).Name);
        }

        public override string GetExcelString(object obj) => obj.ToString();
    }
}

