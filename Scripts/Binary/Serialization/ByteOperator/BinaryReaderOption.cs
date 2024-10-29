using System.Text;

namespace ZincFramework
{
    namespace Binary
    {
        namespace Serialization
        {
            public struct BinaryReaderOption
            {
                public bool IsUsingVariant { get; set; }

                public Encoding Encoding { get; set; }
            }
        }
    }
}