using System;
using System.IO;
using ZincFramework.Binary.Serialization;
using ZincFramework.Serialization;


namespace ZincFramework
{
    namespace Binary
    {
        public static class BinaryDataManager
        {
            //这是变量的扩展名
            public static string Extension { get; } = FrameworkConsole.Instance.SharedData.extension;


            public static object LoadData(string name, Type type, SerializerOption serializerOption = null)
            {
                string path = Path.Combine(FrameworkPaths.SavePath, name + Extension);

                if (!File.Exists(path))
                {
                    return Activator.CreateInstance(type);
                }

                using (FileStream fileStream = File.OpenRead(path))
                {
                    return BinarySerializer.Deserialize(fileStream, type, serializerOption);
                }
            }

            /// <summary>
            /// 要想加载此类必须要有默认无参构造函数
            /// </summary>
            /// <returns></returns>
            public static T LoadData<T>(string name, SerializerOption serializerOption = null) where T : new()
            {
                return LoadData(name, () => new T(), serializerOption);
            }

            public static T LoadData<T>(string name, Func<T> factory, SerializerOption serializerOption = null)
            {
                string path = Path.Combine(FrameworkPaths.SavePath, name + Extension);

                if (!File.Exists(path))
                {
                    return factory.Invoke();
                }

                using (FileStream fileStream = File.OpenRead(path))
                {
                    return BinarySerializer.Deserialize<T>(fileStream, factory, serializerOption);
                }
            }


            public static void SaveData(object data, Type type, string name, SerializerOption serializerOption = null)
            {
                LogUtility.Log(FrameworkPaths.SavePath);
                string path = Path.Combine(FrameworkPaths.SavePath, name + Extension);

                if (!Directory.Exists(FrameworkPaths.SavePath))
                {
                    Directory.CreateDirectory(FrameworkPaths.SavePath);
                }

                using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    BinarySerializer.Serialize(data, fileStream, type, serializerOption);
                }
            }

            /// <summary>
            /// 要想保存此类必须要拥有ZincSerializable特性
            /// </summary>
            /// <param name="data"></param>
            /// <param name="name"></param>
            public static void SaveData<T>(T data, string name, SerializerOption serializerOption = null)
            {
                LogUtility.Log(FrameworkPaths.SavePath);
                string path = Path.Combine(FrameworkPaths.SavePath, name + Extension);

                if (!Directory.Exists(FrameworkPaths.SavePath))
                {
                    Directory.CreateDirectory(FrameworkPaths.SavePath);
                }

                using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    BinarySerializer.Serialize<T>(data, fileStream, serializerOption);
                }
            }


            public static void DeleteData(string name)
            {
                string path = Path.Combine(FrameworkPaths.SavePath, name + Extension);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}

