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
            //���Ǳ�������չ��
            public static string Extension { get; } = FrameworkData.Shared.extension;

            /// <summary>
            /// Ҫ����ش������Ҫ��Ĭ���޲ι��캯��
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
            /// Ҫ�뱣��������Ҫӵ��ZincSerializable����
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

