using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Binary.Serialization.Converters;
using ZincFramework.Binary.Serialization.Factory;
using ZincFramework.Binary.Serialization.Metadata;



namespace ZincFramework.Binary.Serialization.MetaModule
{
    internal partial class DefaultMetaModule : IMetaModule
    {
        private static ConvertibleFactory[] _factories;


        private static Dictionary<Type, BinaryConverter> _converterMaps;

        internal static bool IsSubstantial(Type type) => type.IsValueType && (_converterMaps.ContainsKey(type) || typeof(IFormattable).IsAssignableFrom(type) || typeof(ISubstantiable).IsAssignableFrom(type));

        public BinaryConverter GetConverterFromType(Type type, SerializerOption serializerOption)
        {
            BinaryConverter binaryConverter = serializerOption.FindBinaryConverter(type);

            if (binaryConverter == null)
            {
                IntializeSimpleConverter();

                if (!_converterMaps.TryGetValue(type, out binaryConverter))
                {
                    CreateDefaultFactories();
                    for (int i = 0; i < _factories.Length; i++)
                    {
                        if (_factories[i].IsSerializable(type))
                        {
                            binaryConverter = _factories[i].GetConverter(type, serializerOption);
                            break;
                        }
                    }
                }
            }

            return binaryConverter;
        }

        public BinaryConverter GetConverterFromType<T>(SerializerOption serializerOption)
        {
            Type type = typeof(T);
            BinaryConverter binaryConverter = serializerOption.FindBinaryConverter(type);

            if (binaryConverter == null)
            {
                IntializeSimpleConverter();

                if (!_converterMaps.TryGetValue(type, out binaryConverter))
                {
                    CreateDefaultFactories();
                    for (int i = 0; i < _factories.Length; i++)
                    {
                        if (_factories[i].IsSerializable(type))
                        {
                            binaryConverter = _factories[i].GetConverter(typeof(T), serializerOption);
                            break;
                        }
                    }
                }
            }

            return binaryConverter!;
        }

        private static void IntializeSimpleConverter()
        {
            _converterMaps ??= new Dictionary<Type, BinaryConverter>()
            {                
                {typeof(sbyte), SimpleConverters.SByteConverter },
                {typeof(byte), SimpleConverters.ByteConverter },
                {typeof(short), SimpleConverters.Int16Converter },
                {typeof(ushort), SimpleConverters.UInt16Converter },
                {typeof(char), SimpleConverters.CharConverter },
                {typeof(bool), SimpleConverters.BooleanConverter },
                {typeof(int), SimpleConverters.Int32Converter },
                {typeof(uint), SimpleConverters.UInt32Converter },
                {typeof(float), SimpleConverters.SingleConverter },
                {typeof(long), SimpleConverters.Int64Converter },
                {typeof(ulong), SimpleConverters.UInt64Converter },
                {typeof(double), SimpleConverters.DoubleConverter },
                {typeof(DateTime), SimpleConverters.DateTimeConverter },
                {typeof(TimeSpan), SimpleConverters.TimeSpanConverter },
                {typeof(Uri), SimpleConverters.UriConverter},
                {typeof(string), SimpleConverters.StringConverter },
            };
        }

        private static void CreateDefaultFactories()
        {
            _factories ??= new ConvertibleFactory[]
            {
                new CustomFactory(),
                new EnumerableFactory(),
                new DictionaryFactory(),
                new SubstantialFactory(),
                new ObjectFactory(),
            };
        }
    }
}