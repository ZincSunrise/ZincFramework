using System;


namespace ZincFramework
{
    namespace Serialization
    {
        public enum PaddingMode
        {
            Fill,
            CreateNew
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
        public class BinaryPadding : BinaryAttribute
        {
            public PaddingMode PaddingMode { get; }

            public BinaryPadding(PaddingMode paddingMode)
            {
                PaddingMode = paddingMode;
            }
        }
    }
}