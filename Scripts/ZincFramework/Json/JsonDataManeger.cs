using System;
using System.IO;
using UnityEngine;
using System.Text;
using ZincFramework.Json.Serialization;


namespace ZincFramework
{
    namespace Json
    {
        public enum JsonSerializeType
        {
            JsonSerializer,
            NewtonSoft,
            JsonUtility
        }

        public class JsonDataManager : BaseSafeSingleton<JsonDataManager>
        {
            public const string Extension = ".json";

            private JsonDataManager() { }

            private readonly StringBuilder _pathBulider = new StringBuilder(FrameworkPaths.SavePath, FrameworkPaths.SavePath.Length * 2);

            public void SaveData(object data, string saveName, Type type, JsonSerializeType jsonType = JsonSerializeType.JsonSerializer)
            {
                AppendPath(saveName);
                using (FileStream fileStream = File.Create(_pathBulider.ToString(), 1024))
                {
                    JsonSelector.Serialize(data, type, fileStream, jsonType);
                }

                RestorePath();
            }


            public object LoadData(string saveName, Type type, JsonSerializeType jsonType = JsonSerializeType.JsonSerializer)
            {
                string jsonStr;
                AppendPath(saveName);

                string pathStr = _pathBulider.ToString();
                if (!File.Exists(pathStr))
                {
                    if (!File.Exists(Path.Combine(FrameworkPaths.SavePath, saveName + Extension)))
                    {
                        return Activator.CreateInstance(type);
                    }
                }

                jsonStr = File.ReadAllText(pathStr);
                RestorePath();
                return JsonSelector.Deserialize(jsonStr, type, jsonType);
            }

            public T LoadData<T>(string saveName, JsonSerializeType jsonType = JsonSerializeType.JsonSerializer) where T : class, new()
            {
                string jsonStr;
                AppendPath(saveName);

                string pathStr = _pathBulider.ToString();

                if (!File.Exists(pathStr))
                {
                    if (!File.Exists(Path.Combine(FrameworkPaths.SavePath, saveName + Extension)))
                    {
                        return new T();
                    }
                }

                jsonStr = File.ReadAllText(pathStr);
                RestorePath();

                return JsonSelector.Deserialize<T>(jsonStr, jsonType);
            }


            public T LoadScriptableObjectData<T>(string saveName) where T : ScriptableObject
            {
                string jsonStr;
                AppendPath(saveName);
                string pathStr = _pathBulider.ToString();

                if (!File.Exists(pathStr))
                {
                    return ScriptableObject.CreateInstance<T>();
                }

                jsonStr = File.ReadAllText(pathStr);
                return JsonSelector.DeserializeScriptableObject<T>(jsonStr);
            }

            private void AppendPath(string saveName)
            {
                _pathBulider.Append(saveName);
                _pathBulider.Append(Extension);
            }

            private void RestorePath()
            {
                _pathBulider.Remove(FrameworkPaths.SavePath.Length, _pathBulider.Length - FrameworkPaths.SavePath.Length);
            }
        }
    }   
}

