using System;
using System.Reflection;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Cache
        {
            public static class ConfigurationFactory 
            {

                private readonly static Type _dontSerialize = typeof(DontSerialize);
                private readonly static Type _mustSerialize = typeof(MustSerialize);
                private readonly static Type _obsolete = typeof(MustSerialize);

                public static bool TryGetMemberConfig(FieldInfo fieldInfo, out MemberConfig memberConfig)
                {
                    if (fieldInfo.IsDefined(_dontSerialize, true))
                    {
                        memberConfig = null;
                        return false;
                    }

                    var attributes = fieldInfo.GetCustomAttribute<SerializeOrdinal>(true);
                    memberConfig = new FieldConfig(fieldInfo, attributes.OrdinalNumber, fieldInfo.IsDefined(_obsolete, true));
                    return true;
                }


                public static bool TryGetMemberConfig(PropertyInfo propertyInfo, out MemberConfig memberConfig)
                {
                    if (propertyInfo.IsDefined(_dontSerialize, true))
                    {
                        memberConfig = null;
                        return false;
                    }

                    var attributes = propertyInfo.GetCustomAttribute<SerializeOrdinal>(true);
                    memberConfig = GetPropertyConfig(propertyInfo, attributes.OrdinalNumber, propertyInfo.IsDefined(_obsolete, true));
                    return true;
                }


                public static bool TryGetPrivateMemberConfig(FieldInfo fieldInfo, out MemberConfig memberConfig)
                {
                    if (!fieldInfo.IsDefined(_mustSerialize, true))
                    {
                        memberConfig = null;
                        return false;
                    }

                    var attributes = fieldInfo.GetCustomAttribute<SerializeOrdinal>(false);
                    memberConfig = new FieldConfig(fieldInfo, attributes.OrdinalNumber, fieldInfo.IsDefined(_obsolete, true));
                    return true;
                }

                public static bool TryGetPrivateMemberConfig(PropertyInfo propertyInfo, out MemberConfig memberConfig)
                {
                    if (!propertyInfo.IsDefined(_mustSerialize, true))
                    {
                        memberConfig = null;
                        return false;
                    }

                    var attributes = propertyInfo.GetCustomAttribute<SerializeOrdinal>(false);
                    memberConfig = GetPropertyConfig(propertyInfo, attributes.OrdinalNumber, propertyInfo.IsDefined(_obsolete, true));
                    return true;
                }

                private static MemberConfig GetPropertyConfig(PropertyInfo propertyInfo, int ordinalNumber, bool isObsolete)
                {
                    Type propertyType = propertyInfo.PropertyType;

                    if (propertyType.IsPrimitive)
                    {
                        return propertyType switch
                        {
                            not null when propertyType == typeof(int) => new PropertyConfig<int>(propertyInfo, ordinalNumber, isObsolete),
                            not null when propertyType == typeof(float) => new PropertyConfig<float>(propertyInfo, ordinalNumber, isObsolete),
                            not null when propertyType == typeof(bool) => new PropertyConfig<bool>(propertyInfo, ordinalNumber, isObsolete),
                            not null when propertyType == typeof(double) => new PropertyConfig<double>(propertyInfo, ordinalNumber, isObsolete),
                            not null when propertyType == typeof(long) => new PropertyConfig<long>(propertyInfo, ordinalNumber, isObsolete),
                            not null when propertyType == typeof(short) => new PropertyConfig<short>(propertyInfo, ordinalNumber, isObsolete),
                            not null when propertyType == typeof(ushort) => new PropertyConfig<ushort>(propertyInfo, ordinalNumber, isObsolete),
                            not null when propertyType == typeof(uint) => new PropertyConfig<uint>(propertyInfo, ordinalNumber, isObsolete),
                            not null when propertyType == typeof(ulong) => new PropertyConfig<ulong>(propertyInfo, ordinalNumber, isObsolete),
                            not null when propertyType == typeof(byte) => new PropertyConfig<byte>(propertyInfo, ordinalNumber, isObsolete),
                            not null when propertyType == typeof(sbyte) => new PropertyConfig<sbyte>(propertyInfo, ordinalNumber, isObsolete),
                            _ => null
                        };
                    }
                    else if (propertyType == typeof(string)) 
                    {
                        return new PropertyConfig<string>(propertyInfo, ordinalNumber, isObsolete);
                    }

                    return new PropertyConfig(propertyInfo, ordinalNumber, isObsolete);
                }
            }
        }
    }
}