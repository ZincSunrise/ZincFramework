using ZincFramework.Serialization;

namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public abstract class ReferenceWriter<T> : StrProfileWriter<T>
    {
        public override void WriteWriteMethod(ref MethodWriter methodWriter)
        {
            if (methodWriter.Current.IsWriteMember)
            {
                methodWriter.BeginWriteIfReference();
            }
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            if (methodWriter.Current.IsWriteMember)
            {
                methodWriter.BeginReadIfReference();
            }
        }
    }
}