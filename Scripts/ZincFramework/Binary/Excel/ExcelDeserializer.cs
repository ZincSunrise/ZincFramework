using System;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Binary.Serialization;
using ZincFramework.Binary.Serialization.Converters;
using ZincFramework.Serialization;


namespace ZincFramework
{
    namespace Binary
    {
        namespace Excel
        {
            public static class ExcelDeserializer
            {
                public static TData Deserialize<TData, TKey, TValue>(byte[] bytes, Func<TData> dataFactory, Func<TValue> infoFactory) where TData : IExcelData where TValue : ISerializable
                {
                    SerializerOption serializerOption = SerializerOption.Default;
                    TData container = dataFactory.Invoke();

                    ByteReader byteReader = new ByteReader(bytes.AsMemory(), serializerOption.GetReaderOption());
                    int count = byteReader.ReadInt32();

                    Dictionary<TKey, TValue> values = container.Collection as Dictionary<TKey, TValue>;
                    BinaryConverter<TKey> keyConverter = serializerOption.GetTypeInfo<TKey>().WrapperConverter;

                    for (int i = 0; i < count; i++)
                    {
                        TValue value = infoFactory.Invoke();
                        TKey key = keyConverter.Read(ref byteReader, serializerOption);
                        value.Read(ref byteReader);

                        values.Add(key, value);
                    }

                    return container;
                }


                public static TData Deserialize<TData, TInfo>(byte[] bytes, Func<TData> dataFactory, Func<TInfo> infoFactory) where TData : IExcelData where TInfo : ISerializable
                {
                    SerializerOption serializerOption = SerializerOption.Default;

                    TData container = dataFactory.Invoke();
                    List<TInfo> infos = container.Collection as List<TInfo>;

                    ByteReader byteReader = new ByteReader(bytes.AsMemory(), serializerOption.GetReaderOption());
                    int count = byteReader.ReadInt32();
                    TInfo convert;

                    for (int i = 0; i < count; i++) 
                    {
                        convert = infoFactory.Invoke();
                        convert.Read(ref byteReader);
                        infos.Add(convert);
                    }

                    return container;
                }


                public static TData Deserialize<TData, TKey, TValue>(byte[] bytes) where TData : IExcelData, new() where TValue : ISerializable, new()
                {
                    return Deserialize<TData, TKey, TValue>(bytes, () => new TData(), () => new TValue());
                }


                public static TData Deserialize<TData, TInfo>(byte[] bytes) where TData : IExcelData, new() where TInfo : ISerializable, new()
                {
                    return Deserialize(bytes, () => new TData(), () => new TInfo());
                }
            }
        }
    }
}

