using System;
using System.Reflection;
using ZincFramework.Binary.Serialization.Metadata;
using ZincFramework.Serialization;



namespace ZincFramework.Binary.Serialization.MetaModule
{
    internal partial class DefaultMetaModule : IMetaModule
    {
        public static IAccessorsProvider AccessorsProvider
        {
            get
            {

                _accessorsProvider ??= Iniatilize();
                return _accessorsProvider;

                IAccessorsProvider Iniatilize()
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

            if (binaryTypeInfo.TypeValueKind == TypeValueKind.Object)
            {
                binaryTypeInfo.SetConstructor(AccessorsProvider.GetConstructor(type));
                PopulateProperty(binaryTypeInfo, serializerOption);
            }

            return binaryTypeInfo;
        }

        private static BinaryTypeInfo CreateTypeInfoCore<T>(Func<T> factory, BinaryConverter binaryConverter, SerializerOption serializerOption)
        {
            BinaryTypeInfo binaryTypeInfo = factory != null ?
                BinaryTypeInfo.CreateTypeInfo<T>(factory, binaryConverter, serializerOption) : 
                BinaryTypeInfo.CreateTypeInfo<T>(binaryConverter, serializerOption);


            if (binaryTypeInfo.TypeValueKind == TypeValueKind.Object)
            {
                binaryTypeInfo.SetConstructor(AccessorsProvider.GetConstructor(typeof(T)));
                PopulateProperty(binaryTypeInfo, serializerOption);
            }

            return binaryTypeInfo;
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
                if (propertyInfos[i].PropertyType.IsAbstract || propertyInfos[i].PropertyType.IsInterface)
                {
                    throw new NotSupportedException($"不支持对接口类型和抽象类型的序列化,类型为{type.Name}");
                }

                if (propertyInfos[i].GetMethod?.IsPublic == true ||
                    propertyInfos[i].SetMethod?.IsPublic == true ||
                    propertyInfos[i].IsDefined(typeof(BinaryInclude)))
                {
                    BinaryMemberInfo binaryMemberInfo = CreatePropertyInfo(binaryTypeInfo, propertyInfos[i].PropertyType, propertyInfos[i], serializerOption);

                    if (!propertyInfos[i].IsDefined(typeof(BinaryIgnore)))
                    {
                        //创建Get和Set函数
                        binaryMemberInfo.GetAccessorDelegates();
                        //创建自己的转换器
                        binaryMemberInfo.ConfigureConverter();

                        binaryTypeInfo.AddUsingProperty(binaryMemberInfo);
                    }
                    else
                    {
                        binaryTypeInfo.AddIgnoreProperty(binaryMemberInfo);
                    }
                }
            }


            if (serializerOption.IsIncludeField)
            {
                FieldInfo[] fieldInfos = type.GetFields(allMemProperty);
                for (int i = 0; i < fieldInfos.Length; i++) 
                {
                    if (fieldInfos[i].FieldType.IsAbstract || fieldInfos[i].FieldType.IsInterface)
                    {
                        throw new NotSupportedException($"不支持对接口类型和抽象类型的序列化,类型为{type.Name}");
                    }

                    if (fieldInfos[i].IsDefined(typeof(BinaryIgnore)))
                    {
                        continue;
                    }

                    if (fieldInfos[i].IsPublic ||
                        fieldInfos[i].IsDefined(typeof(BinaryInclude)))
                    {
                        BinaryMemberInfo fieldInfo = CreatePropertyInfo(binaryTypeInfo, propertyInfos[i].PropertyType, propertyInfos[i], serializerOption);
                        binaryTypeInfo.AddUsingProperty(fieldInfo);
                    }
                }
            }
        }

        private static BinaryMemberInfo CreatePropertyInfo(BinaryTypeInfo binaryTypeInfo, Type convertType, MemberInfo memberInfo, SerializerOption serializerOption)
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


