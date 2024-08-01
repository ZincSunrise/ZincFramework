using System;
using System.Reflection;
using ZincFramework.Serialization.Runtime;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Cache
        {
            internal class PropertyConfig : MemberConfig
            {
                private readonly Action<object, object> _setAction;
                private readonly Func<object, object> _getAction;

                public PropertyConfig(PropertyInfo propertyInfo, int ordinalNumber, bool isObsolete) : base(propertyInfo, ordinalNumber, isObsolete)
                {
                    _getAction = EmitTool.GetPropertyGetMethod(propertyInfo);
                    _setAction = EmitTool.GetPropertySetMethod(propertyInfo);
                }

                public override object GetValue(object obj) => _getAction?.Invoke(obj);

                public override void SetValue(object obj, object value) => _setAction?.Invoke(obj, value);
            }
        }
    }
}
