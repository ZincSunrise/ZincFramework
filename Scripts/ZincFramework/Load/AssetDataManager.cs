using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UIElements;


namespace ZincFramework
{
    namespace Load
    {
        namespace Editor
        {
            public static class AssetDataManager
            {
                private readonly static string _loadPath = "Assets";

                public static string FrameworkLoadPath { get; } = Path.Combine("Editor", "ZincFramework");

                public static string ScriptLoadPath  { get; } = Path.Combine("Editor", "ZincFramework", "Scripts");

                public static T LoadAssetAtPath<T>(string name) where T : Object
                {
#if UNITY_EDITOR
                    T asset = null;
                    string extension = GetExtension<T>();

                    asset = AssetDatabase.LoadAssetAtPath<T>(Path.Combine(_loadPath, name + extension));

                    if (asset == null)
                    {
                        extension = GetAnotherExtension<T>();
                        asset = AssetDatabase.LoadAssetAtPath<T>(Path.Combine(_loadPath, name + extension));
                    }
                    return asset;
#else
        return null;
#endif
                }


                /// <summary>
                /// Load All asset which is this type
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <returns></returns>
                public static T[] LoadAllTypeAsset<T>() where T : Object
                {
#if UNITY_EDITOR
                    string[] guid = AssetDatabase.FindAssets("t:" + typeof(T).Name);
                    T[] values = new T[guid.Length];
                    for (int i = 0; i < guid.Length; i++)
                    {
                        values[i] = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid[i]));
                    }

                    return values;
#else
        return null;
#endif
                }

                private static string GetExtension<T>() => typeof(T) switch
                {
                    var t when t == typeof(GameObject) => ".prefab",
                    var t when t == typeof(Material) => ".mat",
                    var t when t == typeof(Texture) || t == typeof(Texture2D) || t == typeof(Sprite) => ".png",
                    var t when t == typeof(AudioClip) => ".mp3",
                    var t when t == typeof(StyleSheet) => ".uss",
                    var t when t == typeof(VisualTreeAsset) => ".uxml",
                    var t when typeof(ScriptableObject).IsAssignableFrom(t) => ".asset",
                    _ => string.Empty
                };


                private static string GetAnotherExtension<T>() => typeof(T) switch
                {
                    var t when t == typeof(Texture) || t == typeof(Texture2D) => ".jpg",
                    var t when t == typeof(Sprite) => ".jpg",
                    var t when t == typeof(AudioClip) => ".wav",
                    _ => string.Empty
                };

                public static Sprite LoadSprite(string name, string spriteName)
                {
#if UNITY_EDITOR
                    Object[] objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(_loadPath + name + ".spriteatlasv2");
                    if (objects == null)
                    {

                    }
                    foreach (Object obj in objects)
                    {
                        if (obj.name == spriteName)
                        {
                            return obj as Sprite;
                        }
                    }
                    return null;
#else
        return null;
#endif
                }

                public static Dictionary<string, Sprite> LoadSprites(string name)
                {
#if UNITY_EDITOR
                    Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
                    Object[] objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(_loadPath + name + ".spriteatlasv2");
                    foreach (Object obj in objects)
                    {
                        spriteDic.Add(obj.name, obj as Sprite);
                    }
                    return spriteDic;
#else
        return null;
#endif
                }
            }
        }
    }
}
