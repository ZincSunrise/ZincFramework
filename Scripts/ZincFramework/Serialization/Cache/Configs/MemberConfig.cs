using System;
using System.Reflection;




namespace ZincFramework
{
    namespace Serialization
    {
        namespace Cache
        {
            public abstract class MemberConfig
            {
                public MemberInfo MemberInfo { get; }

                public Type ConfigType { get; }

                public bool IsObsolete { get; } 

                public int OrdinalNumber { get; }

                public abstract object GetValue(object obj);

                public abstract void SetValue(object obj, object value);


                public MemberConfig(FieldInfo fieldInfo, int ordinalNumber, bool isObsolete)
                {
                    ConfigType = fieldInfo.FieldType;
                    MemberInfo = fieldInfo;
                    OrdinalNumber = ordinalNumber;
                    IsObsolete = isObsolete;
                }

                public MemberConfig(PropertyInfo propertyInfo, int ordinalNumber, bool isObsolete) 
                {
                    ConfigType = propertyInfo.PropertyType;
                    MemberInfo = propertyInfo;
                    OrdinalNumber = ordinalNumber;
                    IsObsolete = isObsolete;
                }


                public static implicit operator bool(MemberConfig memberConfig) 
                {
                    return memberConfig != null;
                }
            }
        }
    }
} 