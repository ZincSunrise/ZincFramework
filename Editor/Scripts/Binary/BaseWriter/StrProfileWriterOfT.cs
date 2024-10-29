using System;

namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public abstract class StrProfileWriter<T> : StrProfileWriter
    {
        public override Type GetWriteType() => typeof(T);
    }
}
