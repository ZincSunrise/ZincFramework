using System;


namespace ZincFramework
{
    namespace Serialization
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
        public class BinarySerializable : BinaryAttribute
        {
            public int SerializableCode { get; }

            public BinarySerializable() { }

            public BinarySerializable(int serializeCode)
            {
                SerializableCode = serializeCode;
            }
        }
    }
}
