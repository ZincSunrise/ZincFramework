using System;


namespace ZincFramework
{
    namespace Serialization
    {
        public enum SerializeConfig
        {
            Field = 2,
            Property = 4,
            NonPublic = 8,
        }


        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
        public class ZincSerializable : Attribute
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
