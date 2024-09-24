using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ZincFramework.Serialization;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;



namespace ZincFramework.Binary.Serialization
{
    namespace Metadata
    {
        public enum TypeValueKind
        {
            Values,
            Dictionary,
            Enumerable,
            Object
        }

        public abstract partial class BinaryTypeInfo
        {
            internal Dictionary<int, BinaryMemberInfo> MemberInfos => _metaInfo.SerializableInfo;

            internal List<BinaryMemberInfo> IgnoredInfos => _metaInfo.IngoredInfo;

            public Type CacheType { get; set; }

            public Func<object> UnTypedConstructor { get; set; }

            public int SerializableCode { get; set; }

            public BinaryConverter BinaryConverter { get; }

            public TypeValueKind TypeValueKind { get; }


            public BinaryTypeInfo KeyTypeInfo { get; set; }

            public BinaryTypeInfo ValueTypeInfo { get; set; }

            public BinaryTypeInfo ElementTypeInfo { get; set; }


            internal BinaryTypeInfo(Type cacheType, BinaryConverter binaryConverter, SerializerOption serializerOption)
            {
                CacheType = cacheType;
                BinaryConverter = binaryConverter;
                _serializerOption = serializerOption;
                _metaInfo = new MetaInfo(cacheType);

                TypeValueKind = binaryConverter.GetConvertStrategy() switch
                {
                    ConvertStrategy.Substantial or ConvertStrategy.SimpleValue or ConvertStrategy.Custom => TypeValueKind.Values,
                    ConvertStrategy.Enumerable => TypeValueKind.Enumerable,
                    ConvertStrategy.Dictionary => TypeValueKind.Dictionary,
                    _ => TypeValueKind.Object,
                };
            }

            internal BinaryTypeInfo(Func<object> factory, Type cacheType, BinaryConverter binaryConverter, SerializerOption serializerOption) : this(cacheType, binaryConverter, serializerOption)
            {
                UnTypedConstructor = factory;
            }

            public object CreateInstance()
            {
                return UnTypedConstructor?.Invoke();
            }

            internal abstract void SetConstructor(Delegate @delegates);

            public abstract void SerializeAsObject(object obj, Stream stream);

            public abstract void SerializeAsObject(object obj, ByteWriter byteWriter);

            public abstract ValueTask SerializeAsObjectAsync(object obj, Stream stream, CancellationToken cancellationToken);


            public abstract object DeserializeAsObject(Stream stream);

            public abstract object DeserializeAsObject(ref ByteReader byteReader);

            public abstract ValueTask<object> DeserializeAsObjectAsync(Stream stream, CancellationToken cancellationToken);

            /// <summary>
            /// 该函数是创建该类的返回值为memberType的属性
            /// </summary>
            /// <param name="propertyInfo"></param>
            /// <param name="serializerOption"></param>
            /// <returns></returns>
            internal BinaryMemberInfo GetMemberInfoReflection(MemberInfo memberInfo, Type memberType, SerializerOption serializerOption)
            {
                //此处观察函数缓存中是否存在该属性对象
                if (serializerOption.TryGetTypeInfo(memberType, out BinaryTypeInfo binaryTypeInfo))
                {
                    BinaryMemberInfo binaryMemberInfo = binaryTypeInfo.CreatePropertyInfoInternal(memberInfo, memberType, serializerOption)!;

                    Debug.Assert(binaryMemberInfo != null);
                    return binaryMemberInfo;
                }
                else
                {
                    //不存在就创建一个新的
                    return GetPropertyInfo(memberInfo, memberType, serializerOption);
                }
            }

            /// <summary>
            /// 该函数是创建该返回值为该类的属性
            /// </summary>
            /// <param name="propertyInfo"></param>
            /// <param name="serializerOption"></param>
            /// <returns></returns>
            private protected abstract BinaryMemberInfo CreatePropertyInfoInternal(MemberInfo memberInfo, Type memberType, SerializerOption serializerOption);

            private protected abstract BinaryMemberInfo GetPropertyInfo(MemberInfo memberInfo, Type memberType, SerializerOption serializerOption);
        }
    }
}

