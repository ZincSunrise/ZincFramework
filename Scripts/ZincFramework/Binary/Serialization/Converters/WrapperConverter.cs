using System;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class WrapperConverter<T> : BinaryConverter<T>
    {
        private readonly BinaryConverter _binaryConverter;

        public override Type ElementType => _binaryConverter.ElementType;

        public override Type KeyType => _binaryConverter.KeyType;

        public override Type ValueType => _binaryConverter.ValueType;

        public override Type ConvertType => _binaryConverter.ConvertType;


        public WrapperConverter(BinaryConverter binaryConverter)
        {
            _binaryConverter = binaryConverter;
        }

        public override T Convert(ref ByteReader byteReader, SerializerOption serializerOption) => (T)_binaryConverter.ConvertAsObject(ref byteReader, serializerOption);

        public override void Write(T data, ByteWriter byteWriter, SerializerOption serializerOption) => _binaryConverter.WriteAsObject(data, byteWriter, serializerOption);
    }
}
