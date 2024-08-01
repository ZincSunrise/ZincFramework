using LitJson;
using System;
using System.Text;
using UnityEngine;
using ZincFramework.Json;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Json
        {
            public static class JsonSerializer
            {
                private readonly static StringBuilder _jsonStr = new StringBuilder();

                public static string Serialize(object data, JsonSerializeType jsonSerializeType = JsonSerializeType.LitJson)
                {
                    _jsonStr.Clear();
                    switch (jsonSerializeType)
                    {
                        case JsonSerializeType.LitJson:
                            _jsonStr.Append(JsonMapper.ToJson(data));
                            break;
                        case JsonSerializeType.JsonUtility:
                            _jsonStr.Append(JsonUtility.ToJson(data));
                            break;

                    }

                    return _jsonStr.ToString();
                }

                public static object Deserialize(string jsonStr, Type type, JsonSerializeType jsonSerializeType = JsonSerializeType.LitJson)
                {
                    return jsonSerializeType == JsonSerializeType.LitJson ? 
                        JsonMapper.ToObject(jsonStr, type) : 
                        JsonUtility.FromJson(jsonStr, type);
                }

                public static T DeserializeScriptableObject<T>(string jsonStr) where T : ScriptableObject
                {
                    var data = ScriptableObject.CreateInstance<T>();
                    JsonUtility.FromJsonOverwrite(jsonStr, data);
                    return data;
                }
            }
        }
    }
}

