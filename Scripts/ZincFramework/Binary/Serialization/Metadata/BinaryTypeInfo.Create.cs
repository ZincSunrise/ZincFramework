using System;
using System.Reflection;




namespace ZincFramework.Binary.Serialization.Metadata
{
    public abstract partial class BinaryTypeInfo
    {
        public static BinaryTypeInfo CreateTypeInfo(Type cacheType, BinaryConverter binaryConverter, SerializerOption serializerOption)
        {
            Type genericType = typeof(BinaryTypeInfo<>).MakeGenericType(cacheType);
            ConstructorInfo constructorInfo = genericType.GetConstructor(new[]
            {
                    typeof(BinaryConverter),
                    typeof(SerializerOption)
                });
            return constructorInfo.Invoke(new object[] { binaryConverter, serializerOption }) as BinaryTypeInfo;
        }

        public static BinaryTypeInfo CreateTypeInfo<T>(BinaryConverter binaryConverter, SerializerOption serializerOption)
        {
            return new BinaryTypeInfo<T>(binaryConverter, serializerOption);
        }

        public static BinaryTypeInfo CreateTypeInfo<T>(Func<T> factory, BinaryConverter binaryConverter, SerializerOption serializerOption)
        {
            return new BinaryTypeInfo<T>(factory, binaryConverter, serializerOption);
        }

    }
}