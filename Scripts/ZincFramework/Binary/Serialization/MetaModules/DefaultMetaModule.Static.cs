using System;
using System.Reflection;
using ZincFramework.Serialization;
using ZincFramework.Binary.Serialization.Metadata;


namespace ZincFramework.Binary.Serialization.MetaModule
{
    internal partial class DefaultMetaModule : IMetaModule
    {
        public static IAccessorsProvider AccessorsProvider
        {
            get
            {

                _accessorsProvider ??= Initailize();
                return _accessorsProvider;

                IAccessorsProvider Initailize()
                {
#if ENABLE_IL2CPP
                    return new ReflectionProvider();
#else
                    return new EmitAccessorsProvider();
#endif
                }
            }
        }

        private static IAccessorsProvider _accessorsProvider;

        private static BinaryTypeInfo CreateTypeInfoCore(Type type, BinaryConverter binaryConverter, SerializerOption serializerOption)
        {
            BinaryTypeInfo binaryTypeInfo = BinaryTypeInfo.CreateTypeInfo(type, binaryConverter, serializerOption);

            ConfigTypeInfo(binaryTypeInfo, serializerOption);
            return binaryTypeInfo;
        }

        private static BinaryTypeInfo CreateTypeInfoCore<T>(Func<T> factory, BinaryConverter binaryConverter, SerializerOption serializerOption)
        {
            BinaryTypeInfo binaryTypeInfo = factory != null ?
                BinaryTypeInfo.CreateTypeInfo<T>(factory, binaryConverter, serializerOption) : 
                BinaryTypeInfo.CreateTypeInfo<T>(binaryConverter, serializerOption);

            ConfigTypeInfo(binaryTypeInfo, serializerOption);
            return binaryTypeInfo;
        }

        private static void ConfigTypeInfo(BinaryTypeInfo binaryTypeInfo, SerializerOption serializerOption)
        {
            Type type = binaryTypeInfo.CacheType;
            if (binaryTypeInfo.TypeValueKind == TypeValueKind.Object)
            {
                if (binaryTypeInfo.UnTypedConstructor == null)
                {
                    binaryTypeInfo.SetConstructor(AccessorsProvider.GetConstructor(type));
                }
                
                PopulateProperty(binaryTypeInfo, serializerOption);
            }
            else if (binaryTypeInfo.TypeValueKind == TypeValueKind.Dictionary)
            {
                Type[] genericTypes = type.GetGenericArguments();
                binaryTypeInfo.KeyTypeInfo = serializerOption.GetTypeInfo(genericTypes[0]);
                binaryTypeInfo.ValueTypeInfo = serializerOption.GetTypeInfo(genericTypes[1]);
            }
            else if (binaryTypeInfo.TypeValueKind == TypeValueKind.Enumerable)
            {
                Type genericType = !type.IsArray ? type.GetGenericArguments()[0] : type.GetElementType();
                binaryTypeInfo.ElementTypeInfo = serializerOption.GetTypeInfo(genericType);
            }
        }

        private static void PopulateProperty(BinaryTypeInfo binaryTypeInfo, SerializerOption serializerOption)
        {
            BindingFlags allMemProperty = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;

            Type type = binaryTypeInfo.CacheType;
            if(type.IsPrimitive || type == typeof(object))
            {
                throw new ArgumentException("值类型和Object不能出现在序列化缓存中");
            }

            PropertyInfo[] propertyInfos = type.GetProperties(allMemProperty);

            for (int i = 0; i < propertyInfos.Length; i++) 
            {
                bool isAccurate = false;
                if (propertyInfos[i].PropertyType.IsAbstract || propertyInfos[i].PropertyType.IsInterface)
                {
                    if (propertyInfos[i].IsDefined(typeof(AccurateType)))
                    {
                        isAccurate = true;
                    }
                    else
                    {
                        throw new NotSupportedException($"不支持对未显式指定序列化类型的接口类型和抽象类型的序列化,类型为{type.Name}");
                    }
                }

                if (propertyInfos[i].GetMethod?.IsPublic == true ||
                    propertyInfos[i].SetMethod?.IsPublic == true ||
                    propertyInfos[i].IsDefined(typeof(BinaryInclude)))
                {
                    Type propertyType = isAccurate ? propertyInfos[i].GetCustomAttribute<AccurateType>().Type : propertyInfos[i].PropertyType;
                    BinaryMemberInfo binaryMemberInfo = CreateMemberInfo(binaryTypeInfo, propertyType, propertyInfos[i], serializerOption);

                    if (!propertyInfos[i].IsDefined(typeof(BinaryIgnore)))
                    {
                        //创建Get和Set函数
                        binaryMemberInfo.GetAccessorDelegates();
                        //创建自己的转换器

                        int ordinalNumber = serializerOption.IsGiveupOrdinal ? binaryTypeInfo.MemberCount : propertyInfos[i].GetCustomAttribute<BinaryOrdinal>().OrdinalNumber;
                        binaryMemberInfo.ConfigureConverter(ordinalNumber);
                        binaryTypeInfo.AddUsingMember(binaryMemberInfo);
                    }
                    else
                    {
                        binaryTypeInfo.AddIgnoreMember(binaryMemberInfo);
                    }
                }
            }


            if (serializerOption.IsIncludeField)
            {
                FieldInfo[] fieldInfos = type.GetFields(allMemProperty);
                for (int i = 0; i < fieldInfos.Length; i++) 
                {
                    bool isAccurate = false;
                    if (fieldInfos[i].FieldType.IsAbstract || fieldInfos[i].FieldType.IsInterface)
                    {
                        if (fieldInfos[i].IsDefined(typeof(AccurateType)))
                        {
                            isAccurate = true;
                        }
                        else
                        {
                            throw new NotSupportedException($"不支持对未显式指定序列化类型的接口类型和抽象类型的序列化,类型为{type.Name}");
                        }                
                    }

                    Type fieldType = isAccurate ? fieldInfos[i].GetCustomAttribute<AccurateType>().Type : fieldInfos[i].FieldType;
                    BinaryMemberInfo binaryFieldInfo = CreateMemberInfo(binaryTypeInfo, fieldType, fieldInfos[i], serializerOption);

                    if (fieldInfos[i].IsPublic ||
                        fieldInfos[i].IsDefined(typeof(BinaryInclude)))
                    {
                        if (!fieldInfos[i].IsDefined(typeof(BinaryIgnore)))
                        {
                            binaryFieldInfo.GetAccessorDelegates();
                            int ordinalNumber = serializerOption.IsGiveupOrdinal ? binaryTypeInfo.MemberCount : fieldInfos[i].GetCustomAttribute<BinaryOrdinal>().OrdinalNumber;
                            binaryFieldInfo.ConfigureConverter(ordinalNumber);
                            binaryTypeInfo.AddUsingMember(binaryFieldInfo);
                        }
                        else
                        {
                            binaryTypeInfo.AddIgnoreMember(binaryFieldInfo);
                        }
                    }
                }
            }
        }

        private static BinaryMemberInfo CreateMemberInfo(BinaryTypeInfo binaryTypeInfo, Type convertType, MemberInfo memberInfo, SerializerOption serializerOption)
        {
            BinaryMemberInfo binaryMemberInfo = binaryTypeInfo.GetMemberInfoReflection(memberInfo, convertType, serializerOption);

            BinaryConverterAttribute binaryConverterAttribute = memberInfo.GetCustomAttribute<BinaryConverterAttribute>();
            if (binaryConverterAttribute != null)
            {
                binaryMemberInfo.CustomConverter = binaryConverterAttribute.GetConverter();
            }

            return binaryMemberInfo;
        }
    }
}


