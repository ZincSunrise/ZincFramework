using System;
using System.IO;
using System.Text.Json;
using UnityEngine;
using ZincFramework.Json;


namespace ZincFramework
{
    namespace Json
    {
        namespace Serialization
        {
            public static class JsonSelector
            {
                public static string Serialize(object data, Type type, JsonSerializeType jsonSerializeType) => jsonSerializeType switch
                {
                    JsonSerializeType.JsonSerializer => JsonSerializer.Serialize(data, type),
                    JsonSerializeType.NewtonSoft => Newtonsoft.Json.JsonConvert.SerializeObject(data, type, null),
                    JsonSerializeType.JsonUtility => JsonUtility.ToJson(data),
                    _ => throw new NotSupportedException($"不支持当前的{jsonSerializeType}类型")
                };


                public static void Serialize(object data, Type type, Stream stream, JsonSerializeType jsonSerializeType)
                {
                    switch (jsonSerializeType)
                    {
                        case JsonSerializeType.JsonSerializer:
                            JsonSerializer.Serialize(stream, data, type);
                            break;
                        case JsonSerializeType.NewtonSoft:
                        case JsonSerializeType.JsonUtility:
                            using (StreamWriter textWriter = new StreamWriter(stream))
                            {
                                textWriter.Write(Serialize(data, type, jsonSerializeType));
                            }
                            break;
                    }               
                }

                public static T Deserialize<T>(string jsonStr, JsonSerializeType jsonSerializeType) => jsonSerializeType switch
                {
                    JsonSerializeType.JsonSerializer => DeserializeDocument<T>(jsonStr),
                    JsonSerializeType.NewtonSoft => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr),
                    JsonSerializeType.JsonUtility => JsonUtility.FromJson<T>(jsonStr),
                    _ => throw new NotSupportedException($"不支持当前的{jsonSerializeType}类型")
                };

                public static object Deserialize(string jsonStr, Type type, JsonSerializeType jsonSerializeType) => jsonSerializeType switch
                {
                    JsonSerializeType.JsonSerializer => DeserializeDocument(jsonStr, type),
                    JsonSerializeType.NewtonSoft => Newtonsoft.Json.JsonConvert.DeserializeObject(jsonStr, type),
                    JsonSerializeType.JsonUtility => JsonUtility.FromJson(jsonStr, type),
                    _ => throw new NotSupportedException($"不支持当前的{jsonSerializeType}类型"),
                };

                public static T DeserializeScriptableObject<T>(string jsonStr) where T : ScriptableObject
                {
                    var data = ScriptableObject.CreateInstance<T>();
                    JsonUtility.FromJsonOverwrite(jsonStr, data);
                    return data;
                }

                private static T DeserializeDocument<T>(string jsonStr)
                {
                    using (JsonDocument jsonDocument = JsonDocument.Parse(jsonStr))
                    {
                        return jsonDocument.Deserialize<T>();
                    }             
                }

                private static object DeserializeDocument(string jsonStr, Type type)
                {
                    using (JsonDocument jsonDocument = JsonDocument.Parse(jsonStr))
                    {
                        return jsonDocument.Deserialize(type);
                    }
                }
            }
        }
    }
}

