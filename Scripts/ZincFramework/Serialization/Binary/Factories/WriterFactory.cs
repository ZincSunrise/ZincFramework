using System;
using ZincFramework.Serialization.Binary;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Factory
        {
            public class WriterFactory : BaseBuilderFactory<ISerializeWrite>
            {

                public static WriterFactory Shared => _shared.Value;

                private static Lazy<WriterFactory> _shared = new Lazy<WriterFactory>(() => new WriterFactory());

                static WriterFactory()
                {
                    Shared.AddBuilder((int)E_Builder_Type.PrimitiveValue, () => new PrimitiveWriter());
                    Shared.AddBuilder((int)E_Builder_Type.Enumerable, () => new EnumerableWriter());
                    Shared.AddBuilder((int)E_Builder_Type.OtherSystemValue, () => new OtherSystemValueWriter());
                    Shared.AddBuilder((int)E_Builder_Type.OtherValue, () => new OtherValueWriter());
                }

                private WriterFactory() { }
            }
        }
    }
}

