using System;


namespace ZincFramework
{
    namespace Serialization
    {
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
        public class ObsoleteElement : Attribute
        {

        }
    }
}