using System;
using System.Reflection;



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
        }
    }
}
