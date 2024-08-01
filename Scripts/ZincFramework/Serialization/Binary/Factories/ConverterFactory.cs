using System;
using ZincFramework.Serialization.Binary;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Factory
        {
            public class ConverterFactory : BaseBuilderFactory<ISerializeConvert>
            {
                public static ConverterFactory Shared => _shared.Value;

                private static Lazy<ConverterFactory> _shared = new Lazy<ConverterFactory>(() => new ConverterFactory());

                static ConverterFactory()
                {
                    _shared.Value.AddBuilder((int)E_Builder_Type.PrimitiveValue, () => new PrimitiveConverter());
                    _shared.Value.AddBuilder((int)E_Builder_Type.Enumerable, () => new EnumerableConverter());
                    _shared.Value.AddBuilder((int)E_Builder_Type.OtherSystemValue, () => new OtherSystemValueConverter());
                    _shared.Value.AddBuilder((int)E_Builder_Type.OtherValue, () => new OtherValueConverter());
                }

                private ConverterFactory() { }
            }
        }
    }
}

