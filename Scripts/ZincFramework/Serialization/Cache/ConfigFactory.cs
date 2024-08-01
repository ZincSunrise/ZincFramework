using System;
using System.Reflection;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Cache
        {
            internal static class ConfigFactory
            {
                private readonly static Type _dontSerialize = typeof(DontSerialize);
                private readonly static Type _mustSerialize = typeof(MustSerialize);
                private readonly static Type _obsolete = typeof(MustSerialize);

                public static bool TryGetMemberConfig(FieldInfo fieldInfo, out MemberConfig memberConfig)
                {
                    if(fieldInfo.IsDefined(_dontSerialize, true))
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
                    memberConfig = new PropertyConfig(propertyInfo, attributes.OrdinalNumber, propertyInfo.IsDefined(_obsolete, true));
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
                    memberConfig = new PropertyConfig(propertyInfo, attributes.OrdinalNumber, propertyInfo.IsDefined(_obsolete, true));
                    return true;
                }
            }
        }
    }
}
