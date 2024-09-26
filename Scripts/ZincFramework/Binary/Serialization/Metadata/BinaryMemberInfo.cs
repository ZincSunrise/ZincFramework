using System;
using System.Reflection;




namespace ZincFramework.Binary.Serialization
{
    namespace Metadata
    {
        public abstract class BinaryMemberInfo
        {
            public BinaryTypeInfo OnwerTypeInfo { get; }

            public BinaryConverter Converter { get; protected set; } 

            public BinaryConverter CustomConverter { get; set; }

            public MemberInfo MemberInfo { get;  }

            public int OrdinalNumber { get; protected set; }

            public virtual Type MemberType { get; protected set; }

            public Func<object, object> GetAction
            {
                get => _untypedGet;
                set
                {
                    SetGetter(value);
                }
            }

            public Action<object, object> SetAction
            {
                get => _untypedSet;
                set
                {
                    SetSetter(value);
                }
            }

            private protected Func<object, object> _untypedGet;

            private protected Action<object, object> _untypedSet;


            protected BinaryTypeInfo _binaryTypeInfo;


            protected SerializerOption _serializeOption;

            public BinaryMemberInfo(BinaryTypeInfo onwerType, MemberInfo memberInfo, SerializerOption serializerOption)
            {
                OnwerTypeInfo = onwerType;
                MemberInfo = memberInfo;
                _serializeOption = serializerOption;
            }

            internal abstract void ConfigureConverter(int ordinalNumber);

            public abstract void GetAsObjectAndWrite(object obj, ByteWriter byteWriter, SerializerOption serializerOption);

            public abstract void ReadAndSetAsObject(object obj, ref ByteReader byteReader, SerializerOption serializerOption);

            internal abstract void GetAccessorDelegates();

            private protected abstract void SetGetter(Delegate getter);

            private protected abstract void SetSetter(Delegate setter);
        }
    }
} 