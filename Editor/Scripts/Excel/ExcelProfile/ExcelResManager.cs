using System.IO;
using UnityEditor;
using ZincFramework.Excel;


namespace ZincFramework
{
    namespace Serialization
    {
        public class ExcelResManager : BaseSafeSingleton<ExcelResManager>
        {
            public AutoWriteConfig ExcelDefault { get; private set; }

            public AutoWriteConfig ProtocolDefault { get; private set; }

            public ExcelSytleData DefaultStyleData { get; private set; }

            private ExcelResManager()
            {
                string path = "Assets/Editor/ZincFramework/Excel/Temp";
                ExcelDefault = AssetDatabase.LoadAssetAtPath<AutoWriteConfig>(Path.Combine(path, "ExcelConfig.asset"));
                DefaultStyleData = AssetDatabase.LoadAssetAtPath<ExcelSytleData>(Path.Combine(path, "DefaultStyleData.asset"));
            }
        }
    }
}