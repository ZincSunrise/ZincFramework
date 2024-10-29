using System.Text;


namespace ZincFramework
{
    namespace Binary
    {
        namespace Serialization
        {
            public class BinaryWriterOption
            {
                public Encoding Encoding { get; set; } = Encoding.Unicode;

                public bool IsUsingVariant { get; set; } = true;

                public int DefaultBufferSize { get; set; } = 1024;

                public BinaryWriterOption() 
                {

                }
            }
        }
    }
}

