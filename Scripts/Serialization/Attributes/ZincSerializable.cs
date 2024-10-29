using System;


namespace ZincFramework
{
    namespace Serialization
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
        public class ZincSerializable : BinaryAttribute
        {
            public int SerializableCode { get; }

            public ZincSerializable() { }

            public ZincSerializable(int serializeCode)
            {
                SerializableCode = serializeCode;
            }
        }
    }
}
