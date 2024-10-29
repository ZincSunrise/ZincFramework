using System;


namespace ZincFramework.Binary.Serialization.Metadata
{
    public interface IMetaModule
    {
        BinaryTypeInfo CreateTypeInfo(Type type, SerializerOption serializerOption);

        BinaryTypeInfo<T> CreateTypeInfo<T>(SerializerOption serializerOption);

        BinaryTypeInfo<T> CreateTypeInfo<T>(Func<T> factory, SerializerOption serializerOption);
    } 
}
