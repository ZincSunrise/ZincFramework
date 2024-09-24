using System;
using System.Diagnostics;
using System.Reflection;
using ZincFramework.Binary.Serialization.Converters;
using ZincFramework.Serialization;



namespace ZincFramework.Binary.Serialization.Metadata
{
    public sealed partial class BinaryTypeInfo<T> : BinaryTypeInfo
    {
        public WrapperConverter<T> WrapperConverter { get; private set; }

        public Func<T> TypeConstructor { get; private set; }

        internal new T CreateInstance() => TypeConstructor.Invoke();

        public BinaryTypeInfo(BinaryConverter binaryConverter, SerializerOption serializerOption) : base(typeof(T), binaryConverter, serializerOption)
        {
            WrapperConverter = new WrapperConverter<T>(binaryConverter);
        }

        public BinaryTypeInfo(Func<T> factory, BinaryConverter binaryConverter, SerializerOption serializerOption) : base(() => factory, typeof(T), binaryConverter, serializerOption)
        {
            WrapperConverter = new WrapperConverter<T>(binaryConverter);
        }


        internal void GetProperiesAndWrite(T data, ByteWriter byteWriter)
        {
            foreach (var item in MemberInfos.Values)
            {
                item.GetAsObjectAndWrite(data, byteWriter, _serializerOption);
            }
        }

        internal void ReadAndSetProperties(T data, ref ByteReader byteReader)
        {
            for (int i = 0; i < MemberInfos.Count; i++) 
            {
                int oridinal = SimpleConverters.Int32Converter.Convert(ref byteReader, _serializerOption);

                if (!MemberInfos.TryGetValue(oridinal, out var member))
                {
                    throw new InvalidOperationException($"出现了不期望类型的码{oridinal}");
                }

                member.ReadAndSetAsObject(data, ref byteReader, _serializerOption);
            }
        }

        private protected override BinaryMemberInfo CreatePropertyInfoInternal(MemberInfo memberInfo, Type memberType, SerializerOption serializerOption)
        {
            if(memberInfo is PropertyInfo)
            {
                return new BinaryPropertyInfo<T>(this, memberInfo, serializerOption);
            }
            else if (memberInfo is FieldInfo)
            {
                return new BinaryFieldInfo<T>(this, memberInfo, serializerOption);
            }

            return null;
        }

        private protected override BinaryMemberInfo GetPropertyInfo(MemberInfo memberInfo, Type memberType, SerializerOption serializerOption)
        {
            if(memberInfo is PropertyInfo propertyInfo)
            {
                return MemberInfoCreator.GetPropertyMetaData<T>(this, propertyInfo, serializerOption);
            }
            else if(memberInfo is FieldInfo fieldInfo)
            {
                return MemberInfoCreator.GetFieldMetaData<T>(this, fieldInfo, serializerOption);
            }

            return null;         
        }

        internal override void SetConstructor(Delegate ctor)
        {
            if (ctor is Func<object> objectFac)
            {
                UnTypedConstructor = objectFac;
                TypeConstructor = () => (T)objectFac.Invoke();
            }
            else if (ctor is Func<T> tFuc)
            {
                UnTypedConstructor = () => tFuc.Invoke();
                TypeConstructor = tFuc;
            }
        }
    }
}