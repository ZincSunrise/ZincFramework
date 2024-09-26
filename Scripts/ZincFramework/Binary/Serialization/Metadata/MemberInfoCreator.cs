using System;
using System.Reflection;
using ZincFramework.Binary.Serialization.MetaModule;




namespace ZincFramework.Binary.Serialization
{
    namespace Metadata
    {
        internal static class MemberInfoCreator
        {
            internal static BinaryMemberInfo GetFieldMetaData<T>(BinaryTypeInfo ownerTypeInfo, FieldInfo fieldInfo, SerializerOption serializerOption)
            {
                Type fieldType = fieldInfo.FieldType;
                BinaryMemberInfo binaryFieldInfo;
                if (DefaultMetaModule.IsSubstantial(fieldType))
                {
                    binaryFieldInfo = fieldType switch
                    {
                        not null when fieldType == typeof(int) => CreateFieldInfo<T, int>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(float) => CreateFieldInfo<T, float>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(bool) => CreateFieldInfo<T, bool>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(string) => CreateFieldInfo<T, string>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(double) => CreateFieldInfo<T, double>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(long) => CreateFieldInfo<T, long>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(short) => CreateFieldInfo<T, short>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(TimeSpan) => CreateFieldInfo<T, TimeSpan>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(char) => CreateFieldInfo<T, char>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(ushort) => CreateFieldInfo<T, ushort>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(uint) => CreateFieldInfo<T, uint>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(ulong) => CreateFieldInfo<T, ulong>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(byte) => CreateFieldInfo<T, byte>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        not null when fieldType == typeof(sbyte) => CreateFieldInfo<T, sbyte>(ownerTypeInfo, fieldType, fieldInfo, serializerOption),
                        _ => null
                    };
                }
                else if (fieldType.IsEnum)
                {
                    Type underType = fieldType.GetEnumUnderlyingType();
                    binaryFieldInfo = underType switch
                    {
                        not null when underType == typeof(int) => CreateFieldInfo<T, int>(ownerTypeInfo, underType, fieldInfo, serializerOption),
                        not null when underType == typeof(long) => CreateFieldInfo<T, long>(ownerTypeInfo, underType, fieldInfo, serializerOption),
                        not null when underType == typeof(short) => CreateFieldInfo<T, short>(ownerTypeInfo, underType, fieldInfo, serializerOption),
                        not null when underType == typeof(byte) => CreateFieldInfo<T, byte>(ownerTypeInfo, underType, fieldInfo, serializerOption),
                        not null when underType == typeof(uint) => CreateFieldInfo<T, uint>(ownerTypeInfo, underType, fieldInfo, serializerOption),
                        not null when underType == typeof(ushort) => CreateFieldInfo<T, ushort>(ownerTypeInfo, underType, fieldInfo, serializerOption),
                        not null when underType == typeof(ulong) => CreateFieldInfo<T, ulong>(ownerTypeInfo, underType, fieldInfo, serializerOption),
                        not null when underType == typeof(sbyte) => CreateFieldInfo<T, sbyte>(ownerTypeInfo, underType, fieldInfo, serializerOption),
                        _ => null
                    };
                }
                else
                {
                    Type genericType = typeof(BinaryFieldInfo<>).MakeGenericType(fieldType);
                    ConstructorInfo constructorInfo = genericType.GetConstructor(new Type[]
                    {
                        typeof(BinaryTypeInfo),
                        typeof(Type),
                        typeof(MemberInfo),
                        typeof(SerializerOption),
                    });

                    binaryFieldInfo = constructorInfo.Invoke(new object[]
                    {
                        ownerTypeInfo,
                        fieldType,
                        fieldInfo,
                        serializerOption
                    }) as BinaryMemberInfo;
                }

                return binaryFieldInfo;
            }

            internal static BinaryMemberInfo GetPropertyMetaData<T>(BinaryTypeInfo ownerTypeInfo, PropertyInfo propertyInfo, SerializerOption serializerOption)
            {
                Type propertyType = propertyInfo.PropertyType;
                BinaryMemberInfo binaryPropertyInfo;
                if (DefaultMetaModule.IsSubstantial(propertyType))
                {
                    binaryPropertyInfo = propertyType switch
                    {
                        not null when propertyType == typeof(int) => CreatePropertyInfo<T, int>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(float) => CreatePropertyInfo<T, float>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(bool) => CreatePropertyInfo<T, bool>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(double) => CreatePropertyInfo<T, double>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(string) => CreatePropertyInfo<T, string>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(long) => CreatePropertyInfo<T, long>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(short) => CreatePropertyInfo<T, short>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(TimeSpan) => CreatePropertyInfo<T, TimeSpan>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(char) => CreatePropertyInfo<T, char>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ushort) => CreatePropertyInfo<T, ushort>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(uint) => CreatePropertyInfo<T, uint>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ulong) => CreatePropertyInfo<T, ulong>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(byte) => CreatePropertyInfo<T, byte>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(sbyte) => CreatePropertyInfo<T, sbyte>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else if (propertyType.IsArray && DefaultMetaModule.IsSubstantial(propertyType.GetElementType()))
                {
                    binaryPropertyInfo = propertyType switch
                    {
                        not null when propertyType == typeof(string[]) => CreatePropertyInfo<T, string[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(bool[]) => CreatePropertyInfo<T, bool[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(int[]) => CreatePropertyInfo<T, int[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(float[]) => CreatePropertyInfo<T, float[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(double[]) => CreatePropertyInfo<T, double[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),        
                        not null when propertyType == typeof(long[]) => CreatePropertyInfo<T, long[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(short[]) => CreatePropertyInfo<T, short[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(TimeSpan[]) => CreatePropertyInfo<T, TimeSpan[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(char[]) => CreatePropertyInfo<T, char[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ushort[]) => CreatePropertyInfo<T, ushort[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(uint[]) => CreatePropertyInfo<T, uint[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(ulong[]) => CreatePropertyInfo<T, ulong[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(byte[]) => CreatePropertyInfo<T, byte[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        not null when propertyType == typeof(sbyte[]) => CreatePropertyInfo<T, sbyte[]>(ownerTypeInfo, propertyType, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else if (propertyType.IsEnum)
                {
                    Type underType = propertyType.GetEnumUnderlyingType();
                    binaryPropertyInfo = underType switch
                    {
                        not null when underType == typeof(int) => CreatePropertyInfo<T, int>(ownerTypeInfo, underType, propertyInfo, serializerOption),
                        not null when underType == typeof(long) => CreatePropertyInfo<T, long>(ownerTypeInfo, underType, propertyInfo, serializerOption),
                        not null when underType == typeof(short) => CreatePropertyInfo<T, short>(ownerTypeInfo, underType, propertyInfo, serializerOption),
                        not null when underType == typeof(byte) => CreatePropertyInfo<T, byte>(ownerTypeInfo, underType, propertyInfo, serializerOption),
                        not null when underType == typeof(uint) => CreatePropertyInfo<T, uint>(ownerTypeInfo, underType, propertyInfo, serializerOption),
                        not null when underType == typeof(ushort) => CreatePropertyInfo<T, ushort>(ownerTypeInfo, underType, propertyInfo, serializerOption),
                        not null when underType == typeof(ulong) => CreatePropertyInfo<T, ulong>(ownerTypeInfo, underType, propertyInfo, serializerOption),
                        not null when underType == typeof(sbyte) => CreatePropertyInfo<T, sbyte>(ownerTypeInfo, underType, propertyInfo, serializerOption),
                        _ => null
                    };
                }
                else
                {
                    Type genericType = typeof(BinaryPropertyInfo<>).MakeGenericType(propertyType);
                    ConstructorInfo constructorInfo = genericType.GetConstructor(new Type[]
                    {
                        typeof(BinaryTypeInfo),
                        typeof(Type),
                        typeof(MemberInfo),
                        typeof(SerializerOption),
                    });

                    binaryPropertyInfo = constructorInfo.Invoke(new object[]
                    {
                        ownerTypeInfo,
                        propertyType,
                        propertyInfo,
                        serializerOption
                    }) as BinaryMemberInfo;
                }

                return binaryPropertyInfo;
            }

            internal static BinaryFieldInfo<TMember> CreateFieldInfo<TOwner, TMember>(BinaryTypeInfo binaryTypeInfo, Type fieldType, FieldInfo fieldInfo, SerializerOption serializerOption)
            {
                BinaryFieldInfo<TMember> binaryFieldInfo = new BinaryFieldInfo<TMember>(binaryTypeInfo, fieldType, fieldInfo, serializerOption);
                var (getAction, setAction) = DefaultMetaModule.AccessorsProvider.GetField<TOwner, TMember>(fieldInfo);

                binaryFieldInfo.GetAction = (a) => getAction.Invoke((TOwner)a);
                binaryFieldInfo.SetAction = (a, b) => setAction.Invoke((TOwner)a, b);

                return binaryFieldInfo;
            }

            internal static BinaryPropertyInfo<TMember> CreatePropertyInfo<TOwner, TMember>(BinaryTypeInfo binaryTypeInfo, Type propertyType, PropertyInfo propertyInfo, SerializerOption serializerOption)
            {
                BinaryPropertyInfo<TMember> binaryPropertyInfo = new BinaryPropertyInfo<TMember>(binaryTypeInfo, propertyType, propertyInfo, serializerOption);
                var (getAction, setAction) = DefaultMetaModule.AccessorsProvider.GetPropertyAccessors<TOwner, TMember>(propertyInfo);

                binaryPropertyInfo.GetAction = (a) => getAction.Invoke((TOwner)a);
                binaryPropertyInfo.SetAction = (a, b) => setAction.Invoke((TOwner)a, b);

                return binaryPropertyInfo;
            }
        }
    }
}