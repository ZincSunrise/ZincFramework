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
                    throw new ArgumentException("��������кŲ��ܹ���int����Сֵ");
                }

                OrdinalNumber = ordinalNumber;
            }
        }
    }
}