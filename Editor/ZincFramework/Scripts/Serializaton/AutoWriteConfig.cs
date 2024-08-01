using System.IO;
using UnityEngine;
using ZincFramework.Load.Editor;


namespace ZincFramework
{
    namespace Serialization
    {
        [CreateAssetMenu(fileName = "AutoWriteConfig", menuName = "GameTool/AutoWriteConfig")]
        public class AutoWriteConfig : ScriptableObject
        {
            public static AutoWriteConfig ExcelDefault { get; private set; }
            public static AutoWriteConfig ProtocolDefault { get; private set; }


            private void OnEnable()
            {
                ExcelDefault = ExcelDefault == null ? AssetDataManager.LoadAssetAtPath<AutoWriteConfig>(Path.Combine(AssetDataManager.ScriptLoadPath, "ExcelProfile", "Temp", "ExcelConfig")) : ExcelDefault;
                ProtocolDefault = ProtocolDefault == null ? AssetDataManager.LoadAssetAtPath<AutoWriteConfig>(Path.Combine(AssetDataManager.ScriptLoadPath, "ProtocolTool", "Temp", "ProtocolConfig")) : ProtocolDefault;
            }

            public string[] UsingNamespaces => _usingNamespaces;

            [SerializeField]
            private string[] _usingNamespaces;


            public int StartLine => _startLine;

            [SerializeField]
            private int _startLine = 6;
        }
    }
}