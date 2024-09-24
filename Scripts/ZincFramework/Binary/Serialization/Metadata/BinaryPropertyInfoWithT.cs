using System;
using System.Diagnostics;
using System.Reflection;
using ZincFramework.Binary.Serialization.Converters;
using ZincFramework.Binary.Serialization.MetaModule;



namespace ZincFramework.Binary.Serialization
{
    namespace Metadata
    {
        public class BinaryPropertyInfo<T> : BinaryPropertyInfo
        {
            private WrapperConverter<T> _wrapperConverter;

            public new Func<object, T> GetAction 
            { 
                get => _getAction; 
                set 
                { 
                    SetGetter(value);
                }
            }

            public new Action<object, T> SetAction 
            { 
                get => _setAction;
                set
                {
                    SetSetter(value);
                }
            }


            private Func<object, T> _getAction;

            private Action<object, T> _setAction;


            public BinaryPropertyInfo(BinaryTypeInfo onwerType, MemberInfo memberInfo, SerializerOption serializerOption) : base(onwerType, memberInfo, serializerOption)
            {
                _wrapperConverter = new WrapperConverter<T>(Converter);
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
                T value = _wrapperConverter.Convert(ref byteReader, serializerOption);
                SetAction.Invoke(obj, value);
            }

            internal override void GetAccessorDelegates()
            {
                var (getter, setter) = DefaultMetaModule.AccessorsProvider.GetPropertyAccessors<T>(PropertyInfo);
                SetGetter(getter);
                SetSetter(setter);
            }

            private protected override void SetGetter(Delegate getter)
            {
                Debug.Assert(getter is Func<object, T> || getter is Func<object, object>);

                if(getter is Func<object, T> typedGetter)
                {
                    _getAction = typedGetter;
                    _untypedGet = (a) => typedGetter.Invoke(a);
                }
                else if(getter is Func<object, object> unTypedGetter)
                {
                    _getAction = (a) => (T)unTypedGetter.Invoke(a);
                    _untypedGet = unTypedGetter;
                }
            }

            private protected override void SetSetter(Delegate setter)
            {
                Debug.Assert(setter is Action<object, T> || setter is Action<object, object>);

                if (setter is Action<object, T> typedSetter)
                {
                    _setAction = typedSetter;
                    _untypedSet = (a, b) => typedSetter.Invoke(a, (T)b);
                }
                else if (setter is Action<object, object> unTypedSetter)
                {
                    _setAction = (a, b) => unTypedSetter.Invoke(a, b);
                    _untypedSet = unTypedSetter;
                }
            }

            internal override void ConfigureConverter(int ordinalNumber)
            {
                OrdinalNumber = ordinalNumber;
                var typeInfo = _serializeOption.GetTypeInfo<T>();
                _binaryTypeInfo = typeInfo;

                Converter = typeInfo.BinaryConverter;
                _wrapperConverter = typeInfo.WrapperConverter;
            }
        }
    }
}
