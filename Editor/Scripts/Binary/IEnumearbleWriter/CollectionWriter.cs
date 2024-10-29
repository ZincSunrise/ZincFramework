using System.Collections.Generic;
using ZincFramework.Serialization;


namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class CollectionWriter<TCollection, TElement> : IEnumerableWriter<TCollection, TElement> where TCollection : ICollection<TElement>
    {
        public override bool CanWrite(string typeStr)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            throw new System.NotImplementedException();
        }

        public override string GetExcelString(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteWriteMethod(ref MethodWriter methodWriter)
        {
            throw new System.NotImplementedException();
        }
    }
}
