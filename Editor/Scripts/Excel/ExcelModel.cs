using System.IO;
using UnityEditor;
using UnityEngine;
using ZincFramework.Excel;


namespace ZincFramework.Serialization
{
    public class ExcelModel : BaseSafeSingleton<ExcelModel>
    {
        public static string ExcelPath { get; } = Application.dataPath + "/ArtRes/Excel/";

        public const string Extension = ".xlsx";

        public AutoWriteConfig ExcelDefault { get; private set; }

        public AutoWriteConfig ProtocolDefault { get; private set; }

        public ExcelSytleData DefaultStyleData { get; private set; }

        private ExcelModel()
        {
            string path = "Assets/Editor/ZincFramework/Excel/Temp";
            ExcelDefault = AssetDatabase.LoadAssetAtPath<AutoWriteConfig>(Path.Combine(path, "ExcelConfig.asset"));
            DefaultStyleData = AssetDatabase.LoadAssetAtPath<ExcelSytleData>(Path.Combine(path, "DefaultStyleData.asset"));
        }
    }
}