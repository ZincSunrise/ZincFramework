using System.IO;
using System.Text;
using UnityEngine;
using ZincFramework.Binary;
using ZincFramework.Serialization.Excel;



namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelBinaryReader
        {
            private readonly static StringBuilder _filePath = new StringBuilder();


            ///<typeparam name = "T">ÈÝÆ÷ÀàÃû</typeparam>
            public static T LoadData<T>(string name) where T : class
            {
                _filePath.Append(Path.Combine(FrameworkPaths.ProfilePath, name + BinaryDataManager.Extension));

                T container;
                using (FileStream fs = File.Open(_filePath.ToString(), FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    container = ExcelDeserializer.Deserialize(typeof(T), buffer) as T;
                }

                _filePath.Clear();
                return container;
            }


            public static T ReadBytes<T>(TextAsset textAsset) where T : class
            {
                return ExcelDeserializer.Deserialize(typeof(T), textAsset.bytes) as T;
            }
        }
    }
}

