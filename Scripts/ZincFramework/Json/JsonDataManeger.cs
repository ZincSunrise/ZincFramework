using UnityEngine;
using System;
using System.IO;
using ZincFramework.Serialization.Json;

namespace ZincFramework
{
    namespace Json
    {
        public enum JsonSerializeType
        {
            LitJson,
            JsonUtility
        }

        public class JsonDataManager : BaseSafeSingleton<JsonDataManager>
        {
            public const string Extension = ".json";

            private JsonDataManager() { }

            public void SaveData(object data, string name, JsonSerializeType jsonType = JsonSerializeType.LitJson)
            {
                string jsonStr = JsonSerializer.Serialize(data, jsonType);
                File.WriteAllText(FrameworkPaths.SavePath + name + Extension, jsonStr);
            }

            public object LoadData(string name, Type type, JsonSerializeType jsonType = JsonSerializeType.LitJson)
            {
                string jsonStr;
                if (!File.Exists(Path.Combine(FrameworkPaths.ProfilePath, name + Extension)))
                {
                    if (!File.Exists(Path.Combine(FrameworkPaths.SavePath, name + Extension)))
                    {
                        return Activator.CreateInstance(type);
                    }
                }
                jsonStr = File.ReadAllText(FrameworkPaths.SavePath + name + Extension);
                return JsonSerializer.Deserialize(jsonStr, type, jsonType);
            }

            public T LoadData<T>(string name, JsonSerializeType jsonType = JsonSerializeType.LitJson) where T : class, new()
            {
                string jsonStr;
                if (!File.Exists(Path.Combine(FrameworkPaths.ProfilePath, name + Extension)))
                {
                    if (!File.Exists(Path.Combine(FrameworkPaths.SavePath, name + Extension)))
                    {
                        return new T();
                    }
                }
                jsonStr = File.ReadAllText(FrameworkPaths.SavePath + name + Extension);
                return JsonSerializer.Deserialize(jsonStr, typeof(T), jsonType) as T;
            }

            public T LoadScriptableObjectData<T>(string name) where T : ScriptableObject
            {
                string jsonStr;
                if (!File.Exists(Path.Combine(FrameworkPaths.SavePath, name + Extension)))
                {
                    return default;
                }

                jsonStr = File.ReadAllText(FrameworkPaths.SavePath + name + Extension);
                return JsonSerializer.DeserializeScriptableObject<T>(jsonStr);
            }
        }
    }   
}

