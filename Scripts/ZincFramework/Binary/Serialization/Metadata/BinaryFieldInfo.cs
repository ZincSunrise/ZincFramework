using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using UnityEngine;
using ZincFramework.Binary.Serialization.Converters;
using ZincFramework.Binary.Serialization.MetaModule;


namespace ZincFramework.Binary.Serialization
{
    namespace Metadata
    {
        internal class BinaryFieldInfo<T> : BinaryMemberInfo
        {
            private WrapperConverter<T> _wrapperConverter;

            public new Func<object, T> GetAction 
            {
                get => _getAction;
                set => SetGetter(value);
            }

            public new Action<object, T> SetAction
            {
                get => _setAction;
                set => SetSetter(value);
            }

            private Func<object, T> _getAction;

            private Action<object, T> _setAction;

            public FieldInfo FieldInfo => MemberInfo as FieldInfo;

            public BinaryFieldInfo(BinaryTypeInfo ownerType, Type memberType, MemberInfo memberInfo, SerializerOption serializerOption) : base(ownerType, memberInfo, serializerOption)
            {
                MemberType = memberType;
            }


            public override void GetAsObjectAndWrite(object obj, ByteWriter byteWriter, SerializerOption serializerOption)
            {
                T value = GetAction.Invoke(obj);

                if (!(serializerOption.IsIgnoreNullValue && !MemberType.IsValueType && value == null))
                {
                    SimpleConverters.Int32Converter.Write(OrdinalNumber, byteWriter, serializerOption);

                    value ??= serializerOption.GetTypeInfo<T>().CreateInstance();
                    _wrapperConverter.Write(value, byteWriter, serializerOption);
                }
            }

            public override void ReadAndSetAsObject(object obj, ref ByteReader byteReader, SerializerOption serializerOption)
            {
                T value = _wrapperConverter.Read(ref byteReader, serializerOption);
                SetAction.Invoke(obj, value);
            }

            internal override void GetAccessorDelegates()
            {
                var (getter, setter) = DefaultMetaModule.AccessorsProvider.GetField<T>(FieldInfo);
                SetGetter(getter);
                SetSetter(setter);
            }

            private protected override void SetGetter(Delegate getter)
            {
                Contract.Assert(getter is Func<object, T> || getter is Func<object, object>);
                if (getter is Func<object, T> fuc) 
                {
                    _getAction = fuc;
                    _untypedGet = (a) => fuc.Invoke(a);
                }
                else if (getter is Func<object, object> fuc2)
                {
                    _getAction = (a) => (T)fuc2.Invoke(a);
                    _untypedGet = fuc2;
                }
            }

            private protected override void SetSetter(Delegate setter)
            {
                Contract.Assert(setter is Action<object, T> || setter is Action<object, object>);
                if (setter is Action<object, T> setter1)
                {
                    _setAction = setter1;
                    _untypedSet = (a, b) => setter1.Invoke(a, (T)b);
                }
                else if (setter is Action<object, object> setter2)
                {
                    _setAction = (a, b) => setter2.Invoke(a, b);
                    _untypedSet = setter2;
                }
            }

            internal override void ConfigureConverter(int ordinalNumber)
            {
                OrdinalNumber = ordinalNumber;
                var typeInfo = _serializeOption.GetTypeInfo(MemberType);

                _binaryTypeInfo = typeInfo;
                Converter = typeInfo.BinaryConverter;
                _wrapperConverter = new WrapperConverter<T>(typeInfo.BinaryConverter);
            }
        }
    }
}