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
                    throw new ArgumentException("��������кŲ��ܹ���int����Сֵ");
                }

                OrdinalNumber = ordinalNumber;
            }
        }
    }
}