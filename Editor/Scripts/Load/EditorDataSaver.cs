using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using ZincFramework.Binary.Serialization;




namespace ZincFramework.LoadServices
{
    public static class EditorDataSaver
    {
        public static void SaveText<T>(string target, T obj)
        {
            string path = Path.Combine(Application.dataPath, target);
            using (FileStream fileStream = File.Create(path))
            {
                try
                {
                    BinarySerializer.Serialize<T>(obj, fileStream);
                    AssetDatabase.Refresh();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    File.Delete(path);
                }
            }
        }


        public static T LoadText<T>(string target) where T : new()
        {
            using (FileStream fileStream = File.OpenRead(Path.Combine(Application.dataPath, target)))
            {
                return BinarySerializer.Deserialize<T>(fileStream, () => new T());
            }
        }
    }
}