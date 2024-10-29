using System;
using ZincFramework.Serialization;


namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public abstract class StrProfileWriter
    {
        public abstract Type GetWriteType();

        public abstract bool CanWrite(string typeStr);

        public abstract void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption);

        public abstract string GetExcelString(object obj);

        public abstract void WriteWriteMethod(ref MethodWriter methodWriter);

        public abstract void WriteReadMethod(ref MethodWriter methodWriter);
    }
}