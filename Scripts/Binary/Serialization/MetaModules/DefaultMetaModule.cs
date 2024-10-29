using System;
using ZincFramework.Binary.Serialization.Metadata;




namespace ZincFramework.Binary.Serialization.MetaModule
{
    internal partial class DefaultMetaModule : IMetaModule
    {
        public BinaryTypeInfo CreateTypeInfo(Type type, SerializerOption serializerOption)
        {
            BinaryConverter binaryConverter = GetConverterFromType(type, serializerOption) ??
                throw new InvalidOperationException($"�ⲻ����{type.Name} �����е�����Ϊ{type?.GetElementType().Name ?? string.Join(' ', Array.ConvertAll(type?.GenericTypeArguments, (x) => x.Name))}");

            return CreateTypeInfoInternal(type, binaryConverter, serializerOption);
        }

        public BinaryTypeInfo<T> CreateTypeInfo<T>(SerializerOption serializerOption)
        {
            BinaryConverter binaryConverter = GetConverterFromType<T>(serializerOption) ??
             throw new InvalidOperationException($"�ⲻ����{typeof(T).Name} �����е�����Ϊ{typeof(T)?.GetElementType()?.Name ?? string.Join(' ', Array.ConvertAll(typeof(T)?.GenericTypeArguments ?? Array.Empty<Type>(), (x) => x.Name))}");

            return CreateTypeInfoInternal<T>(null, binaryConverter, serializerOption) as BinaryTypeInfo<T>;
        }

        public BinaryTypeInfo<T> CreateTypeInfo<T>(Func<T> factory, SerializerOption serializerOption)
        {
            BinaryConverter binaryConverter = GetConverterFromType<T>(serializerOption) ??
                throw new InvalidOperationException($"�ⲻ����{typeof(T).Name} �����е�����Ϊ{typeof(T)?.GetElementType().Name ?? string.Join(' ', Array.ConvertAll(typeof(T)?.GenericTypeArguments, (x) => x.Name))}");

            return CreateTypeInfoInternal(factory, binaryConverter, serializerOption) as BinaryTypeInfo<T>;
        }

        private BinaryTypeInfo CreateTypeInfoInternal(Type type, BinaryConverter binaryConverter, SerializerOption serializerOption)
        {
            return CreateTypeInfoCore(type, binaryConverter, serializerOption); 
        }

        private BinaryTypeInfo CreateTypeInfoInternal<T>(Func<T> factory, BinaryConverter binaryConverter, SerializerOption serializerOption)
        {
            return CreateTypeInfoCore<T>(factory, binaryConverter, serializerOption);
        }
    }
}