using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ZincFramework
{
    public static class FrameworkPaths
    {
        public static string ProfilePath { get; } = Path.Combine(Application.streamingAssetsPath, "Profile");

        public static string SavePath { get; } = Path.Combine(Application.persistentDataPath, "SavesDirectory");

        public static string FrameworkPath { get; } = Path.Combine(Application.dataPath , "Scripts", "ZincFramework");

#if UNITY_EDITOR
        public static string FrameworkEditorPath { get; } = Path.Combine(Application.dataPath , "Editor" ,"Scripts", "ZincFramework");
#endif
    }
}
