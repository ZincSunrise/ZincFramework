using System;
using System.Collections.Generic;
using System.Reflection;
using ZincFramework.Serialization;


namespace ZincFramework.Binary.Serialization.Metadata
{
    public abstract partial class BinaryTypeInfo
    {
        protected readonly SerializerOption _serializerOption;

        public int MemberCount => _metaInfo.SerializableInfo.Count;

        //存储自身底下的所有字段和属性的类
        private class MetaInfo
        {
            public Dictionary<int, BinaryMemberInfo> SerializableInfo { get; } = new Dictionary<int, BinaryMemberInfo>();

            public List<BinaryMemberInfo> IngoredInfo { get; } = new List<BinaryMemberInfo>();

            public bool IsConstantStruct { get; private set; }

            public MetaInfo(Type type)
            {
                IsConstantStruct = type.IsDefined(typeof(SequentialStruct));

            }

            public void AddIgnore(BinaryMemberInfo binaryMemberInfo)
            {
                IngoredInfo.Add(binaryMemberInfo);
            }

            public void AddSerializable(int oridinal, BinaryMemberInfo binaryMemberInfo)
            {
                SerializableInfo.Add(oridinal, binaryMemberInfo);
            }
        }

        private readonly MetaInfo _metaInfo;

        public void AddIgnoreMember(BinaryMemberInfo binaryMemberInfo)
        {
            _metaInfo.IngoredInfo.Add(binaryMemberInfo);
        }

        public void AddUsingMember(BinaryMemberInfo binaryMemberInfo)
        {
            _metaInfo.SerializableInfo.Add(binaryMemberInfo.OrdinalNumber, binaryMemberInfo);
        }
    }
}

