using System;
using System.Reflection;


namespace ZincFramework
{
    namespace Binary
    {
        namespace Serialization
        {
            public enum ConvertStrategy : byte
            {
                None = 0,

                SimpleValue = 2,

                Substantial = 4,

                Custom = 8,

                Enumerable = 16,

                Dictionary = 32,

                Object = 64,
            }

            public abstract class BinaryConverter
            {
                public abstract Type ConvertType { get; }

                public virtual Type ElementType { get; }

                public virtual Type KeyType { get;  }

                public virtual Type ValueType { get; }

                public ConstructorInfo ConstructorInfo { get; set; }

                public abstract bool CanConvert(Type convertType);

                public virtual ConvertStrategy GetConvertStrategy() => ConvertStrategy.None;

                public abstract object ConvertAsObject(ref ByteReader byteReader, SerializerOption serializerOption);

                public abstract void WriteAsObject(object data, ByteWriter byteWriter, SerializerOption serializerOption);
            }
        }
    }
}
