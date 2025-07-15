using System.IO;
using UnityEditor;
using UnityEngine;

namespace ZincFramework.MVC.Editor
{
    public static class MVCGenerator
    {
        private readonly static string _loadPath = "Assets/Editor/ZincFramework/MVC/Temps";

        public static void GenerateMVC(string path, string name, MVCGenerateType generateType)
        {
            GenerateModel(path, name);
            GenerateMediator(path, name);

            if(generateType == MVCGenerateType.All)
            {
                GenerateProcessor(path, name);
            }

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }

        private static void GenerateModel(string path, string name)
        {
            var codeAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"{_loadPath}/ModelTemp.txt");
            string codeStr = codeAsset.text;
            codeStr = string.Format(codeStr, name);

            File.WriteAllText(Path.Combine(path, $"{name}Model.cs"), codeStr);
        }

        private static void GenerateMediator(string path, string name)
        {
            var codeAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"{_loadPath}/MediatorTemp.txt");
            string codeStr = codeAsset.text;
            codeStr = string.Format(codeStr, name);

            File.WriteAllText(Path.Combine(path, $"{name}Mediator.cs"), codeStr);
        }

        private static void GenerateProcessor(string path, string name) 
        {
            var codeAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"{_loadPath}/ProcessorTemp.txt");
            string codeStr = codeAsset.text;
            codeStr = string.Format(codeStr, name);

            File.WriteAllText(Path.Combine(path, $"{name}Processor.cs"), codeStr);
        }
    }
}