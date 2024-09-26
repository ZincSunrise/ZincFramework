using System;
using System.IO;
using System.Text;
using UnityEngine;
using ZincFramework.Binary;
using ZincFramework.Serialization;
using ZincFramework.Binary.Excel;


namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelBinaryReader
        {
            private readonly static StringBuilder _filePath = new StringBuilder();


            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="TData">容器类名</typeparam>
            /// <typeparam name="TInfo">数据类名</typeparam>
            /// <param name="name"></param>
            /// <param name="dataFactory"></param>
            /// <param name="infoFactory"></param>
            /// <returns></returns>
            public static TData LoadListData<TData, TInfo>(string name, Func<TData> dataFactory, Func<TInfo> infoFactory) where TData : class, IExcelData where TInfo : ISerializable
            {
                _filePath.Append(Path.Combine(FrameworkPaths.ProfilePath, name + BinaryDataManager.Extension));

                TData container;
                using (FileStream fs = File.Open(_filePath.ToString(), FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    container = ExcelDeserializer.Deserialize(buffer, dataFactory, infoFactory);
                }

                _filePath.Clear();
                return container;
            }

            public static TData LoadListData<TData, TInfo>(string name) where TData : class, IExcelData, new() where TInfo : ISerializable, new()
            {
                return LoadListData(name, () => new TData(), () => new TInfo());
            }

            public static TData LoadListData<TData, TInfo>(TextAsset textAsset, Func<TData> dataFactory, Func<TInfo> infoFactory) where TData : class, IExcelData where TInfo : ISerializable
            {
                return ExcelDeserializer.Deserialize(textAsset.bytes, dataFactory, infoFactory);
            }

            public static TData LoadListData<TData, TInfo>(TextAsset textAsset) where TData : class, IExcelData, new() where TInfo : ISerializable, new()
            {
                return LoadListData(textAsset, () => new TData(), () => new TInfo());
            }

            public static TData LoadListData<TData, TInfo>(byte[] bytes, Func<TData> dataFactory, Func<TInfo> infoFactory) where TData : class, IExcelData where TInfo : ISerializable
            {
                return ExcelDeserializer.Deserialize(bytes, dataFactory, infoFactory);
            }

            public static TData LoadListData<TData, TInfo>(byte[] bytes) where TData : class, IExcelData, new() where TInfo : ISerializable, new()
            {
                return LoadListData(bytes, () => new TData(), () => new TInfo());
            }


            public static TData LoadDictionaryData<TData, TKey, TValue>(string name, Func<TData> dataFactory, Func<TValue> infoFactory) where TData : class, IExcelData where TValue : ISerializable
            {
                _filePath.Append(Path.Combine(FrameworkPaths.ProfilePath, name + BinaryDataManager.Extension));

                TData container;
                using (FileStream fs = File.Open(_filePath.ToString(), FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    container = ExcelDeserializer.Deserialize<TData, TKey, TValue>(buffer, dataFactory, infoFactory);
                }

                _filePath.Clear();
                return container;
            }

            public static TData LoadDictionaryData<TData, TKey, TValue>(string name) where TData : class, IExcelData, new() where TValue : ISerializable, new()
            {
                return LoadDictionaryData<TData, TKey, TValue>(name, () => new TData(), () => new TValue());
            }


            public static TData LoadDictionaryData<TData, TKey, TValue>(TextAsset textAsset, Func<TData> dataFactory, Func<TValue> infoFactory) where TData : class, IExcelData where TValue : ISerializable
            {
                return ExcelDeserializer.Deserialize<TData, TKey, TValue>(textAsset.bytes, dataFactory, infoFactory);
            }

            public static TData LoadDictionaryData<TData, TKey, TValue>(TextAsset textAsset) where TData : class, IExcelData, new() where TValue : ISerializable, new()
            {
                return LoadDictionaryData<TData, TKey, TValue>(textAsset, () => new TData(), () => new TValue());
            }


            public static TData LoadDictionaryData<TData, TKey, TValue>(byte[] bytes, Func<TData> dataFactory, Func<TValue> infoFactory) where TData : class, IExcelData where TValue : ISerializable
            {
                return ExcelDeserializer.Deserialize<TData, TKey, TValue>(bytes, dataFactory, infoFactory);
            }

            public static TData LoadDictionaryData<TData, TKey, TValue>(byte[] bytes) where TData : class, IExcelData, new() where TValue : ISerializable, new()
            {
                return LoadDictionaryData<TData, TKey, TValue>(bytes, () => new TData(), () => new TValue());
            }
        }
    }
}

