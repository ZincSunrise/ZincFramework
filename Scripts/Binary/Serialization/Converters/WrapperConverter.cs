using System;
using ZincFramework.Binary.Serialization;

public class WrapperConverter<T> : BinaryConverter<T>
{
    private readonly BinaryConverter _binaryConverter;

    private readonly BinaryConverter<T> _typedConverter;

    public override Type ElementType => _binaryConverter.ElementType;

    public override Type KeyType => _binaryConverter.KeyType;

    public override Type ValueType => _binaryConverter.ValueType;

    public override Type ConvertType => _binaryConverter.ConvertType;


    public WrapperConverter(BinaryConverter binaryConverter)
    {
        _binaryConverter = binaryConverter;
        if (_binaryConverter is BinaryConverter<T> typedConverter)
        {
            _typedConverter = typedConverter;
        }
    }

    public override T Read(ref ByteReader byteReader, SerializerOption serializerOption)
    {
        if (_typedConverter == null)
        {
            return (T)_binaryConverter.ConvertAsObject(ref byteReader, serializerOption);
        }

        return _typedConverter.Read(ref byteReader, serializerOption);
    }

    public override void Write(T data, ByteWriter byteWriter, SerializerOption serializerOption)
    {
        if (_typedConverter == null)
        {
            _binaryConverter.WriteAsObject(data, byteWriter, serializerOption);
            return;
        }

        _typedConverter.Write(data, byteWriter, serializerOption);
    }
}