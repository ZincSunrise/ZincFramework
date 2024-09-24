using System;


namespace ZincFramework
{
    namespace Serialization
    {
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class BinaryOrdinal : BinaryAttribute
        {
            public int OrdinalNumber { get; }

            public BinaryOrdinal(int ordinalNumber) 
            {
                if(ordinalNumber == int.MinValue)
                {
                    throw new ArgumentException("填入的序列号不能够是int的最小值");
                }

                OrdinalNumber = ordinalNumber;
            }
        }
    }
}