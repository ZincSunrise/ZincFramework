using System;


namespace ZincFramework.Serialization
{
    /// <summary>
    /// ����ָʾ����װ������ͨ������Ϊ�������ͱ����л�
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AccurateType : Attribute
    {
        public Type Type { get; }

        public AccurateType(Type type) => Type = type;
    }
}