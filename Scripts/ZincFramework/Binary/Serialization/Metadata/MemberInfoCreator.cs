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
                int ordinalNumber = serializerOption.IsGiveupOrdinal ? fieldInfo.GetCustomAttribute<BinaryOrdinal>().OrdinalNumber : ownerTypeInfo.MemberInfos.Count;
                return new BinaryFieldInfo<T>(ordinalNumber, ownerTypeInfo, fieldInfo, serializerOption);
            }

            internal static BinaryPropertyInfo CreatePropertyMetaData(BinaryTypeInfo ownerTypeInfo, PropertyInfo propertyInfo, SerializerOption serializerOption)
            {
                Type propertyType = propertyInfo.PropertyType;
                int ordinalNumber = serializerOption.IsGiveupOrdinal ? propertyInfo.GetCustomAttribute<BinaryOrdinal>().OrdinalNumber : ownerTypeInfo.MemberInfos.Count;

                BinaryPropertyInfo binaryPropertyInfo;
                if (propertyType.IsPrimitive)
                {
                    binaryPropertyInfo = propertyType switch
                    {
                        not null when propertyType == typeof(int) => new BinaryPropertyInfo<int>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(float) => new BinaryPropertyInfo<float>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(bool) => new BinaryPropertyInfo<bool>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(double) => new BinaryPropertyInfo<double>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(long) => new BinaryPropertyInfo<long>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(short) => new BinaryPropertyInfo<short>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(char) => new BinaryPropertyInfo<char>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ushort) => new BinaryPropertyInfo<ushort>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(uint) => new BinaryPropertyInfo<uint>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ulong) => new BinaryPropertyInfo<ulong>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(byte) => new BinaryPropertyInfo<byte>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(sbyte) => new BinaryPropertyInfo<sbyte>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else if (propertyType.IsEnum)
                {
                    Type underType = propertyType.GetEnumUnderlyingType();
                    binaryPropertyInfo = underType switch
                    {
                        not null when underType == typeof(int) => new BinaryPropertyInfo<int>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(long) => new BinaryPropertyInfo<long>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(short) => new BinaryPropertyInfo<short>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(byte) => new BinaryPropertyInfo<byte>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(uint) => new BinaryPropertyInfo<uint>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(ushort) => new BinaryPropertyInfo<ushort>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(ulong) => new BinaryPropertyInfo<ulong>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(sbyte) => new BinaryPropertyInfo<sbyte>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else
                {
                    Type genericType = typeof(BinaryPropertyInfo<>).MakeGenericType(propertyType);
                    ConstructorInfo constructorInfo = genericType.GetConstructor(new Type[]
                    {
                        typeof(int),
                        typeof(BinaryTypeInfo),
                        typeof(MemberInfo),
                        typeof(SerializerOption),
                    });

                    binaryPropertyInfo = constructorInfo.Invoke(null, new object[]
                    { 
                        ordinalNumber,
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
                int ordinalNumber = serializerOption.IsGiveupOrdinal ? ownerTypeInfo.MemberInfos.Count : propertyInfo.GetCustomAttribute<BinaryOrdinal>().OrdinalNumber;

                BinaryPropertyInfo binaryPropertyInfo;
                if (propertyType.IsPrimitive)
                {
                    binaryPropertyInfo = propertyType switch
                    {
                        not null when propertyType == typeof(int) => CreatePropertyInfo<T, int>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(float) => CreatePropertyInfo<T, float>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(bool) => CreatePropertyInfo<T, bool>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(double) => CreatePropertyInfo<T, double>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(long) => CreatePropertyInfo<T, long>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(short) => CreatePropertyInfo<T, short>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(char) => CreatePropertyInfo<T, char>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ushort) => CreatePropertyInfo<T, ushort>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(uint) => CreatePropertyInfo<T, uint>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ulong) => CreatePropertyInfo<T, ulong>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(byte) => CreatePropertyInfo<T, byte>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when propertyType == typeof(sbyte) => CreatePropertyInfo<T, sbyte>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else if (propertyType.IsEnum)
                {
                    Type underType = propertyType.GetEnumUnderlyingType();
                    binaryPropertyInfo = underType switch
                    {
                        not null when underType == typeof(int) => CreatePropertyInfo<T, int>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(long) => CreatePropertyInfo<T, long>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(short) => CreatePropertyInfo<T, short>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(byte) => CreatePropertyInfo<T, byte>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(uint) => CreatePropertyInfo<T, uint>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(ushort) => CreatePropertyInfo<T, ushort>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(ulong) => CreatePropertyInfo<T, ulong>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        not null when underType == typeof(sbyte) => CreatePropertyInfo<T, sbyte>(ordinalNumber, ownerTypeInfo, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else
                {
                    Type genericType = typeof(BinaryPropertyInfo<>).MakeGenericType(propertyType);
                    ConstructorInfo constructorInfo = genericType.GetConstructor(new Type[]
                    {
                        typeof(int),
                        typeof(BinaryTypeInfo),
                        typeof(MemberInfo),
                        typeof(SerializerOption),
                    });

                    binaryPropertyInfo = constructorInfo.Invoke(new object[]
                    {
                        ordinalNumber,
                        ownerTypeInfo,
                        propertyInfo,
                        serializerOption
                    }) as BinaryPropertyInfo;
                }

                return binaryPropertyInfo;
            }

            internal static BinaryPropertyInfo<TMember> CreatePropertyInfo<TOwner, TMember>(int ordinalNumber, BinaryTypeInfo binaryTypeInfo, PropertyInfo propertyInfo, SerializerOption serializerOption)
            {
                BinaryPropertyInfo<TMember> binaryPropertyInfo = new BinaryPropertyInfo<TMember>(ordinalNumber, binaryTypeInfo, propertyInfo, serializerOption);
                var (getAction, setAction) = DefaultMetaModule.AccessorsProvider.GetPropertyAccessors<TOwner, TMember>(propertyInfo);

                binaryPropertyInfo.GetAction = (a) => getAction.Invoke((TOwner)a);
                binaryPropertyInfo.SetAction = (a, b) => setAction.Invoke((TOwner)a, b);

                return binaryPropertyInfo;
            }
        }
    }
}