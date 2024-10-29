using System;

namespace ZincFramework.Serialization
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class BinaryConstructor : BinaryAttribute
    {
        public int Weight { get; }
        public BinaryConstructor(int weight) 
        {
            Weight = weight;
        }
    }
}