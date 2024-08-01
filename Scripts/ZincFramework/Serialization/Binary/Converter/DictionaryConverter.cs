using System;
using System.Collections;
using ZincFramework.Binary;
using ZincFramework.Serialization.Factory;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct DictionaryConverter : ISerializeConvert
            {
                public object Convert(byte[] bytes, ref int nowIndex, Type type)
                {
                    int count = ByteConverter.ToInt16(bytes, ref nowIndex);

                    var constructor = EnumerableConverter.GetCapacityConstructorMap(type);
                    IDictionary dictionary = constructor.Invoke(count) as IDictionary;

                    Type[] types = type.GenericTypeArguments;
                    Type keyType = types[0];
                    Type valueType = types[1];

                    ISerializeConvert keyBuilder = ConverterFactory.Shared.CreateBuilder(keyType);
                    ISerializeConvert valueBulider = ConverterFactory.Shared.CreateBuilder(valueType);

                    for (int i = 0; i < count; i++)
                    {
                        dictionary.Add(keyBuilder.Convert(bytes, ref nowIndex, keyType), 
                            valueBulider.Convert(bytes, ref nowIndex, valueType));
                    }

                    return dictionary;
                }

                public object Convert(ref ReadOnlySpan<byte> bytes, Type type)
                {
                    int count = ByteConverter.ToInt16(ref bytes);

                    var constructor = EnumerableConverter.GetCapacityConstructorMap(type);
                    IDictionary dictionary = constructor.Invoke(count) as IDictionary;

                    Type[] types = type.GenericTypeArguments;
                    Type keyType = types[0];
                    Type valueType = types[1];

                    ISerializeConvert keyBuilder = ConverterFactory.Shared.CreateBuilder(keyType);
                    ISerializeConvert valueBulider = ConverterFactory.Shared.CreateBuilder(valueType);

                    for (int i = 0; i < count; i++)
                    {
                        dictionary.Add(keyBuilder.Convert(ref bytes, keyType), 
                            valueBulider.Convert(ref bytes, valueType));
                    }

                    return dictionary;
                }
            }   
        }
    }
}

