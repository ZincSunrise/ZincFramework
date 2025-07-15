using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace ZincFramework.LoadServices.Editor
{
    public static class AssetDataManager
    {
#if UNITY_EDITOR
        private readonly static string _loadPath = "Assets";

        public static string FrameworkLoadPath { get; } = Path.Combine("Editor", "ZincFramework");

        private static readonly Dictionary<string, Object> _loadedObject = new Dictionary<string, Object>();

        public static T LoadAssetAtPath<T>(string name) where T : Object
        {
            string saveName = name + typeof(T).Name;
#if UNITY_EDITOR
            if (!_loadedObject.TryGetValue(saveName, out var asset))
            {
                string extension = GetExtension<T>();

                asset = AssetDatabase.LoadAssetAtPath<T>(Path.Combine(_loadPath, name + extension));

                if (asset == null)
                {
                    extension = GetAnotherExtension<T>();
                    asset = AssetDatabase.LoadAssetAtPath<T>(Path.Combine(_loadPath, name + extension));
                }

                _loadedObject.Add(saveName, asset);
            }

            return asset as T;
#else
        return null;
#endif
        }

        private static string _nowSavePath;

        private static string _nowLoadPath;


        public static bool SaveAssetsInPanel<T>(T[] assets, string title, string directory, string[] assetNames) where T : Object
        {
            return SaveAssetsInPanel(assets, title, directory, assetNames, GetExtension<T>()[1..]);
        }

        public static bool SaveAssetsInPanel(Object[] assets, string title, string directory, string[] assetNames, string extension)
        {
            if (assets.Length != assetNames.Length)
            {
                return false;
            }

            _nowSavePath = EditorUtility.SaveFilePanel(title, directory, assetNames[0], extension);
            bool canSave = !string.IsNullOrEmpty(_nowSavePath);

            if (canSave)
            {
                for (int i = 0; i < assetNames.Length; i++)
                {
                    string relativePath = _nowSavePath.Substring(Application.dataPath.Length - "Assets".Length).Replace(assetNames[0], assetNames[i]);
                    AssetDatabase.CreateAsset(assets[i], relativePath);
                }

                AssetDatabase.SaveAssets();
            }

            _nowSavePath = null;
            return canSave;
        }

        public static bool SaveAssetInPanel<T>(T asset, string title, string directory, string defaultName) where T : Object
        {
            return SaveAssetInPanel(asset, title, directory, defaultName, GetExtension<T>()[1..]);
        }

        public static bool SaveAssetInPanel(Object asset, string title, string directory, string defaultName, string extension)
        {
            _nowSavePath = EditorUtility.SaveFilePanel(title, directory, defaultName, extension);
            bool canSave = !string.IsNullOrEmpty(_nowSavePath);

            if (canSave)
            {
                string relativePath = _nowSavePath.Substring(Application.dataPath.Length - "Assets".Length);
                AssetDatabase.CreateAsset(asset, relativePath);
                AssetDatabase.SaveAssets();
            }

            _nowSavePath = null;
            return canSave;
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
#endif
    }
}
