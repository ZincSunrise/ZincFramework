using System;
using System.Reflection;
using ZincFramework.Binary.Serialization.MetaModule;


namespace ZincFramework.Binary.Serialization
{
    namespace Metadata
    {
        internal class BinaryFieldInfo<T> : BinaryMemberInfo
        {
            public new Func<object, T> GetAction { get; set; }

            public new Action<object, T> SetAction { get; set; }

            public BinaryFieldInfo(int ordinalNumber, BinaryTypeInfo onwerType, MemberInfo memberInfo, SerializerOption serializerOption) : base(ordinalNumber, onwerType, memberInfo, serializerOption)
            {
            }

            public FieldInfo FieldInfo => MemberInfo as FieldInfo;

            public override void GetAsObjectAndWrite(object obj, ByteWriter byteWriter, SerializerOption serializerOption)
            {
                throw new NotImplementedException();
            }

            public override void ReadAndSetAsObject(object obj, ref ByteReader byteReader, SerializerOption serializerOption)
            {
                throw new NotImplementedException();
            }

            internal override void ConfigureConverter()
            {
                throw new NotImplementedException();
            }

            internal override void GetAccessorDelegates()
            {
                var (getter, setter) = DefaultMetaModule.AccessorsProvider.GetField<object, T>(FieldInfo);
                SetGetter(getter);
                SetSetter(setter);
            }

            private protected override void SetGetter(Delegate getter)
            {
                throw new NotImplementedException();
            }

            private protected override void SetSetter(Delegate setter)
            {
                throw new NotImplementedException();
            }
        }
    }
}