using System;
using System.Collections.Generic;
using ZincFramework.Binary;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Excel
        {
            public static class ExcelDeserializer
            {
                public static TData Deserialize<TData, TKey, TValue>(byte[] bytes, Func<TData> dataFactory, Func<TValue> infoFactory) where TData : IExcelData where TValue : IConvertable
                {
                    TData container = dataFactory.Invoke();
                    int nowIndex = 0;
                    short count = ByteConverter.ToInt16(bytes, ref nowIndex);

                    Type keyType = typeof(TKey);

                    Dictionary<TKey, TValue> values = container.Collection as Dictionary<TKey, TValue>;
                    for (int i = 0; i < count; i++)
                    {
                        TValue value = infoFactory.Invoke();
                        if (keyType == typeof(int))
                        {
                            (values as Dictionary<int, TValue>).Add(ByteConverter.ToInt32(bytes, ref nowIndex), value);
                        }

                        value.Convert(bytes, ref nowIndex);
                    }


                    return container;
                }


                public static TData Deserialize<TData, TInfo>(byte[] bytes, Func<TData> dataFactory, Func<TInfo> infoFactory) where TData : IExcelData where TInfo : IConvertable
                {
                    TData container = dataFactory.Invoke();
                    List<TInfo> infos = container.Collection as List<TInfo>;

                    int nowIndex = 0;
                    short count = ByteConverter.ToInt16(bytes, ref nowIndex);
                    TInfo convert;

                    for (int i = 0; i < count; i++) 
                    {
                        convert = infoFactory.Invoke();
                        convert.Convert(bytes, ref nowIndex);
                        infos.Add(convert);
                    }

                    return container;
                }


                public static TData Deserialize<TData, TKey, TValue>(byte[] bytes) where TData : IExcelData, new() where TValue : IConvertable, new()
                {
                    return Deserialize<TData, TKey, TValue>(bytes, () => new TData(), () => new TValue());
                }


                public static TData Deserialize<TData, TInfo>(byte[] bytes) where TData : IExcelData, new() where TInfo : IConvertable, new()
                {
                    return Deserialize(bytes, () => new TData(), () => new TInfo());
                }
            }
        }
    }
}

