using System.Reflection;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Cache
        {
            internal class FieldConfig : MemberConfig
            {
                public FieldConfig(FieldInfo fieldInfo, int ordinalNumber, bool isObsolete) : base(fieldInfo, ordinalNumber, isObsolete)
                {

                }

                public override object GetValue(object obj)
                {
                    return (MemberInfo as FieldInfo).GetValue(obj);
                }

                public override void SetValue(object obj, object value)
                {
                    (MemberInfo as FieldInfo).SetValue(obj, value);
                }
            }
        }
    }
}