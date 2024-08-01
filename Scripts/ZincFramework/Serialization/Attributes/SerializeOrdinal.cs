using System;


namespace ZincFramework
{
    namespace Serialization
    {
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
        public class SerializeOrdinal : Attribute
        {
            public int OrdinalNumber { get; }

            public SerializeOrdinal(int ordinalNumber) 
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