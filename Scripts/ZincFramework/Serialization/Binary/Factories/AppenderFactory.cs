using System;
using ZincFramework.Serialization.Binary;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Factory
        {
            public class AppenderFactory : BaseBuilderFactory<ISerializeAppend>
            {
                public static AppenderFactory Shared => _shared.Value;

                private static Lazy<AppenderFactory> _shared = new Lazy<AppenderFactory>(() => new AppenderFactory());

                static AppenderFactory()
                {
                    Shared.AddBuilder((int)E_Builder_Type.PrimitiveValue, () => new PrimitiveAppender());
                    Shared.AddBuilder((int)E_Builder_Type.Enumerable, () => new EnumerableAppender());
                    Shared.AddBuilder((int)E_Builder_Type.OtherSystemValue, () => new OtherSystemValueAppender());
                    Shared.AddBuilder((int)E_Builder_Type.OtherValue, () => new OtherValueAppender());
                }

                private AppenderFactory() { }
            }
        }
    }
}