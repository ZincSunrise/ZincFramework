using System;


namespace ZincFramework.Serialization
{
    /// <summary>
    /// 用于指示父类装子类下通常是作为哪种类型被序列化
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AccurateType : Attribute
    {
        public Type Type { get; }

        public AccurateType(Type type) => Type = type;
    }
}