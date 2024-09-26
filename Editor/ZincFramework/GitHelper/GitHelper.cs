using System;
using System.IO;
using UnityEditor;
using UnityEngine;



namespace ZincFramework.EditorInUnity.GitHelper
{
    internal static class GitHelper
    {
        [MenuItem("GameTool/GitHelper/RemoveAllMetaFile")]
        private static void RemoveAllMetaFile()
        {
            string directoryPath = EditorUtility.OpenFolderPanel("选择你的git文件夹", Application.dataPath, "GitFile");
            if (!string.IsNullOrEmpty(directoryPath))
            {
                if (Directory.Exists(directoryPath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                    DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();

                    if (Array.Find(directoryInfos, x => x.Name == ".git") == null)
                    {
                        return;
                    }

                    for (int i = 0; i < directoryInfos.Length; i++) 
                    {
                        if (directoryInfos[i].Name != "git")
                        {
                            FileInfo[] fileInfos = directoryInfo.GetFiles("*.meta", SearchOption.AllDirectories);
                            foreach (var file in fileInfos)
                            {
                                file.Delete();
                            }
                        }
                    }
                }
            }
        }
    }
}