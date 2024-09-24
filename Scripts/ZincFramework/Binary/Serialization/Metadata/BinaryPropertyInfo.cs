using System;
using System.Diagnostics;
using System.Reflection;
using ZincFramework.Serialization.Runtime;



namespace ZincFramework.Binary.Serialization
{
    namespace Metadata
    {
        public abstract class BinaryPropertyInfo : BinaryMemberInfo
        {
            protected BinaryPropertyInfo(BinaryTypeInfo onwerType, MemberInfo memberInfo, SerializerOption serializerOption) : base(onwerType, memberInfo, serializerOption)
            {

            }

            public PropertyInfo PropertyInfo => MemberInfo as PropertyInfo;

            public override Type MemberType => PropertyInfo.PropertyType;
        }
    }
}
