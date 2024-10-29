using System;
using ZincFramework.Serialization;

namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public sealed class IgnoreWriter : StrProfileWriter
    {
        public override bool CanWrite(string typeStr) => typeStr.Equals("ignore", StringComparison.OrdinalIgnoreCase);


        public override Type GetWriteType() => null;


        public override string GetExcelString(object obj)
        {
            return string.Empty;
        }


        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            
        }

        public override void WriteWriteMethod(ref MethodWriter methodWriter)
        {

        }
    }
}