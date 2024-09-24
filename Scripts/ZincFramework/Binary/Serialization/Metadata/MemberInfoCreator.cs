using Newtonsoft.Json.Converters;
using System;
using System.Reflection;
using System.Text.Json;
using ZincFramework.Binary.Serialization.MetaModule;
using ZincFramework.Serialization;
using ZincFramework.Serialization.Events;


namespace ZincFramework.Binary.Serialization
{
    namespace Metadata
    {
        internal static class MemberInfoCreator
        {
            internal static BinaryFieldInfo<T> GetFieldMetaData<T>(BinaryTypeInfo ownerTypeInfo, FieldInfo fieldInfo, SerializerOption serializerOption)
            {
                return new BinaryFieldInfo<T>(ownerTypeInfo, fieldInfo, serializerOption);
            }

            internal static BinaryPropertyInfo CreatePropertyMetaData(BinaryTypeInfo ownerTypeInfo, PropertyInfo propertyInfo, SerializerOption serializerOption)
            {
                Type propertyType = propertyInfo.PropertyType;
                BinaryPropertyInfo binaryPropertyInfo;
                if (propertyType.IsPrimitive)
                {
                    binaryPropertyInfo = propertyType switch
                    {
                        not null when propertyType == typeof(int) => new BinaryPropertyInfo<int>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(float) => new BinaryPropertyInfo<float>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(bool) => new BinaryPropertyInfo<bool>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(double) => new BinaryPropertyInfo<double>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(long) => new BinaryPropertyInfo<long>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(short) => new BinaryPropertyInfo<short>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(char) => new BinaryPropertyInfo<char>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ushort) => new BinaryPropertyInfo<ushort>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(uint) => new BinaryPropertyInfo<uint>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ulong) => new BinaryPropertyInfo<ulong>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(byte) => new BinaryPropertyInfo<byte>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(sbyte) => new BinaryPropertyInfo<sbyte>(ownerTypeInfo, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else if (propertyType.IsEnum)
                {
                    Type underType = propertyType.GetEnumUnderlyingType();
                    binaryPropertyInfo = underType switch
                    {
                        not null when underType == typeof(int) => new BinaryPropertyInfo<int>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(long) => new BinaryPropertyInfo<long>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(short) => new BinaryPropertyInfo<short>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(byte) => new BinaryPropertyInfo<byte>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(uint) => new BinaryPropertyInfo<uint>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(ushort) => new BinaryPropertyInfo<ushort>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(ulong) => new BinaryPropertyInfo<ulong>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(sbyte) => new BinaryPropertyInfo<sbyte>(ownerTypeInfo, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else
                {
                    Type genericType = typeof(BinaryPropertyInfo<>).MakeGenericType(propertyType);
                    ConstructorInfo constructorInfo = genericType.GetConstructor(new Type[]
                    {
                        typeof(BinaryTypeInfo),
                        typeof(MemberInfo),
                        typeof(SerializerOption),
                    });

                    binaryPropertyInfo = constructorInfo.Invoke(null, new object[]
                    { 
                        ownerTypeInfo, 
                        propertyInfo,
                        serializerOption
                    }) as BinaryPropertyInfo;
                }

                return binaryPropertyInfo;
            }

            internal static BinaryPropertyInfo GetPropertyMetaData<T>(BinaryTypeInfo ownerTypeInfo, PropertyInfo propertyInfo, SerializerOption serializerOption)
            {
                Type propertyType = propertyInfo.PropertyType;
                BinaryPropertyInfo binaryPropertyInfo;
                if (propertyType.IsPrimitive)
                {
                    binaryPropertyInfo = propertyType switch
                    {
                        not null when propertyType == typeof(int) => CreatePropertyInfo<T, int>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(float) => CreatePropertyInfo<T, float>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(bool) => CreatePropertyInfo<T, bool>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(double) => CreatePropertyInfo<T, double>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(long) => CreatePropertyInfo<T, long>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(short) => CreatePropertyInfo<T, short>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(char) => CreatePropertyInfo<T, char>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ushort) => CreatePropertyInfo<T, ushort>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(uint) => CreatePropertyInfo<T, uint>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ulong) => CreatePropertyInfo<T, ulong>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(byte) => CreatePropertyInfo<T, byte>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(sbyte) => CreatePropertyInfo<T, sbyte>(ownerTypeInfo, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else if (propertyType.IsEnum)
                {
                    Type underType = propertyType.GetEnumUnderlyingType();
                    binaryPropertyInfo = underType switch
                    {
                        not null when underType == typeof(int) => CreatePropertyInfo<T, int>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(long) => CreatePropertyInfo<T, long>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(short) => CreatePropertyInfo<T, short>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(byte) => CreatePropertyInfo<T, byte>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(uint) => CreatePropertyInfo<T, uint>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(ushort) => CreatePropertyInfo<T, ushort>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(ulong) => CreatePropertyInfo<T, ulong>(ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(sbyte) => CreatePropertyInfo<T, sbyte>(ownerTypeInfo, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else
                {
                    Type genericType = typeof(BinaryPropertyInfo<>).MakeGenericType(propertyType);
                    ConstructorInfo constructorInfo = genericType.GetConstructor(new Type[]
                    {
                        typeof(BinaryTypeInfo),
                        typeof(MemberInfo),
                        typeof(SerializerOption),
                    });

                    binaryPropertyInfo = constructorInfo.Invoke(new object[]
                    {
                        ownerTypeInfo,
                        propertyInfo,
                        serializerOption
                    }) as BinaryPropertyInfo;
                }

                return binaryPropertyInfo;
            }

            internal static BinaryPropertyInfo<TMember> CreatePropertyInfo<TOwner, TMember>(BinaryTypeInfo binaryTypeInfo, PropertyInfo propertyInfo, SerializerOption serializerOption)
            {
                BinaryPropertyInfo<TMember> binaryPropertyInfo = new BinaryPropertyInfo<TMember>(binaryTypeInfo, propertyInfo, serializerOption);
                var (getAction, setAction) = DefaultMetaModule.AccessorsProvider.GetPropertyAccessors<TOwner, TMember>(propertyInfo);

                binaryPropertyInfo.GetAction = (a) => getAction.Invoke((TOwner)a);
                binaryPropertyInfo.SetAction = (a, b) => setAction.Invoke((TOwner)a, b);

                return binaryPropertyInfo;
            }
        }
    }
}