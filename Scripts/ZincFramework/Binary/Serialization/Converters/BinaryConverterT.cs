using System;

namespace ZincFramework
{
    namespace Binary
    {
        namespace Serialization
        {
            public abstract class BinaryConverter<T> : BinaryConverter
            {
                public override Type ConvertType => typeof(T);

                public override bool CanConvert(Type convertType) => ConvertType == convertType;

                public override ConvertStrategy GetConvertStrategy() => ConvertStrategy.Custom;

                public override void WriteAsObject(object data, ByteWriter byteWriter, SerializerOption serializerOption) => Write((T)data, byteWriter, serializerOption);

                public override object ConvertAsObject(ref ByteReader byteReader, SerializerOption serializerOption) => Convert(ref byteReader, serializerOption);

                public abstract T Convert(ref ByteReader byteReader, SerializerOption serializerOption);

                public abstract void Write(T data, ByteWriter byteWriter, SerializerOption serializerOption);
            }
        }
    }
}