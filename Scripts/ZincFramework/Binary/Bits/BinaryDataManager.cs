using System.IO;
using ZincFramework.Serialization;
using ZincFramework.Serialization.Binary;


namespace ZincFramework
{
    namespace Binary
    {
        public enum SaveConfig
        {
            Commom,
            Stream,
        }

        public static class BinaryDataManager
        {
            //这是变量的扩展名
            public static string Extension { get; } = FrameworkData.Shared.extension;

            /// <summary>
            /// 要想加载此类必须要有默认无参构造函数
            /// </summary>
            /// <returns></returns>
            public static T LoadData<T>(string name, SerializeConfig serializeConfig = SerializeConfig.Property) where T : new()
            {
                T data;
                string path = Path.Combine(FrameworkPaths.SavePath, name + Extension);

                if (!File.Exists(path)) 
                {
                    return new T();
                }

                using (FileStream fileStream = File.OpenRead(path))
                {
                    data = StreamSerializer.Deserialize<T>(fileStream, serializeConfig);
                }
                return data;
            }



            /// <summary>
            /// 要想保存此类必须要拥有ZincSerializable特性
            /// </summary>
            /// <param name="data"></param>
            /// <param name="name"></param>
            public static void SaveData<T>(T data, string name, SaveConfig saveConfig = SaveConfig.Commom, SerializeConfig serializeConfig = SerializeConfig.Property)
            {
                LogUtility.Log(FrameworkPaths.SavePath);
                string path = Path.Combine(FrameworkPaths.SavePath, name + Extension);
                if (!Directory.Exists(FrameworkPaths.SavePath))
                {
                    Directory.CreateDirectory(FrameworkPaths.SavePath);
                }

                using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    if (saveConfig == SaveConfig.Stream)
                    {
                        StreamSerializer.Serialize(fileStream, data, typeof(T), serializeConfig);
                        fileStream.Close();

                    }
                    else 
                    {
                        byte[] buffer = BinarySerializer.Serialize(data, typeof(T), serializeConfig);
                        fileStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
    }
}

