using System;


namespace ZincFramework
{
    namespace Serialization
    {
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class BinaryIgnore : BinaryAttribute
        {

        }
    }
}